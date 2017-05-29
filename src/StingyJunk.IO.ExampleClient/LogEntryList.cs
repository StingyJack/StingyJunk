namespace StingyJunk.IO.ExampleClient
{
    using System;
    using System.Collections.Generic;

    internal static partial class ExampleClient
    {
        public class LogEntryList
        {
            public List<LogEntry> LogEntries { get; set; } = new List<LogEntry>();

            public void Add(string msg, ConsoleColor? color = null)
            {
                LogEntries.Add(new LogEntry(msg, color));
            }
        }
    }
}