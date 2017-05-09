namespace StingyJunk.Compilation.Bags
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    /// <summary>
    ///     Common compilation details
    /// </summary>
    public class CommonCompilationDetails
    {
        /// <summary>
        ///     Gets or sets the list of usings.
        /// </summary>
        public List<string> ListOfUsings { get; set; }

        /// <summary>
        ///     Gets or sets the metadata references.
        /// </summary>
        public MetadataReference[] MetadataReferences { get; set; }
    }
}