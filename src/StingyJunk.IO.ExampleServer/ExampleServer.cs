namespace StingyJunk.IO.ExampleServer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Console;

    internal static class ExampleServer
    {
        private const string TRIGGER_TEXT = "SENDING SOME DATA FROM";
        private static ConsoleWindow _consoleWindow;
        private const string HEADER_AREA = "HeaderArea";
        private const int HEADER_LINES = 4;
        private const string STATS_TOTAL_CONNECTION_AREA = "TotalConnectionsArea";
        private const string STATS_ACTIVE_CONNECTION_AREA = "ActiveConnectionsArea";
        private const string LOG_AREA_DEMARCATION = "LogAreaDemarcation";
        private const string LOG_AREA = "LogArea";

        private static void Main()
        {
            Thread.CurrentThread.Name = nameof(ExampleServer);

            var headerArea = new DisplayArea(HEADER_AREA, 0, 0, HEADER_LINES - 1, Console.WindowWidth);
            var totalConnectionArea = new DisplayArea(STATS_TOTAL_CONNECTION_AREA, headerArea.Bottom + 1, 0, headerArea.Bottom + 1, Console.WindowWidth) {Cycle = true};
            var activeConnectionArea =
                new DisplayArea(STATS_ACTIVE_CONNECTION_AREA, totalConnectionArea.Bottom + 1, 0, totalConnectionArea.Bottom + 1, Console.WindowWidth) {Cycle = true};
            var logAreaDemarcation = new DisplayArea(LOG_AREA_DEMARCATION, activeConnectionArea.Bottom + 1, 0, activeConnectionArea.Bottom + 1, Console.WindowWidth) {Cycle = true};
            var logArea = new DisplayArea(LOG_AREA, logAreaDemarcation.Bottom + 1, 0, Console.WindowHeight - logAreaDemarcation.Bottom, Console.WindowWidth) {Cycle = true};

            _consoleWindow = new ConsoleWindow(new[] {headerArea, totalConnectionArea, activeConnectionArea, logAreaDemarcation, logArea});

            _consoleWindow.Clear();
            _consoleWindow.WriteLine($"Locating hosting information...", Flair.Log, HEADER_AREA);
            _consoleWindow.WriteLine($"-----------------------LOG-------------------------", Flair.Log, LOG_AREA_DEMARCATION);

            var service = new AsyncTcpListener("Listener Simulator", 20000);

            Task.Run(() =>
            {
                service.DebugMessageEventHandler += (sender, e) => { _consoleWindow.WriteLine($"{e.Timestamp.TimeOfDay} - {e.SourceName} - {e.Message}", Flair.Log, LOG_AREA); };
                service.InfoMessageEventHandler += (sender, e) => { _consoleWindow.WriteLine($"{e.Timestamp.TimeOfDay} - {e.SourceName} - {e.Message}", Flair.Log, LOG_AREA); };
                service.ConnectionInfoMessageEventHandler += (sender, e) =>
                {
                    _consoleWindow.WriteLine($"{e.Timestamp.TimeOfDay} - Client Connections {e.Stats.ClientConnectionsCount}", Flair.Log, STATS_TOTAL_CONNECTION_AREA);
                    _consoleWindow.WriteLine($"{e.Timestamp.TimeOfDay} - Active Connections {e.Stats.ActiveClientCount}", Flair.Log, STATS_ACTIVE_CONNECTION_AREA);
                };
                service.ErrorMessageEventHandler += (sender, e) =>
                {
                    _consoleWindow.WriteLine($"{e.Timestamp.TimeOfDay} - {e.SourceName} - {e.Message} - {e.Exception}", Flair.Error, LOG_AREA);
                };
                service.ResponseEventHandler += (sender, eventArgs) =>
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

            _consoleWindow.WriteLine("Press ESC to stop listening", Flair.Success, HEADER_AREA);
            _consoleWindow.WaitForKey(ConsoleKey.Escape);
            service.Stop();
            _consoleWindow.Close();
        }
    }
}