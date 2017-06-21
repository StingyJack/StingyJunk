namespace StingyJunk.IO.Odd
{
    using System.Collections.Generic;
    using System.IO;

    internal class CryptoAlterations
    {
        public string ContentType { get; set; }
        public Stream Data { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}