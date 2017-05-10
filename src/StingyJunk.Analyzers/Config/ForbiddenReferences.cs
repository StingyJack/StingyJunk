namespace StingyJunk.Analyzers.Config
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ForbiddenReferences
    {
        public ForbiddenReferences(List<ForbiddenReference> references = null)
        {
            References = references;
        }

        [DataMember]
        // ReSharper disable CollectionNeverUpdated.Global
        public List<ForbiddenReference> References { get;  }
        // ReSharper restore CollectionNeverUpdated.Global
    }
}
