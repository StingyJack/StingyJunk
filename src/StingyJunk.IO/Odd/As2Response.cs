namespace StingyJunk.IO.Odd
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;

    public class As2Response
    {
        public HttpWebResponse RawHttpWebResponse { get; set; }
        public HttpResponseMessage RawHttpResponseMessage { get; set; }
        public Encoding EncodingType { get; set; } = Encoding.Unicode;
        public string Content { get; set; }

        public HttpStatusCode StatusCode
        {
            get
            {
                if (RawHttpWebResponse != null)
                {
                    return RawHttpWebResponse.StatusCode;
                }
                if (RawHttpResponseMessage != null)
                {
                    return RawHttpResponseMessage.StatusCode;
                }
                throw new InvalidOperationException("No Raw Response available");
            }
        }

        public List<string> FlattenedHeaders
        {
            get
            {
                var returnValue = new List<string>();
                if (RawHttpWebResponse != null)
                {
                    foreach (var key in RawHttpWebResponse.Headers.AllKeys)
                    {
                        var values = RawHttpWebResponse.Headers.GetValues(key);
                        if (values == null)
                        {
                            returnValue.Add($"{key}:");
                        }
                        else
                        {
                            returnValue.Add($"{key}:{string.Join(",", values)}");
                        }
                    }
                }
                if (RawHttpResponseMessage != null)
                {
                    foreach (var header in RawHttpResponseMessage.Headers)
                    {
                        if (header.Value == null)
                        {
                            returnValue.Add($"{header.Key}:");
                        }
                        else
                        {
                            returnValue.Add($"{header.Key}:{string.Join(",", header.Value)}");
                        }
                    }
                }
                return returnValue;
            }
        }
    }
}