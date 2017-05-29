namespace StingyJunk.IO
{
    using System;

    public class MessageRequestResponseEventArgs : EventArgs
    {
        public string RequestMessage { get; set; }
        public string ResponseMessage { get; set; }
    }
}