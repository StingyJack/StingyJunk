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
    
    //TODO: Test with multiple projects  (stingybot)
    //TODO: Package, distribute, reference

    /// <summary>
    ///     Causes errors when a refernce is included that should not be added
    ///  </summary>
    /// <remarks>
    ///     There is no codefix associated with this. 
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public class ForbiddenReferenceAnalyzer : DiagnosticAnalyzer
    {
        #region "consts and fields"

        public const string DIAGNOSTIC_ID = "SJ001";

        private static readonly LocalizableString _Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager,
            typeof(Resources));

        private static readonly LocalizableString _MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat),
            Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString _Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription),
            Resources.ResourceManager, typeof(Resources));

        private const string CATEGORY = "References";

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DIAGNOSTIC_ID, _Title, _MessageFormat, CATEGORY,
            DiagnosticSeverity.Error, true, _Description);

        #endregion //#region "consts and fields"

        #region "props and enums"

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(_Rule); }
        }

        #endregion //#region "props and enums"

        #region "analysis members"

        public override void Initialize(AnalysisContext context)
        {
            
            context.RegisterCompilationAction(
                compilationContext =>
                {
                    var referencedAssemblies = compilationContext.Compilation.ReferencedAssemblyNames;
                    var options = compilationContext.Options;
                    var cancellationToken = compilationContext.CancellationToken;
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
                                var descr = string.Format(_MessageFormat.ToString(), refAssem.Name);
                                compilationContext.ReportDiagnostic(Diagnostic.Create(DIAGNOSTIC_ID, CATEGORY, descr, DiagnosticSeverity.Error, 
                                    DiagnosticSeverity.Error, true,0,_Title,_Description));

                            }
                        }
                    }
                });
        }

        #endregion //#region "analysis members"

        #region "config"

        private static IList<string> GetForbiddenReferenceRegexValues(AnalyzerOptions options, CancellationToken cancellationToken)
        {
            var returnValue = new List<string>();

            if (options == null || options.AdditionalFiles.Length == 0)
            {
                return returnValue;
            }

            var demFiles = options.AdditionalFiles;

            var configFile = demFiles.FirstOrDefault(
                file => Path.GetFileName(file.Path)
                    .Equals("ForbiddenReferences.xml", StringComparison.OrdinalIgnoreCase));

            if (configFile == null)
            {
                return returnValue;
            }
            
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
                if (doc.Root == null) { return returnValue; }
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