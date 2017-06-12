namespace StingyJunk.Console.Example
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using StingyJunk.Extensions;

    internal static class MultiAreaExampleConsoleWindow
    {
        private static ConsoleWindow _consoleWindow;
        private static LinkedList<Flair> _flairs;
        private static LinkedListNode<Flair> _lastFlair;
        private static void Main()
        {
            Thread.CurrentThread.Name = nameof(SimpleExampleConsoleWindow);
            _flairs = BuildFlairLibrary();

            var header = new DisplayArea("Header", 0, 0, 5, Console.WindowWidth);

            var scrollingMessages = new DisplayArea("Messages", header.Bottom + 1, 0,
                Console.WindowHeight - (header.Bottom + 1), Console.WindowWidth);
            scrollingMessages.Cycle = true;


            _consoleWindow = new ConsoleWindow(new[] { header, scrollingMessages });
            WriteHeader($"This is the head");

            Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    WriteMessage($"Detail record {i} at {DateTime.Now.TimeOfDay}");
                    Thread.Sleep(100);
                }
            });

            //WriteMessage($"This is any kind of message {DateTime.Now.TimeOfDay}");
            WriteHeader($"Press ESC to close");
            _consoleWindow.WaitForKey(ConsoleKey.Escape);
            _consoleWindow.Close();
        }

        private static void WriteHeader(string msg, bool isErr = false)
        {
            var flair = isErr ? Flair.Error : Flair.Success;
            _consoleWindow.WriteLine(msg, flair, "Header");
        }


        private static void WriteMessage(string msg)
        {
            LinkedListNode<Flair> flair = null;
            if (_lastFlair == null)
            {
                _lastFlair = _flairs.First;
                flair = _lastFlair;
            }
            else
            {
                flair = _lastFlair.NextOrFirst();
                _lastFlair = flair;
            }
            

            _consoleWindow.WriteLine(msg, flair.Value, "Messages");
        }

        private static LinkedList<Flair> BuildFlairLibrary()
        {
            var flairs = new LinkedList<Flair>();
            foreach (ConsoleColor foregroundColor in Enum.GetValues(typeof(ConsoleColor)))
            {   
                var backgroundColor = ConsoleColor.Black;
                if ((int) foregroundColor < 10)
                {
                    backgroundColor = ConsoleColor.White;
                }
                
                var flair = new Flair(foregroundColor, backgroundColor);
                if (flairs.Count == 0)
                {
                    flairs.AddFirst(flair);
                }
                else
                {
                    flairs.AddAfter(flairs.Last, flair);        
                }
            }
            
            return flairs;
        }

    }
}