namespace StingyJunk.IO.ExampleClient
{
    using System;
    using System.IO;
    using Odd;

    internal class ExampleAs2Client
    {
        public static void Run()
        {
            //this uses the mendelson test server
            // view message postings at http://testas2.mendelson-e-c.com:8080/webas2/
            //  login to the server as user:guest password:guest

            // this is the test server endpoint
            var serverUrl = "http://testas2.mendelson-e-c.com:8080/as2/HttpReceiver";
            
            var as2Client = new As2Client(serverUrl);
            var fileName = "sample.edi";
            var data = File.OpenRead(fileName);


            var resp = as2Client.Send(data, "as2Client", "as2Server", fileName);
            Console.WriteLine($"Overall Response: {(int) resp.StatusCode} - {resp.StatusCode}");
            Console.WriteLine();
            Console.WriteLine($"***********Response Headers**********");
            foreach (var header in resp.FlattenedHeaders)
            {
                Console.WriteLine(header);
            }
            Console.WriteLine();
            Console.WriteLine($"***********Response Content**********");
            Console.WriteLine(resp.Content);

            Console.WriteLine("Press ENTER to close");
            Console.ReadLine();
        }
    }
}