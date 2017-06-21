namespace StingyJunk.IO.Odd
{
    using System.IO;

    public interface IAs2Client
    {
        As2Response Send(Stream data, string fromAddress, string toAddress, string fileName, CertInfo certificateInfo = null, string specifiedContentType = null);

        As2Response Send(As2Request request);

        int TimeoutMs { get; set; }
    }
}