namespace StingyJunk.IO.ExampleServer
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using ConsoleHelpers;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    internal class ExampleServer
    {
        private const string TRIGGER_TEXT = "SENDING SOME DATA FROM";
        private static readonly ConsoleHelpers _ch = new ConsoleHelpers();

        private static void Main(string[] args)
        {
            var service = new AsyncTcpListener("Listener Simulator", 20000);
            Task.Run(async () =>
            {
                service.DiagMessage += Service_DiagMessage;
                service.ResponseHandler += (sender, eventArgs) =>
                {
                    if (eventArgs == null)
                    {
                        throw new ArgumentNullException(nameof(eventArgs));
                    }

                    _ch.Cwl($"Got {eventArgs.RequestMessage}", ConsoleColor.Cyan);

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
                await service.RunAsync();
                return Task.CompletedTask;
            });

            _ch.Cwl("Press ENTER to stop listening", ConsoleColor.Yellow);
            Console.ReadLine();
            service.Stop();
        }

        private static void Service_DiagMessage(object sender, DiagMessageEventArgs e)
        {
            _ch.Cwl($"{e.Timestamp} - {e.SourceName} - {e.DiagnosticMessage}", ConsoleColor.DarkYellow);
        }
    }
}