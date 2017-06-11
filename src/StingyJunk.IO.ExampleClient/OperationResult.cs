namespace StingyJunk.IO.ExampleClient
{
    using System;

    internal class OperationResult
    {
        public int ClientId { get; set; }
        public long ElapsedMs { get; set; }
        public string RequestMessage { get; set; }
        public DataExchangeResult DataExchangeResult { get; set; }
        public Exception Ex { get; set; }
    }
}