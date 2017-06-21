namespace StingyJunk.IO.Odd
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    
    public class As2Client : IAs2Client
    {
        private readonly string _serverUrl;
        private readonly HttpClient _httpClient;
      
        public int TimeoutMs { get; set; } = 3000;

        public As2Client(string serverUrl)
        {
            _serverUrl = serverUrl;
            _httpClient = new HttpClient();
        }

        public As2Client(HttpClient existingHttpClient, string serverUrl)
        {
            _httpClient = existingHttpClient;
            _serverUrl = serverUrl;
        }

        public As2Response Send(As2Request request)
        {
            return Send(request.Data, request.FromAddress, request.ToAddress, request.FileName, 
                request.CertificateInfo, request.ContentType);
        }

        public As2Response Send(Stream data, string fromAddress, string toAddress, string fileName, 
            CertInfo certificateInfo = null, string specifiedContentType = null)
        {
            var headers = new Dictionary<string, string>
            {
                {"Mime-Version", "1.0"},
                {"AS2-Version", "1.2"},
                {"AS2-From", fromAddress},
                {"AS2-To", toAddress},
                {"Message-Id", "<AS2_" + DateTime.Now.ToString("hhmmssddd") + ">"},
                {"Subject", fileName}
            };

            var contentType = DeriveContentType(specifiedContentType, fileName);

            if (certificateInfo == null)
            {
                headers.Add("Content-Transfer-Encoding", "binary");
                headers.Add("Content-Disposition", "inline; filename=\"" + fileName + "\"");
            }
            else
            {
                var cryptoAlterations = PerformEncryptionOrSigning(data, certificateInfo, contentType);
                foreach (var header in cryptoAlterations.Headers)
                {
                    headers.Add(header.Key, header.Value);
                }
                data = cryptoAlterations.Data;
                contentType = cryptoAlterations.ContentType;
            }

            var content = new StreamContent(data);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            foreach (var header in headers)
            {
                content.Headers.Add(header.Key, header.Value);
            }

            var as2Response = new As2Response();
            var task = _httpClient.PostAsync(_serverUrl, content)
                .ContinueWith(taskWithResponse =>
                {
                    var respMsg = taskWithResponse.Result;
                    as2Response.Content = respMsg.Content.ReadAsStringAsync().Result;
                    var encType = respMsg.Content.Headers.ContentEncoding?.FirstOrDefault();
                    as2Response.EncodingType = GetResponseEncoding(encType);
                    as2Response.RawHttpResponseMessage = respMsg;
                });
            task.Wait();
            return as2Response;
        }


        private string DeriveContentType(string specifiedContentType, string fileName)
        {
            if (string.IsNullOrWhiteSpace(specifiedContentType) == false)
            {
                return specifiedContentType;
            }
            return Path.GetExtension(fileName) == ".xml" ? "application/xml" : "application/EDIFACT";
        }

        private CryptoAlterations PerformEncryptionOrSigning(Stream data, CertInfo certificateInfo, string startingContentType)
        {
            var ms = new MemoryStream();
            data.CopyTo(ms);
            var content = ms.ToArray();
            var returnValue = new CryptoAlterations();
            var alteredContentType = startingContentType;

            if (certificateInfo.Sign)
            {
                content = MimeUtils.CreateMessage(alteredContentType, "binary", "", content);
                var signResult = MimeUtils.Sign(content, certificateInfo.SendingCertifcatePath, certificateInfo.SendingCertPassword);
                content = signResult.Item1;
                alteredContentType = signResult.Item2;
                returnValue.Headers.Add("EDIINT-Features", "multiple-attachments");
            }

            if (certificateInfo.Encrypt)
            {
                if (string.IsNullOrEmpty(certificateInfo.ReceiversCertificatePath))
                {
                    throw new ArgumentNullException(certificateInfo.ReceiversCertificatePath, "if encrytionAlgorithm is specified then recipientCertFilename must be specified");
                }

                var signedContentTypeHeader = Encoding.ASCII.GetBytes($"Content-Type: {alteredContentType}{Environment.NewLine}");
                var contentWithContentTypeHeaderAdded = MimeUtils.ConcatBytes(signedContentTypeHeader, content);

                content = EncryptionUtils.Encrypt(contentWithContentTypeHeaderAdded, certificateInfo.ReceiversCertificatePath, EncryptionAlgorithm.DES3);
                alteredContentType = "application/pkcs7-mime; smime-type=enveloped-data; name=\"smime.p7m\"";
            }

            returnValue.ContentType = alteredContentType;
            returnValue.Data = new MemoryStream(content);
            return returnValue;
        }


        private Encoding GetResponseEncoding(string contentEncoding)
        {
            var encoding = Encoding.ASCII;
            if (string.IsNullOrWhiteSpace(contentEncoding) == false)
            {
                try
                {
                    encoding = Encoding.GetEncoding(contentEncoding);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Couldn't get encoding {contentEncoding}: {ex}");
                }
            }
            return encoding;
        }
    }
}