namespace StingyJunk.IO.ExampleClient
{
    using System;

    public class LogEntry
    {
        public string Msg { get; set; }
        public ConsoleColor? Color { get; set; }

        public LogEntry(string msg, ConsoleColor? color = null)
        {
            Msg = msg;
            Color = color;
        }
    }
}