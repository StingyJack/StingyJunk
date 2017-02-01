namespace StingyJunk.Analyzers.Config
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ForbiddenReferences
    {
        [DataMember]
        public List<AssemblyRef> References { get; set; }
    }
}
