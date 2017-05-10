namespace StingyJunk.Compilation.Bags
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///     A single code file that has had some content removed and saved to disk
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RewrittenFile
    {
        public RewrittenFile(string rewrittenFilePath, string originalFilePath)
        {
            RewrittenFilePath = rewrittenFilePath;
            OriginalFilePath = originalFilePath;
        }

        public string OriginalFilePath { get; }
        public string RewrittenFilePath { get; }
    }
}