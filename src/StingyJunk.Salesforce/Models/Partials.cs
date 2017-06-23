namespace StingyJunk.Salesforce.Models
{
    using System;
    using global::Salesforce.Common.Models;

    public partial class Case
    {
        public Contact Contact { get; set; }
        public Account Account { get; set; }
        public QueryResult<CaseComment> CaseComments { get; set; }

        public string CreatedDateShort => Convert.ToDateTime(CreatedDate).ToString("d");
        public string LastModifiedDateShort => Convert.ToDateTime(LastModifiedDate).ToString("d");
    }

    public class CaseComment
    {
        //in the docs but not the objects
        //public string ConnectionReceivedId { get; set; }
        //public string ConnectionSentId { get; set; }
        //public string CreatorFullPhotoUrl { get; set; }
        //public string CreatorName { get; set; }
        //public string CreatorSmallPhotoUrl { get; set; }
        public string IsDeleted { get; set; }

        public string IsPublished { get; set; }
        public string ParentId { get; set; }
        public string CreatedById { get; set; }
        public string CommentBody { get; set; }
    }
}