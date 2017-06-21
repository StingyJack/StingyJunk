namespace StingyJunk.IO.Odd
{
    using System.Security.Cryptography;
    using System.Security.Cryptography.Pkcs;
    using System.Security.Cryptography.X509Certificates;

    public static class EncryptionUtils
    {
        public static byte[] Encode(byte[] arMessage, string signerCert, string signerPassword)
        {
            var cert = new X509Certificate2(signerCert, signerPassword);
            var contentInfo = new ContentInfo(arMessage);

            var signedCms = new SignedCms(contentInfo, true); // <- true detaches the signature
            var cmsSigner = new CmsSigner(cert);

            signedCms.ComputeSignature(cmsSigner);
            var signature = signedCms.Encode();

            return signature;
        }

        public static byte[] Encrypt(byte[] message, string recipientCert, EncryptionAlgorithm encryptionAlgorithm)
        {
            var cert = new X509Certificate2(recipientCert);
            var contentInfo = new ContentInfo(message);
            var envelopedCms = new EnvelopedCms(contentInfo, new AlgorithmIdentifier(new Oid(encryptionAlgorithm.ToString())));

            var recipient = new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, cert);
            envelopedCms.Encrypt(recipient);

            var encoded = envelopedCms.Encode();
            return encoded;
        }

        public static byte[] Decrypt(byte[] encodedEncryptedMessage, out string encryptionAlgorithmName)
        {
            var envelopedCms = new EnvelopedCms();

            // NB. the message will have been encrypted with your public key.
            // The corresponding private key must be installed in the Personal Certificates folder of the user
            // this process is running as.
            envelopedCms.Decode(encodedEncryptedMessage);

            envelopedCms.Decrypt();
            encryptionAlgorithmName = envelopedCms.ContentEncryptionAlgorithm.Oid.FriendlyName;

            return envelopedCms.Encode();
        }
    }
}