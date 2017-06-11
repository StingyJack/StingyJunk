namespace StingyJunk.IO
{
    using System;

    public class ErrorMessageEventArgs : MessageEventArgs
    {
        public Exception Exception { get; set; }
    }
}