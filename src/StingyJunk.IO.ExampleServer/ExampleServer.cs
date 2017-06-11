namespace StingyJunk.IO.ExampleServer
{
    using System;
    using System.Threading.Tasks;
    using System.Threading;
    using Console;

    
    internal static class ExampleServer
    {
        private const string TRIGGER_TEXT = "SENDING SOME DATA FROM";
        private static ConsoleWindow _consoleWindow;
        private const string HEADER_AREA = "HeaderArea";
        private const string LOG_AREA = "LogArea";

        private static void Main()
        {
            Thread.CurrentThread.Name = nameof(ExampleServer);
            var headerArea = new DisplayArea(HEADER_AREA, 0, 0, 7, Console.WindowWidth);
            var logArea = new DisplayArea(LOG_AREA, headerArea.Bottom + 1, 0, Console.WindowHeight - (headerArea.Bottom + 1), Console.WindowWidth)
            {
                Cycle = true
            };

            _consoleWindow = new ConsoleWindow(new[] { headerArea, logArea });

            _consoleWindow.Clear();
            _consoleWindow.WriteLine($"Locating hosting information...",Flair.Log, HEADER_AREA);

            var service = new AsyncTcpListener("Listener Simulator", 20000);
            
            Task.Run(() =>
            {
                service.DiagMessage += (sender, e) =>
                {
                    _consoleWindow.WriteLine($"{e.Timestamp} - {e.SourceName} - {e.DiagnosticMessage}", Flair.Log, LOG_AREA);
                };
                service.InfoMessage += (sender, e) =>
                {
                    _consoleWindow.WriteLine($"{e.Timestamp} - {e.SourceName} - {e.InfoMessage}",Flair.Log, HEADER_AREA);
                };
                service.ResponseHandler += (sender, eventArgs) =>
                {
                    if (eventArgs == null)
                    {
                        throw new ArgumentNullException(nameof(eventArgs));
                    }

                    _consoleWindow.WriteLine($"Got {eventArgs.RequestMessage}", Flair.Log, LOG_AREA);

                    if (string.IsNullOrWhiteSpace(eventArgs.RequestMessage))
                    {
                        eventArgs.ResponseMessage = "¿Huh?";
                    }
                    else if (eventArgs.RequestMessage.StartsWith(TRIGGER_TEXT, StringComparison.OrdinalIgnoreCase))
                    {
                        eventArgs.ResponseMessage = $"REPLYING FROM Serverside for {eventArgs.RequestMessage.Replace(TRIGGER_TEXT, string.Empty)}";
                    }
                    else
                    {
                        eventArgs.ResponseMessage = "W/E";
                    }
                };
                service.Run();
                return Task.CompletedTask;
            });

            _consoleWindow.WriteLine("Press ENTER to stop listening", Flair.Success, HEADER_AREA);
            Console.ReadLine();
            service.Stop();
        }
    }
}