namespace StingyJunk.IO.ExampleClient
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Console;

    internal static partial class ExampleClient
    {
        private static ConsoleWindow _consoleWindow;
        private static readonly ConcurrentBag<OperationResult> _results = new ConcurrentBag<OperationResult>();
        private const string TRIGGER_TEXT = "SENDING SOME DATA FROM";
        private const string HEADER_AREA = "HeaderArea";
        private const string LOG_AREA = "LogArea";

        private static void Main()
        {
            Thread.CurrentThread.Name = nameof(ExampleClient);
            //  Pausing this so server can start first when debugging this and server at the same time.
            Thread.Sleep(2 * 1000);
            var headerArea = new DisplayArea(HEADER_AREA, 0, 0, 7, Console.WindowWidth);
            var logArea = new DisplayArea(LOG_AREA, headerArea.Bottom + 1, 0, Console.WindowHeight - (headerArea.Bottom + 1), Console.WindowWidth)
            {
                Cycle = true
            };

            _consoleWindow = new ConsoleWindow(new[] { headerArea, logArea });
            
            // ~16k is where the amount of completed connections in TIME_WAIT 
            //  that are delaying completion and the number of open connections
            //  exceeds the allowed sockets per port on this win10 machine.
            // More can probably be done with either better connection management, shorter timeouts, or 
            //  some registry tweaking. 
            const int RUN_COUNT = 1 * 100;
            const int MAXDOP = 5;

            _consoleWindow.WriteLine("Starting client nag. Press ESC to stop nagging", Flair.Warning, HEADER_AREA);
            //            var tasks = new List<Task>();
            var overallSw = Stopwatch.StartNew();

            Parallel.For(0, RUN_COUNT, new ParallelOptions { MaxDegreeOfParallelism = MAXDOP }, async i =>
            {
                var result = await ExecuteDataExchange(i);
                _results.Add(result);
            });

            overallSw.Stop();

            //          Task.WaitAll(tasks.ToArray()); //let any further console updates post before adding summary

            _consoleWindow.WriteLine($"Ran {RUN_COUNT} records in {overallSw.ElapsedMilliseconds}ms with MAXDOP of {MAXDOP}", Flair.Success, HEADER_AREA);
            var errors = _results.Where(r => r.Ex != null).ToArray();
            if (errors.Any())
            {
                _consoleWindow.WriteLine($"There were {errors.Length} errors", Flair.Error, HEADER_AREA);
                foreach (var err in errors)
                {
                    _consoleWindow.WriteLine($"\t client {err.ClientId}: {err.Ex}", Flair.Error, HEADER_AREA);
                }
            }

            var rate = overallSw.ElapsedMilliseconds / (decimal)RUN_COUNT;
            _consoleWindow.WriteLine($"Overall rate of {rate}ms per record", Flair.Success, HEADER_AREA);

            var sumResultTimes = _results.Sum(r => r.ElapsedMs);
            _consoleWindow.WriteLine($"Per item total time of {sumResultTimes}ms", Flair.Success, HEADER_AREA);

            //inner item rate should be larger than overall rate, as there are many items running at once.
            var innerItemRate = sumResultTimes / (decimal)RUN_COUNT;
            _consoleWindow.WriteLine($"Per item rate of {innerItemRate}ms per record", Flair.Success, HEADER_AREA);

            if (_results.Count != RUN_COUNT)
            {
                _consoleWindow.WriteLine($"Results count {_results.Count} doesnt match expected run count {RUN_COUNT}", Flair.Error, HEADER_AREA);
            }

            var noResponse = _results.Where(r => string.IsNullOrWhiteSpace(r.DataExchangeResult.ResponseMessage)).ToArray();
            if (noResponse.Length > 0)
            {
                _consoleWindow.WriteLine($"Results {string.Join(",", noResponse.Select(r => r.ClientId))} had empty responses", Flair.Error, HEADER_AREA);
            }
            else
            {
                var correctResponses = _results
                    .Where(r => r.DataExchangeResult.ResponseMessage.Equals($"REPLYING FROM Serverside for {r.RequestMessage.Replace(TRIGGER_TEXT, string.Empty)}",
                        StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                if (correctResponses.Length == RUN_COUNT)
                {
                    _consoleWindow.WriteLine($"All results had correct responses", Flair.Success, HEADER_AREA);
                }
                else
                {
                    _consoleWindow.WriteLine($"All results did not have correct responses", Flair.Error, HEADER_AREA);
                }
            }

            Console.ReadLine();
        }

        private static async Task<OperationResult> ExecuteDataExchange(int i)
        {
            var result = new OperationResult {ClientId = i};
            result.RequestMessage = $"{TRIGGER_TEXT} Clientside ident {result.ClientId}. OK?";
            try
            {
                var perItemSw = Stopwatch.StartNew();
                result.DataExchangeResult = await ExchangeData(result.RequestMessage, result.ClientId).ConfigureAwait(true);
                perItemSw.Stop();
                result.ElapsedMs = perItemSw.ElapsedMilliseconds;
                _consoleWindow.WriteLines(result.DataExchangeResult.Log, Flair.Log, LOG_AREA);
            }
            catch (Exception e)
            {
                result.Ex = e;
                _consoleWindow.WriteLine(e.Message, Flair.Error, HEADER_AREA);
            }
            return result;
        }

        private static async Task<DataExchangeResult> ExchangeData(string message, int clientId)
        {
            var dexResult = new DataExchangeResult();
            dexResult.Log.Add($"{clientId} - Connecting...");

            var tc = new TcpClient();
            tc.Connect("192.168.1.2", 20000);

            dexResult.Log.Add($"\t {clientId} - Getting Stream...");

            var stream = tc.GetStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };
            var reader = new StreamReader(stream);

            dexResult.Log.Add($"\t {clientId} - sending {message}");
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();

            if (stream.CanTimeout)
            {
                stream.ReadTimeout = 500;
            }

            try
            {
                dexResult.Log.Add($"\t {clientId} - Getting result...");
                var val = await reader.ReadLineAsync();
                dexResult.ResponseMessage = val;
                dexResult.Log.Add($"\t {clientId} - Didnt fail connection");
            }
            catch (IOException e) //this was happening sometimes.
            {
                //if (e.Message.IndexOf("the connected party did not properly respond after a period of time", StringComparison.Ordinal) < 0)
                //{
                //    throw;
                //}

                dexResult.Errors.Add("${clientId} - connection dropped");
                dexResult.ResponseMessage = e.ToString();
            }
            finally
            {
                tc.Dispose();
            }

            dexResult.Log.Add($"{clientId} - Completed");

            return dexResult;
        }
    }
}