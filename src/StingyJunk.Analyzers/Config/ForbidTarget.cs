using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StingyJunk.Analyzers.Config
{
    public class ForbidTarget
    {
        public string NamespaceMatch { get; set; }
        public string TypeNameMatch { get; set; }
        public string MemberMatch { get; set; }
    }
}
