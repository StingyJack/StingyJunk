namespace StingyJunk.Compilation.Bags
{
    /// <summary>
    ///     Assembly details
    /// </summary>
    public class AsmDetail
    {
        /// <summary>
        ///     Gets or sets the path for the portable executable (dll, exe).
        /// </summary>
        public string AsmPath { get; set; }

        /// <summary>
        ///     Gets or sets the PDB path.
        /// </summary>
        public string PdbPath { get; set; }

        /// <summary>
        ///     Gets or sets the asm name.
        /// </summary>
        public string AsmName { get; set; }
    }
}