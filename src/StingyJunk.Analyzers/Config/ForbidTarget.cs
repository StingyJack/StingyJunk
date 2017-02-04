using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StingyJunk.Analyzers.Config
{
    using System.Runtime.Serialization;

    public class ForbidTarget
    {
        
        public string NameMatch { get; set; }
        
        public string VersionGreaterThan { get; set; }

        public string NamespaceMatch { get; set; }
        
        public string TypeNameMatch { get; set; }
        
        public string MemberMatch { get; set; }

       
    }
}
