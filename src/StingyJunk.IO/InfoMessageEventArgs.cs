namespace StingyJunk.IO
{
    using System;

    public class InfoMessageEventArgs
    {
        public string SourceName { get; set; }
        public DateTime Timestamp { get; set; }
        public string InfoMessage { get; set; }
    }
}