namespace StingyJunk.Analyzers
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    //TODO: Add config file (AdditionalFiles) or some other config means
    //TODO: Use that config file
    //TODO: Try codefix
    //TODO: Test with multiple projects  (stingybot)
    //TODO: Is compilation action necessary?
    //TODO: Package, distribute, reference

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StingyJunkAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "StingyJunkAnalyzers";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager,
            typeof(Resources));

        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat),
            Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription),
            Resources.ResourceManager, typeof(Resources));

        private const string Category = "References";

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(_Rule); }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(CheckForbiddenReferencesAsSymbol, SymbolKind.Namespace);
            context.RegisterCompilationAction(CheckForbiddenReferencesAsCompilation);
        }

        private enum ContextType
        {
            Unknown,
            Symbol,
            Compilation
        }

        private static void CheckForbiddenReferences(SymbolAnalysisContext? symbolContext, CompilationAnalysisContext? compilationContext)
        {
            var contextType = ContextType.Unknown;
            Location location = null;
            IEnumerable<AssemblyIdentity> referencedAssemblies = null;
            if (symbolContext.HasValue)
            {
                location = symbolContext.Value.Symbol.Locations.First();
                //location = Location.Create(); symbolContext.Value.Options.
                referencedAssemblies = symbolContext.Value.Compilation.ReferencedAssemblyNames;
                contextType = ContextType.Symbol;
            }
            else if (compilationContext.HasValue)
            {
                referencedAssemblies = compilationContext.Value.Compilation.ReferencedAssemblyNames;
                location = compilationContext.Value.Compilation.SyntaxTrees.First().GetRoot().GetLocation();
                contextType = ContextType.Compilation;
            }

            if (contextType == ContextType.Unknown
                || referencedAssemblies == null)
            {
                return;
            }

            foreach (var refAssem in referencedAssemblies)
            {
                if (refAssem.Name.EndsWith(".Tfs"))
                {
                    var diagnostic = Diagnostic.Create(_Rule, location, refAssem.Name);

                    switch (contextType)
                    {
                        case ContextType.Compilation:
                            // ReSharper disable PossibleInvalidOperationException
                            compilationContext.Value.ReportDiagnostic(diagnostic);
                            // ReSharper restore PossibleInvalidOperationException
                            break;
                        case ContextType.Symbol:
                            // ReSharper disable PossibleInvalidOperationException
                            symbolContext.Value.ReportDiagnostic(diagnostic);
                            // ReSharper restore PossibleInvalidOperationException
                            break;
                    }
                }
            }
        }

        

        private static void CheckForbiddenReferencesAsCompilation(CompilationAnalysisContext compilationContext)
        {
            CheckForbiddenReferences(null, compilationContext);
        }

        private static void CheckForbiddenReferencesAsSymbol(SymbolAnalysisContext symbolContext)
        {
            CheckForbiddenReferences(symbolContext, null);
        }

        /*
        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = (INamedTypeSymbol) context.Symbol;

            // Find just those named type symbols with names containing lowercase letters.
            if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
        */
    }
}