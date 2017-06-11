namespace StingyJunk.Console
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
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
            Debug.WriteLine($"start request to write {groupMessages.ToNewLineList()}");
            VerifyDisplayAreas(displayAreaName);

            var consoleMessages = groupMessages
                .Select(msg =>
                    new ConsoleMessage(msg, flair ?? DefaultFlair, DeriveWritePosition(displayAreaName)))
                .ToList();

            _queuedWriter.WriteMessages(consoleMessages);

            Debug.WriteLine($"start request to write {groupMessages.ToNewLineList()}");
        }

        public void WriteLine(string message, Flair flair = null, string displayAreaName = DEFAULT_DISPLAY_AREA)
        {
            Debug.WriteLine($"start request to write {message}");
            VerifyDisplayAreas(displayAreaName);

            var writePosition = DeriveWritePosition(displayAreaName);
            _queuedWriter.WriteMessage(writePosition, message, flair ?? DefaultFlair);

            Debug.WriteLine($"end request to write {message}");
        }

        private Position DeriveWritePosition(string displayAreaName)
        {
            var debugId = Guid.NewGuid();
            Debug.WriteLine($"Deriving write position for area '{displayAreaName}'");
            Position writePosition = null;

            var displayArea = _displayAreas[displayAreaName];
            if (displayArea.WritePosition == null)
            {
                Debug.WriteLine($"WritePosition {debugId} for {displayArea.Name} is not set. This is the first write");
                writePosition = new Position(displayArea.Top, displayArea.Left);
            }
            else
            {
                Debug.WriteLine($"WritePosition {debugId} for {displayArea.Name} was {displayArea.WritePosition}");

                if (displayArea.Cycle && displayArea.WritePosition.Top >= displayArea.Bottom)
                {
                    writePosition = new Position(displayArea.Top, displayArea.Left);
                }

                if (writePosition == null)
                {
                    writePosition = new Position(displayArea.WritePosition.Top + 1, displayArea.Left);
                }
            }

            Debug.WriteLine($"Write position {debugId} for {displayArea.Name} setting to {writePosition}");
            displayArea.SetWritePosition(writePosition);
            return writePosition;

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
    }
}