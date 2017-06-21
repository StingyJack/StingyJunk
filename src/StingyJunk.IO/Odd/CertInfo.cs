namespace StingyJunk.IO.Odd
{
    public class CertInfo
    {
        /// <summary>
        ///     The path to the senders pfx file 
        /// </summary>
        public string SendingCertifcatePath { get; set; }

        /// <summary>
        ///     The path to the receivers pfx file
        /// </summary>
        public string ReceiversCertificatePath { get; set; }

        /// <summary>
        ///     The password to use it
        /// </summary>
        public string SendingCertPassword { get; set; }

        /// <summary>
        ///     True if the content should be signed
        /// </summary>
        public bool Sign { get; set; }

        /// <summary>
        ///     True if the content should be encrypted
        /// </summary>
        public bool Encrypt { get; set; }

        /// <summary>
        ///     The crypto type
        /// </summary>
        public EncryptionAlgorithm EncryptionType { get; set; } = EncryptionAlgorithm.DES3;
    }
}