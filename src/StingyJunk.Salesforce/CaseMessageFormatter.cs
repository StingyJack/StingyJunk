namespace StingyJunk.SalesForce
{
    using System.Collections.Generic;
    
    using Salesforce;
    using Salesforce.Models;
    

    public class CaseMessageFormatter 
    {
        public Message GetCaseMessage(Case sfCase)
        {
            var commentAttachment = new Attachment();
            foreach (var caseComment in sfCase.CaseComments.Records)
            {
                commentAttachment.Fields.Add(new AttachmentField
                {
                    Title = caseComment.CreatedById,
                    Value = caseComment.CommentBody
                });
            }

            var message = new Message
            {
                Text = "Here is the requested information for the salesforce case",
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = "Fallback Text",
                        ColorHex = "#36a64f",
                        PreText = $"Details for case {sfCase.CaseNumber}",
                        Fields = new List<AttachmentField>
                        {
                            new AttachmentField
                            {
                                Title = "Customer",
                                Value = sfCase.Account.Name,
                            },
                            new AttachmentField
                            {
                                Title = "Case Open Date",
                                Value = sfCase.CreatedDateShort,
                            },
                            new AttachmentField
                            {
                                Title = "Subject",
                                Value = sfCase.Subject,
                            },
                            new AttachmentField
                            {
                                Title = "Owner",
                                Value = sfCase.OwnerId,
                            },
                            new AttachmentField
                            {
                                Title = $"Last Update: {sfCase.LastModifiedDateShort}",
                                Value = "Need to find this field",
                            }
                        }
                    },
                    commentAttachment
                }
            };

            return message;
        }
    }
}