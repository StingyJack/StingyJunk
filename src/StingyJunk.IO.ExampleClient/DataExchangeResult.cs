namespace StingyJunk.IO.ExampleClient
{
    using System.Collections.Generic;

    internal class DataExchangeResult
    {
        public List<string> Log { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
        public string ResponseMessage { get; set; }
    }
}