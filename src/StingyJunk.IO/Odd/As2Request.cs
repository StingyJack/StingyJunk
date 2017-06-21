namespace StingyJunk.IO.Odd
{
    using System.IO;

    /// <summary>
    ///     An Applicability Standard rev 2 request
    /// </summary>
    public class As2Request
    {
        /// <summary>
        ///     The intended recipient. This does not have to be an email
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        ///     The sender address. This does not have to be an email
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        ///     The name of the file to send
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     The content type. Leave null to attempt automatic derivation
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     The message identifier. Leave null for an auto generated one.
        /// </summary>
        /// <remarks>
        ///     Usually of the format "&lt;AS2_SomethingUnique&gt;"
        /// </remarks>
        public string MessageId { get; set; }

        /// <summary>
        ///     The certificate info.
        /// </summary>
        public CertInfo CertificateInfo { get; set; }

        /// <summary>
        ///     The data payload stream
        /// </summary>
        public Stream Data { get; set; }
    }
}