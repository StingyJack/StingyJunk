namespace StingyJunk.IO.Odd
{
    using System;
    using System.Text;

    /// <summary>
    /// Contains a number of useful static functions for creating MIME messages.
    /// </summary>
    public static class MimeUtils
    {
        public const string MESSAGE_SEPARATOR = "\r\n\r\n";

        /// <summary>
        /// return a unique MIME style boundary
        /// this needs to be unique enought not to occur within the data
        /// and so is a Guid without - or { } characters.
        /// </summary>
        /// <returns></returns>
        private static string MimeBoundaryAsString()
        {
            return "_" + Guid.NewGuid().ToString("N") + "_";
        }

        /// <summary>
        /// Creates the a Mime header out of the components listed.
        /// </summary>
        /// <param name="sContentType">Content type</param>
        /// <param name="sEncoding">Encoding method</param>
        /// <param name="sDisposition">Disposition options</param>
        /// <returns>A string containing the three headers.</returns>
        public static string BuildHeader(string sContentType, string sEncoding, string sDisposition)
        {
            var sOut = "Content-Type: " + sContentType + Environment.NewLine;
            if (sEncoding != "")
            {
                sOut += "Content-Transfer-Encoding: " + sEncoding + Environment.NewLine;
            }

            if (sDisposition != "")
            {
                sOut += "Content-Disposition: " + sDisposition + Environment.NewLine;
            }

            sOut = sOut + Environment.NewLine;

            return sOut;
        }

        /// <summary>
        /// Return a single array of bytes out of all the supplied byte arrays.
        /// </summary>
        /// <param name="arBytes">Byte arrays to add</param>
        /// <returns>The single byte array.</returns>
        public static byte[] ConcatBytes(params byte[][] arBytes)
        {
            long lLength = 0;
            long lPosition = 0;

            //Get total size required.
            foreach (var ar in arBytes)
            {
                lLength += ar.Length;
            }

            //Create new byte array
            var toReturn = new byte[lLength];

            //Fill the new byte array
            foreach (var ar in arBytes)
            {
                ar.CopyTo(toReturn, lPosition);
                lPosition += ar.Length;
            }

            return toReturn;
        }

        /// <summary>
        /// Create a Message out of byte arrays (this makes more sense than the above method)
        /// </summary>
        /// <param name="contentType">Content type ie multipart/report</param>
        /// <param name="sEncoding">The encoding provided...</param>
        /// <param name="sDisposition">The disposition of the message...</param>
        /// <param name="abMessageParts">The byte arrays that make up the components</param>
        /// <returns>The message as a byte array.</returns>
        public static byte[] CreateMessage(string contentType, string sEncoding, string sDisposition, params byte[][] abMessageParts)
        {
            int iHeaderLength;
            return CreateMessage(contentType, sEncoding, sDisposition, out iHeaderLength, abMessageParts);
        }

        /// <summary>
        /// Create a Message out of byte arrays (this makes more sense than the above method)
        /// </summary>
        /// <param name="contentType">Content type ie multipart/report</param>
        /// <param name="sEncoding">The encoding provided...</param>
        /// <param name="sDisposition">The disposition of the message...</param>
        /// <param name="iHeaderLength">The length of the headers.</param>
        /// <param name="abMessageParts">The message parts.</param>
        /// <returns>The message as a byte array.</returns>
        public static byte[] CreateMessage(string contentType, string sEncoding, string sDisposition, out int iHeaderLength, params byte[][] abMessageParts)
        {
            long lLength = 0;
            long lPosition = 0;

            //Only one part... Add headers only...
            if (abMessageParts.Length == 1)
            {
                var bHeader = Encoding.ASCII.GetBytes(BuildHeader(contentType, sEncoding, sDisposition));
                iHeaderLength = bHeader.Length;
                return ConcatBytes(bHeader, abMessageParts[0]);
            }

            // get boundary and "static" subparts.
            var stringBoundary = MimeBoundaryAsString();
            var bPackageHeader = Encoding.ASCII.GetBytes(BuildHeader(contentType + "; boundary=\"" + stringBoundary + "\"", sEncoding, sDisposition));
            var bBoundary = Encoding.ASCII.GetBytes(Environment.NewLine + "--" + stringBoundary + Environment.NewLine);
            var bFinalFooter = Encoding.ASCII.GetBytes(Environment.NewLine + "--" + stringBoundary + "--" + Environment.NewLine);

            //Calculate the total size required.
            iHeaderLength = bPackageHeader.Length;

            foreach (var ar in abMessageParts)
            {
                lLength += ar.Length;
            }
            lLength += iHeaderLength + bBoundary.Length * abMessageParts.Length +
                       bFinalFooter.Length;

            //Create new byte array to that size.
            var toReturn = new byte[lLength];

            //Copy the headers in.
            bPackageHeader.CopyTo(toReturn, lPosition);
            lPosition += bPackageHeader.Length;

            //Fill the new byte array in by coping the message parts.
            foreach (var ar in abMessageParts)
            {
                bBoundary.CopyTo(toReturn, lPosition);
                lPosition += bBoundary.Length;

                ar.CopyTo(toReturn, lPosition);
                lPosition += ar.Length;
            }

            //Finally add the footer boundary.
            bFinalFooter.CopyTo(toReturn, lPosition);

            return toReturn;
        }

        /// <summary>
        /// Signs a message and returns a MIME encoded array of bytes containing the signature, as well as the correct content type.
        /// </summary>
        /// <param name="messageData"></param>
        /// <param name="signerCert"></param>
        /// <param name="signerPassword"></param>
        /// <returns></returns>
        public static Tuple<byte[], string> Sign(byte[] messageData, string signerCert, string signerPassword)
        {
            var contentType = "multipart/signed; protocol=\"application/pkcs7-signature\"; micalg=\"sha1\"; boundary=\"" + MimeBoundaryAsString() + "\"";
            var stringBoundary = MimeBoundaryAsString();
            var boundary = Encoding.ASCII.GetBytes($"{Environment.NewLine}--{stringBoundary}{Environment.NewLine}");

            var signatureHeader = Encoding.ASCII.GetBytes(BuildHeader("application/pkcs7-signature; name=\"smime.p7s\"", "base64", "attachment; filename=smime.p7s"));
            var encodedSignatureHeader = EncryptionUtils.Encode(messageData, signerCert, signerPassword);

            var sig = Convert.ToBase64String(encodedSignatureHeader) + MESSAGE_SEPARATOR;
            var encodedSignature = Encoding.ASCII.GetBytes(sig);

            var footer = Encoding.ASCII.GetBytes($"--{stringBoundary}--{Environment.NewLine}");

            var finalMessage = ConcatBytes(boundary, messageData, boundary,signatureHeader, encodedSignature, footer);

            return new Tuple<byte[], string>(finalMessage, contentType);
        }
    }
}