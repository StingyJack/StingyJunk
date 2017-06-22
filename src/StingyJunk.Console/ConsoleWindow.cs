namespace StingyJunk.Console
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Collections;
    using Extensions;

    public partial class ConsoleWindow
    {
        public ImmutableDictionary<string, DisplayArea> DisplayAreas => _displayAreas.ToImmutableDictionary();
        private readonly OicDic<DisplayArea> _displayAreas = new OicDic<DisplayArea>();
        private readonly QueuedWriter _queuedWriter;
        public Flair DefaultFlair { get; set; } = new Flair(Console.ForegroundColor, Console.BackgroundColor);

        public ConsoleWindow()
        {
            AddDisplayAreasIn(new[] { DefaultArea });
            _queuedWriter = new QueuedWriter();
        }

        public ConsoleWindow(IEnumerable<DisplayArea> displayAreas)
        {
            AddDisplayAreasIn(displayAreas);
            EnsureDisplayAreasAreDistinct();
            _queuedWriter = new QueuedWriter();
        }


        public void WriteLines(List<string> groupMessages, Flair flair = null, string displayAreaName = DEFAULT_DISPLAY_AREA)
        {
            Dwl($"start request to write {groupMessages.ToNewLineList()}");
            VerifyDisplayAreas(displayAreaName);

            var consoleMessages = groupMessages
                .Select(msg =>
                    new ConsoleMessage(msg, flair ?? DefaultFlair, DeriveWritePosition(displayAreaName, msg)))
                .ToList();

            _queuedWriter.WriteMessages(consoleMessages);

            Dwl($"start request to write {groupMessages.ToNewLineList()}");
        }

        public void WriteLine(string message, Flair flair = null, string displayAreaName = DEFAULT_DISPLAY_AREA)
        {
            Dwl($"start request to write {message}");
            VerifyDisplayAreas(displayAreaName);

            var writePosition = DeriveWritePosition(displayAreaName, message);
            _queuedWriter.WriteMessage(writePosition, message, flair ?? DefaultFlair);

            Dwl($"end request to write {message}");
        }

        private Position DeriveWritePosition(string displayAreaName, string currentMessage)
        {
            var debugId = Guid.NewGuid();
            Dwl($"Deriving write position for area '{displayAreaName}'");
            Position writePosition = null;

            var displayArea = _displayAreas[displayAreaName];
            lock (displayArea)
            {
                if (displayArea.WritePosition == null)
                {
                    Dwl($"WritePosition {debugId} for {displayArea.Name} is not set. This is the first write");
                    writePosition = new Position(displayArea.Top, displayArea.Left);
                }
                else
                {
                    Dwl($"WritePosition {debugId} for {displayArea.Name} was {displayArea.WritePosition} with length {displayArea.MessageLinesForWritePosition}");

                    //  get the line count for the last message to use for offsetting this one
                    //  the line offset is always one less than that, but never below 0
                    var lineOffset = displayArea.MessageLinesForWritePosition;

                    if (displayArea.Cycle && displayArea.WritePosition.Top + lineOffset > displayArea.Bottom)
                    {
                        writePosition = new Position(displayArea.Top, displayArea.Left);
                    }

                    if (writePosition == null)
                    {
                        writePosition = new Position(displayArea.WritePosition.Top + lineOffset, displayArea.Left);
                    }
                }

                var lineCount = GetMessageLineCount(currentMessage);

                Dwl($"Write position {debugId} for {displayArea.Name} setting to {writePosition}");
                displayArea.SetWritePosition(writePosition, lineCount);
            }
            return writePosition;
        }

        private int GetMessageLineCount(string message )
        {
            //  if the write occupies multiple lines, the next write has to be offset to account for that.
            //
            //  if the message was 225 chars (without explicit newlines) and the console width is 110,
            //  the message would have occupied 3 lines
           

            var explicitLines = message.Split(new[]{Environment.NewLine}, StringSplitOptions.None);
            var lineCount = explicitLines.Length;
            foreach (var line in explicitLines)
            {
                
                lineCount += Convert.ToInt32(Math.Floor(line.Length / (decimal)ConsoleWidth()));
            }
            return lineCount;
        }

        private static int ConsoleWidth()
        {
            return Console.WindowWidth;
        }

        public void WaitForKey(ConsoleKey key)
        {
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == key))
            {
                // do something
            }
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void Close()
        {
            _queuedWriter.Halt();
            _displayAreas.Clear();
        }


        private void Dwl(string message, [CallerMemberName]string methodName = null)
        {
            Debug.WriteLine($"{nameof(ConsoleWindow)}.{methodName} - {DateTime.Now.TimeOfDay} - {message} ");
        }
    }
}