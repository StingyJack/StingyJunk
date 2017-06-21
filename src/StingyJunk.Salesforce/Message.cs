namespace StingyJunk.Salesforce
{
    using System.Collections.Generic;

    public class Message
    {
        public string Text { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}