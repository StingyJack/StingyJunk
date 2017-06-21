namespace StingyJunk.IO.ExampleClient
{
    using System;

    internal class Program
    {
        public static void Main()
        {
            var choice = true;
            while (choice == true)
            {
                Console.WriteLine("Choose a client...");
                Console.WriteLine("\t 1 - ExampleAsyncTcpClient");
                Console.WriteLine("\t 2 - ExampleAs2Client");
                Console.WriteLine("");
                Console.WriteLine("\t 0 - Exit");
                var selection = Console.ReadKey();
                Console.Clear();
                if (selection.Key == ConsoleKey.D0 || selection.Key == ConsoleKey.NumPad0)
                {
                    choice = false;
                }
                else if (selection.Key == ConsoleKey.D1 || selection.Key == ConsoleKey.NumPad1)
                {
                    ExampleAsyncTcpClient.Run();
                }
                else if (selection.Key == ConsoleKey.D2 || selection.Key == ConsoleKey.NumPad2)
                {
                    ExampleAs2Client.Run();
                }
            }
        }
    }
}