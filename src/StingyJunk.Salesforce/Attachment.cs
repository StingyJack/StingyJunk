namespace StingyJunk.Salesforce
{
    using System.Collections.Generic;

    public class Attachment
    {
        public List<AttachmentField> Fields { get; set; }
        public string Fallback { get; set; }
        public string ColorHex { get; set; }
        public string PreText { get; set; }
    }
}