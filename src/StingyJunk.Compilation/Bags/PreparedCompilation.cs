namespace StingyJunk.Compilation.Bags
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class PreparedCompilation
    {
        public CSharpCompilationOptions Options { get; set; }
        public CSharpCompilation Compilation { get; set; }
        public List<string> Usings { get; set; } = new List<string>();
        public IEnumerable<MetadataReference> MetadataReferences { get; set; } = new List<MetadataReference>();
    }
}