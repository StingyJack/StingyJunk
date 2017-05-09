namespace StingyJunk.Compilation.Bags
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Emit;

    /// <summary>
    ///     A compilation of a single script or a script and its referenced scripts, or class file that has 
    /// had some non script compatible content removed and then compiled as script
    /// </summary>
    public class ScriptCompilationResult
    {
        /// <summary>
        ///     Gets or sets the original file path.
        /// </summary>
        public string OriginalFilePath { get; set; }

        /// <summary>
        ///     Gets or sets the assembly file path.
        /// </summary>
        public string AssemblyFilePath { get; set; }

        /// <summary>
        ///     Gets or sets the assembly bytes.
        /// </summary>
        public byte[] AssemblyBytes { get; set; }

        /// <summary>
        ///     Gets or sets the PDB file path.
        /// </summary>
        public string PdbFilePath { get; set; }

        /// <summary>
        ///     Gets or sets the PDB bytes.
        /// </summary>
        public byte[] PdbBytes { get; set; }

        /// <summary>
        ///     Any namepaces found during rewrite.
        /// </summary>
        public List<string> FoundNamespaces { get; } = new List<string>();

        /// <summary>
        ///     Any assemblies found during rewrite.
        /// </summary>
        public List<MetadataReference> FoundMetadataReferences { get; } = new List<MetadataReference>();

        /// <summary>
        ///     Gets the is compiled.
        /// </summary>
        public bool IsCompiled { get; set; }

        /// <summary>
        ///     Gets or sets the compilation result.
        /// </summary>
        public EmitResult CompilationResult { get; set; }
    }
}