namespace StingyJunk.Console.Example
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class MultiAreaExampleConsoleWindow
    {
        private static ConsoleWindow _consoleWindow;
        
        private static void Main()
        {
            Thread.CurrentThread.Name = nameof(SimpleExampleConsoleWindow);


            var header = new DisplayArea("Header", 0, 0, 5, Console.WindowWidth);

            var scrollingMessages = new DisplayArea("Messages", header.Bottom + 1, 0,
                Console.WindowHeight - (header.Bottom + 1), Console.WindowWidth);
            scrollingMessages.Cycle = true;


            _consoleWindow = new ConsoleWindow(new[] {header, scrollingMessages});
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


        private static void WriteMessage(string msg, bool isErr = false)
        {
            var flair = isErr ? Flair.Error : Flair.Log;
            _consoleWindow.WriteLine(msg, flair, "Messages");
        }
    }
}