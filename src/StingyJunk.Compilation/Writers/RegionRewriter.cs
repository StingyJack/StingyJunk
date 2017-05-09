namespace StingyJunk.Compilation.Writers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class RegionRewriter : CSharpSyntaxRewriter
    {
        public RegionRewriter() : base(true)
        {
        }

        public override SyntaxNode VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            return SyntaxFactory.SkippedTokensTrivia();
        }

        public override SyntaxNode VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            return SyntaxFactory.SkippedTokensTrivia();
        }
    }
}