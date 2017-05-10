namespace StingyJunk.Analyzers.Config
{
    using System.Runtime.Serialization;

    /// <summary>
    ///     The identifying matching information for either source or target match.
    /// </summary>
    [DataContract]
    public class IdentMatchInfo
    {
        /// <summary>
        ///     Gets or sets the name match.
        /// </summary>
        /// <example>
        ///     "Namespace.SubNamespace.Type.Member" - disallows that member
        ///     "Namespace.SubNamespace.Type" - disallows that type
        ///     "Namespace.SubNamespace" - disallows that subnamespace
        ///     "Namespace" - disallows that entire namespace
        /// </example>
        [DataMember]
        public string NameMatch { get; set; }

     
    }
}