namespace StingyJunk.IO
{
    using System;

    public class MessageEventArgs : EventArgs
    {
        public string SourceName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
}