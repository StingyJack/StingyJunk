namespace StingyJunk.IO
{
    using System;

    public class ConnectionStatsMessageEventArgs : EventArgs
    {
        public string SourceName { get; set; }
        public DateTime Timestamp { get; set; }
        public ConnectionStats Stats { get; set; }
    }
}