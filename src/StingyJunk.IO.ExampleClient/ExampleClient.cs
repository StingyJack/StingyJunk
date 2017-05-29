namespace StingyJunk.IO.ExampleClient
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ConsoleHelpers;

    internal static partial class ExampleClient
    {
        private static readonly ConsoleHelpers _ch = new ConsoleHelpers();
        private static readonly ConcurrentBag<Result> _results = new ConcurrentBag<Result>();
        private const string TRIGGER_TEXT = "SENDING SOME DATA FROM";

        private static void Main()
        {
            //  Pausing this so server can start first when debugging this and server at the same time.
            Thread.Sleep(2* 1000); 

            // ~16k is where the amount of completed connections in TIME_WAIT 
            //  that are delaying completion and the number of open connections
            //  exceeds the allowed sockets per port on this win10 machine.
            // More can probably be done with either better connection management, shorter timeouts, or 
            //  some registry tweaking. 
            const int RUN_COUNT = 1 * 5000;
            const int MAXDOP = 5;
            
            _ch.Cwl("Starting client nag. Press ESC to stop nagging", ConsoleColor.Yellow);
            var tasks = new List<Task>();
            var overallSw = Stopwatch.StartNew();
            
            Parallel.For(0, RUN_COUNT, new ParallelOptions { MaxDegreeOfParallelism = MAXDOP }, (i) =>
            {
                var result = new Result { ClientId = i };
                result.RequestMessage  = $"{TRIGGER_TEXT} Clientside ident {result.ClientId}. OK?";
                var perItemSw = Stopwatch.StartNew();
                var t = ExchangeData(result.RequestMessage, result.ClientId);
                tasks.Add(t);
                result.ResponseMessage = t.Result;
                perItemSw.Stop();
                result.ElapsedMs = perItemSw.ElapsedMilliseconds;
                _results.Add(result);
            });
            
            overallSw.Stop();
            
            Task.WaitAll(tasks.ToArray()); //let any further console updates post before adding summary
            Console.SetCursorPosition(0,2);
            _ch.Cwl($"Ran {RUN_COUNT} records in {overallSw.ElapsedMilliseconds}ms with MAXDOP of {MAXDOP}");
            var rate = overallSw.ElapsedMilliseconds / (decimal)RUN_COUNT;
            _ch.Cwl($"Overall rate of {rate}ms per record");
            
            var sumResultTimes = _results.Sum(r => r.ElapsedMs);
            _ch.Cwl($"Per item total time of {sumResultTimes}ms");
            //inner item rate should be larger than overall rate, as there are many items running at once.
            var innerItemRate = sumResultTimes / (decimal)RUN_COUNT;
            _ch.Cwl($"Per item rate of {innerItemRate}ms per record");

            if (_results.Count != RUN_COUNT)
            {
                _ch.Cwl($"Results count {_results.Count} doesnt match expected run count {RUN_COUNT}", ConsoleColor.Red);
            }

            var noResponse = _results.Where(r => string.IsNullOrWhiteSpace(r.ResponseMessage)).ToArray();
            if (noResponse.Length > 0)
            {
                _ch.Cwl($"Results {string.Join(",", noResponse.Select(r => r.ClientId))} had empty responses", ConsoleColor.Red);
            }
            else
            {
                var correctResponses = _results.Where(r => r.ResponseMessage.Equals($"REPLYING FROM Serverside for {r.RequestMessage.Replace(TRIGGER_TEXT, string.Empty)}", StringComparison.OrdinalIgnoreCase)).ToArray();
                if (correctResponses.Length == RUN_COUNT)
                {
                    _ch.Cwl($"All results had correct responses", ConsoleColor.Green);
                }
                else
                {
                    _ch.Cwl($"All results did not have correct responses", ConsoleColor.Red);
                }

            }
            
            Console.ReadLine();
        }

        private static async Task<string> ExchangeData(string message, int clientId)
        {
            var log = new LogEntryList();
            var result = new StringBuilder();
            log.Add($"{clientId} - Connecting...");

            var tc = new TcpClient();
            tc.Connect("192.168.1.2", 20000);

            log.Add($"\t {clientId} - Getting Stream...");

            var stream = tc.GetStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };
            var reader = new StreamReader(stream);

            log.Add($"\t {clientId} - sending {message}", ConsoleColor.Cyan);
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();

            if (stream.CanTimeout)
            {
                stream.ReadTimeout = 500;
            }

            try
            {
                log.Add($"\t {clientId} - Getting result...");
                var val = await reader.ReadLineAsync();
                result.Append(val);
                log.Add($"\t {clientId} - Didnt fail connection");
            }
            catch (IOException e) //this was happening sometimes.
            {
                //if (e.Message.IndexOf("the connected party did not properly respond after a period of time", StringComparison.Ordinal) < 0)
                //{
                //    throw;
                //}

                log.Add("${clientId} - connection dropped", ConsoleColor.Red);
                return e.ToString();
            }
            finally
            {
                tc.Dispose();
            }
            log.Add(result.ToString(), ConsoleColor.Cyan);
            log.Add($"{clientId} - Completed");

            const int STARTING_LINE = 8;
            var currentLine = STARTING_LINE;
            
            foreach (var le in log.LogEntries)
            {
                Console.SetCursorPosition(0, currentLine);
                _ch.Cwl(le.Msg, le.Color);
                currentLine++;
            }

            return result.ToString();
        }
    }
}