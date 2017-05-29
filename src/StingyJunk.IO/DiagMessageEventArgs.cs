namespace StingyJunk.IO
{
    using System;

    public class DiagMessageEventArgs : EventArgs
    {
        public string SourceName { get; set; }
        public DateTime Timestamp { get; set; }
        public string DiagnosticMessage { get; set; }
    }
}