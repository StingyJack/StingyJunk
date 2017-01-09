namespace StingyJunk.Analyzers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    
    //TODO: Try codefix
    //TODO: Test with multiple projects  (stingybot)
    //TODO: Is compilation action necessary?
    //TODO: Package, distribute, reference

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ForbiddenReferenceAnalyzer : DiagnosticAnalyzer
    {
        #region "consts and fields"

        public const string DIAGNOSTIC_ID = "ForbiddenReferenceAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager,
            typeof(Resources));

        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat),
            Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription),
            Resources.ResourceManager, typeof(Resources));

        private const string Category = "References";

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DIAGNOSTIC_ID, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, true, Description);

        #endregion //#region "consts and fields"

        #region "props and enums"

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(_Rule); }
        }

        private enum ContextType
        {
            Unknown,
            Symbol,
            Compilation
        }

        #endregion //#region "props and enums"

        #region "analysis members"

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(CheckForbiddenReferencesAsSymbol, SymbolKind.Namespace);
            context.RegisterCompilationAction(CheckForbiddenReferencesAsCompilation);
        }

        private static void CheckForbiddenReferencesAsCompilation(CompilationAnalysisContext compilationContext)
        {
            CheckForbiddenReferences(null, compilationContext);
        }

        private static void CheckForbiddenReferencesAsSymbol(SymbolAnalysisContext symbolContext)
        {
            CheckForbiddenReferences(symbolContext, null);
        }

        private static void CheckForbiddenReferences(SymbolAnalysisContext? symbolContext, CompilationAnalysisContext? compilationContext)
        {
            var contextType = GetContextType(symbolContext, compilationContext);
            if (contextType == ContextType.Unknown) { return; }

            Location location = null;
            IEnumerable<AssemblyIdentity> referencedAssemblies = null;
            AnalyzerOptions options = null;
            CancellationToken cancellationToken;

            switch (contextType)
            {
                case ContextType.Symbol:
                    location = symbolContext.Value.Symbol.Locations.First();
                    referencedAssemblies = symbolContext.Value.Compilation.ReferencedAssemblyNames;
                    options = symbolContext.Value.Options;
                    cancellationToken = symbolContext.Value.CancellationToken;
                    break;

                case ContextType.Compilation:
                    referencedAssemblies = compilationContext.Value.Compilation.ReferencedAssemblyNames;
                    location = compilationContext.Value.Compilation.SyntaxTrees.First().GetRoot().GetLocation();
                    options = compilationContext.Value.Options;
                    cancellationToken = compilationContext.Value.CancellationToken;
                    break;

                default:
                    return;
            }

            var forbiddenAssemblyRegexes = GetForbiddenReferenceRegexValues(options, cancellationToken);

            if (forbiddenAssemblyRegexes == null
                || forbiddenAssemblyRegexes.Any() == false)
            {
                return;
            }

            foreach (var refAssem in referencedAssemblies)
            {
                foreach (var forbiddenAssemblyRegex in forbiddenAssemblyRegexes)
                {
                    if (Regex.IsMatch(refAssem.Name, forbiddenAssemblyRegex))
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
        }

        private static ContextType GetContextType(SymbolAnalysisContext? symbolContext, CompilationAnalysisContext? compilationContext)
        {
            if (symbolContext.HasValue)
            {
                return ContextType.Symbol;
            }

            if (compilationContext.HasValue)
            {
                return ContextType.Compilation;
            }
            return ContextType.Unknown;
        }

        #endregion //#region "analysis members"

        #region "config"

        private static IEnumerable<string> GetForbiddenReferenceRegexValues(AnalyzerOptions options, CancellationToken cancellationToken)
        {
            if (options == null || options.AdditionalFiles.Length == 0)
            {
                return Enumerable.Empty<string>();
            }

            var demFiles = options.AdditionalFiles;

            var configFile = demFiles.FirstOrDefault(
                file => Path.GetFileName(file.Path)
                    .Equals("ForbiddenReferences.xml", StringComparison.OrdinalIgnoreCase));

            if (configFile == null)
            {
                return Enumerable.Empty<string>();
            }

            var returnValue = new List<string>();
            try
            {
                var sourceText = configFile.GetText(cancellationToken);
                var stream = new MemoryStream();
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    sourceText.Write(writer, cancellationToken);
                }

                stream.Position = 0;

                var doc = XDocument.Load(stream);
                var elements = doc.Root.Descendants("ForbiddenReference").Select(x => x.Value);
                returnValue.AddRange(elements);
            }
            catch (Exception)
            {
                // ignored, maybe toss a null back for that?
            }

            return returnValue;
        }

        #endregion //#region "config"
    }
}