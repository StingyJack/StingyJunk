namespace ConsoleHelpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;

    public class Options
    {
        public int? MaxDisplayLines { get; }
        public ConsoleColor DefaultForeground { get; }
        public int? HeaderLineCount { get; }
        public int? FirstDetailLineNumber { get; }
        public List<DisplayBoundary> DisplayBoundaries { get; set; }


        public Options(ConsoleColor defaultForeground, int? maxDisplayLines = null,
            int? headerLineCount = null, int? firstDetailLineNumber = null)
        {
            DefaultForeground = defaultForeground;
            MaxDisplayLines = maxDisplayLines.HasValue ? maxDisplayLines.Value : Console.WindowHeight;
            HeaderLineCount = headerLineCount.HasValue ? headerLineCount.Value : 1;
            FirstDetailLineNumber = firstDetailLineNumber.HasValue ? firstDetailLineNumber.Value :  HeaderLineCount + 1;
        }
    }

    public class DisplayBoundary
    {
        public int Top { get; set; }
        public int Bottom { get; set; }
        public bool Cycle { get; set; }

    }

    public class QueuedWriter
    {
        public static Options DefaultOptions => new Options(ConsoleColor.Cyan, 25, 5, 6);

        private readonly Options _options;
        private bool _acceptNewItems;
        private bool _keepWriting;
        private Thread _queueWriter;

        private readonly ConcurrentQueue<ConsoleMessage> _queuedMessages = new ConcurrentQueue<ConsoleMessage>();
        private const int NON_PREFERRED_LINE_NUMBER = -1;

        public QueuedWriter(Options options)
        {
            _options = options;
            StartWorking();
        }

        private void StartWorking()
        {
            _acceptNewItems = true;
            _keepWriting = true;
            if (_queueWriter == null || _queueWriter.IsAlive == false)
            {
                _queueWriter = new Thread(QueueWriter);
            }
            _queueWriter.Start();
        }

        public void Wd(string message, ConsoleColor? color = null, int? lineNumber = null) => WriteDetail(message, color, lineNumber);

        public void WriteDetail(string message, ConsoleColor? color = null, int? preferredLineNumber = null)
        {
            if (_acceptNewItems == false)
            {
                return;
            }

            var lineNumber = preferredLineNumber.HasValue ? preferredLineNumber.Value : NON_PREFERRED_LINE_NUMBER;
            _queuedMessages.Enqueue(new ConsoleMessage(message, color.HasValue ? color.Value : _options.DefaultForeground,
                LineType.Detail, lineNumber));
        }

        public void Wh(string message, ConsoleColor? color = null, int? lineNumber = null) => WriteHeader(message, color, lineNumber);

        public void WriteHeader(string message, ConsoleColor? color = null, int? preferredLineNumber = null, LineType? lineType = null)
        {
            if (_acceptNewItems == false)
            {
                return;
            }

            var lineNumber = preferredLineNumber.HasValue ? preferredLineNumber.Value : NON_PREFERRED_LINE_NUMBER;
            _queuedMessages.Enqueue(new ConsoleMessage(message, color.HasValue ? color.Value : _options.DefaultForeground,
                LineType.Header, lineNumber));
        }

        public void Restart()
        {
            StartWorking();
        }

        public void Halt(bool flushFirst = true)
        {
            _acceptNewItems = false;
            //this will stop writes immediately if set to false. 
            //otherwise it will empty the queue
            _keepWriting = flushFirst;
        }

        private void QueueWriter()
        {
            var lastLineNumber = Console.CursorTop;
            while (_keepWriting)
            {
                ConsoleMessage cm;
                if (_queuedMessages.TryDequeue(out cm) == false)
                {
                    Thread.Sleep(100);
                    continue;
                }

                var lineNumber = cm.LineNumber != NON_PREFERRED_LINE_NUMBER ? cm.LineNumber : lastLineNumber;
                if (cm.LineType == LineType.Detail && _options.FirstDetailLineNumber.HasValue)
                {
                    if (lineNumber > _options.MaxDisplayLines)
                    {
                        lineNumber = _options.FirstDetailLineNumber.Value;
                    }
                }
                Console.SetCursorPosition(0, lineNumber);
                Console.ForegroundColor = cm.Color.HasValue ? cm.Color.Value : _options.DefaultForeground;
                Console.WriteLine(cm.Message);
                lastLineNumber = Console.CursorTop;
            }

        }
    }

    internal class ConsoleMessage
    {
        public string Message { get; set; }
        public ConsoleColor? Color { get; set; }
        public LineType LineType { get; set; }
        public int LineNumber { get; set; }

        public ConsoleMessage(string message, ConsoleColor color, LineType lineType, int lineNumber)
        {
            Message = message;
            Color = color;
            LineType = lineType;
            LineNumber = lineNumber;
        }
    }

    public enum LineType
    {
        Detail,
        Header
    }
}