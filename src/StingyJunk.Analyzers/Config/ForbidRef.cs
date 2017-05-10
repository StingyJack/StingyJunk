namespace StingyJunk.Analyzers.Config
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ForbidRef
    {
        /// <summary>
        ///     The identification of the current project item to block
        /// </summary>
        [DataMember]
        public IdentMatchInfo SourceMatch { get; set; }

        /// <summary>
        ///     The identification of the target(s) to block
        /// </summary>
        [DataMember]
        public IdentMatchInfo DisallowedTarget { get; set; }

        [DataMember]
        public ReportAs ReportAs{ get; set; }

    }
}