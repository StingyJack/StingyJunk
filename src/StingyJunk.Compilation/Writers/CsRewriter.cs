namespace StingyJunk.Compilation.Writers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Bags;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Misc;

    /// <summary>
    ///     Provides the means for creating alternate forms of a c# class file for 
    /// use in scripting
    /// </summary>
    public static class CsRewriter
    {
        public const string DEFAULT_REWRITE_TEMP_EXTENSION = "rewrite.tmp";
        public const string DEFAULT_REWRITE_EXTENSION = "rewrite";
        public const string DEFAULT_DLL_EXTENSION = "dll";
        public const string DEFAULT_EXE_EXTENSION = "exe";
        public const string DEFAULT_PDB_EXTENSION = "pdb";


        public static CSharpParseOptions DefaultScriptParseOptions => CSharpParseOptions.Default.WithKind(SourceCodeKind.Script);

        public static CSharpParseOptions DefaultParseOptions => CSharpParseOptions.Default;

        /// <summary>
        ///     Creates a copy of the original file, without the things the <see cref="CSharpScript"/> resolver
        /// doesnt like.
        /// </summary>
        /// <param name="rewriteCandidate">The rewritten file.</param>
        /// <returns>null if the operation could not succeed</returns>
        /// <remarks>
        ///     This is rudimentary and probably got lots of bugs for edge 
        /// cases (like when you put classes in several namespaces in the same
        /// code file, or use egyptian bracing). 
        /// </remarks>
        [Obsolete("Remove this or hide it")]
        public static RewrittenFile CreateRewriteFile(RewrittenFile rewriteCandidate)
        {
            FileUtilities.RemoveIfPresentSoft(rewriteCandidate.RewrittenFilePath);

            var targetFileStream = new StreamWriter(rewriteCandidate.RewrittenFilePath);

            try
            {
                using (var sr = new StreamReader(rewriteCandidate.OriginalFilePath))
                {
                    //maybe there is a better way to do this aside from counting braces?
                    //http://stackoverflow.com/questions/32769630/how-to-compile-a-c-sharp-file-with-roslyn-programmatically
                    var braceDepth = 0;

                    var inBlockComment = false;
                    var inNamespace = false;

                    while (sr.EndOfStream == false)
                    {
                        var line = sr.ReadLine();
                        if (line == null)
                        {
                            break;
                        }

                        var trimStart = line.TrimStart();
                        var trimEnd = line.TrimEnd();

                        if (trimStart.StartsWith("//"))
                        {
                            targetFileStream.WriteLine(line);
                            continue;
                        }

                        // while in block comments, dont count bracing
                        if (trimStart.StartsWith("/*"))
                        {
                            inBlockComment = true;
                            targetFileStream.WriteLine(line);
                            continue;
                        }

                        if (inBlockComment)
                        {
                            if (trimEnd.EndsWith("*/"))
                            {
                                inBlockComment = false;
                            }
                            targetFileStream.WriteLine(line);
                            continue;
                        }

                        var openingBraceCountForThisLine = trimStart.Length - trimStart.Replace("{", string.Empty).Length;
                        var closingBraceCountForThisLine = trimStart.Length - trimStart.Replace("}", string.Empty).Length;

                        // ReSharper disable StringIndexOfIsCultureSpecific.1
                        if (trimStart.IndexOf("namespace") >= 0)
                            // ReSharper restore StringIndexOfIsCultureSpecific.1
                        {
                            var stackedNamespaces = BuildStackedNamespacePaths(trimStart);
                            targetFileStream.WriteLine(stackedNamespaces.Select(n => $"using {n};"));
                            inNamespace = true;
                            braceDepth += openingBraceCountForThisLine;
                            braceDepth -= closingBraceCountForThisLine;
                            continue;
                        }

                        braceDepth += openingBraceCountForThisLine;
                        braceDepth -= closingBraceCountForThisLine;
                        //bool anythingBetweenBraces; //no nasty one liners

                        if (inNamespace && openingBraceCountForThisLine > 0
                            && braceDepth == 1
                            && closingBraceCountForThisLine < openingBraceCountForThisLine)
                        {
                            // "{", "{{", "{{ }", etc
                            targetFileStream.WriteLine();
                            continue;
                        }

                        if (inNamespace && closingBraceCountForThisLine > 0
                            && braceDepth == 0
                            && closingBraceCountForThisLine > openingBraceCountForThisLine)
                        {
                            // "}", "}}}", "{ }}", etc
                            //targetFileStream.WriteLine();
                            continue;
                        }

                        targetFileStream.WriteLine(line);
                    } //while reading stream
                } //using streamreader
            }
            finally
            {
                targetFileStream.Flush();
                targetFileStream.Close();
            }

            return rewriteCandidate;
        }

        public static CsExtraction ExtractCompilationDetailFromClassFile(string rewriteCandidateFilePath)
        {
            var scriptCode = FileUtilities.GetFileContentSoft(rewriteCandidateFilePath);
            var mainCompilationUnit = CompilationHelpers.GetRootMainCompilationUnit(scriptCode);
            if (mainCompilationUnit == null)
            {
                return new CsExtraction(new List<string> {"Could not get main compilation unit"}, rewriteCandidateFilePath);
            }

            var metadataReferences = GetMetadataReferenceAssemblies(scriptCode);

            var namespaceMembersToCompile = new List<SyntaxTree>();
            var allUsingsAcrossCompilationUnit = new List<UsingDirectiveSyntax>();

            allUsingsAcrossCompilationUnit.AddRange(mainCompilationUnit.Usings);

            var mcuNamespaces = mainCompilationUnit.Members.Where(m => m.IsKind(SyntaxKind.NamespaceDeclaration));
            foreach (var mcuNamespace in mcuNamespaces)
            {
                var usingsForThisCompilationUnit = new List<UsingDirectiveSyntax>(mainCompilationUnit.Usings);

                var namespaceDeclarationSyntax = mcuNamespace as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax == null)
                {
                    continue;
                }

                var outsideOfNamespaceUsings = namespaceDeclarationSyntax.Usings.ToList();
                allUsingsAcrossCompilationUnit.AddRange(outsideOfNamespaceUsings);
                usingsForThisCompilationUnit.AddRange(outsideOfNamespaceUsings);

                foreach (var member in namespaceDeclarationSyntax.Members)
                {
                    var msyntaxTree = CSharpSyntaxTree.ParseText(member.GetText(), DefaultScriptParseOptions);
                    var memberRoot = msyntaxTree.GetRoot();
                    var insideTheNamespaceMember = memberRoot as CompilationUnitSyntax;
                    if (insideTheNamespaceMember == null)
                    {
                        continue;
                    }

                    var innerNamespaceUsings = insideTheNamespaceMember.Usings.ToList();
                    allUsingsAcrossCompilationUnit.AddRange(innerNamespaceUsings);
                    usingsForThisCompilationUnit.AddRange(innerNamespaceUsings);

                    var classDeclarations = insideTheNamespaceMember.Members.Where(c => c.IsKind(SyntaxKind.ClassDeclaration));

                    foreach (var classDecl in classDeclarations)
                    {
                        var ccu = classDecl.SyntaxTree.GetCompilationUnitRoot();

                        // a few days wasted... 
                        //ccu.Usings.AddRange(usingsForThisCompilationUnit); lol, nope
                        foreach (var u in usingsForThisCompilationUnit)
                        {
                            //ccu.Usings.Add(u); // not that either
                            //ccu.AddUsings(u); // that doesn't work
                            //ccu.AddUsings(SyntaxFactory.UsingDirective(u.Name)); // not this either
                            //ccu.WithUsings(new SyntaxList<UsingDirectiveSyntax> {u}); // R# hints this may be a problem

                            // why did MS put Add() and With() members on an object that is immutable (though not named as such)?
                            // and why dont we get a decent runtime error instead of a HINT from resharper

                            //ccu = ccu.WithUsings(new SyntaxList<UsingDirectiveSyntax> { u }); //no, this doesnt work either
                            // with and add don't do the same thing, yet they are equivalent additive ideas. "This and That"
                            // is the same as "This with That" - you still end up with both.
                            ccu = ccu.AddUsings(SyntaxFactory.UsingDirective(u.Name).NormalizeWhitespace());
                        }

                        var classDeclSyntaxTree = CSharpSyntaxTree.Create(ccu, DefaultScriptParseOptions);
                        namespaceMembersToCompile.Add(classDeclSyntaxTree);
                    }
                }
            }

            var references = metadataReferences as MetadataReference[] ?? metadataReferences.ToArray();
            var allNamespaces = GetListOfNamespaces(allUsingsAcrossCompilationUnit.Select(u => u.Name.ToString()), metadataReferences: references);

            return new CsExtraction(references, namespaceMembersToCompile, allNamespaces, rewriteCandidateFilePath);
        }

        private static IEnumerable<MetadataReference> GetMetadataReferenceAssemblies(string codeAsScript)
        {
            var compilation = CSharpScript.Create(codeAsScript).GetCompilation();
            var metadataReferences = compilation.References.Where(r => r.Properties.Kind == MetadataImageKind.Assembly);
            return metadataReferences;
        }

        private static List<string> GetListOfNamespaces(IEnumerable<string> namespacesToUseVerbatim = null, IEnumerable<Assembly> assemblies = null,
            IEnumerable<MetadataReference> metadataReferences = null)
        {
            var listOfUsings = new List<string>();
            if (namespacesToUseVerbatim != null)
            {
                listOfUsings.AddRange(namespacesToUseVerbatim.Distinct());
            }

            var asmList = new List<Assembly>();

            if (metadataReferences != null)
            {
                foreach (var mr in metadataReferences)
                {
                    asmList.Add(mr.GetType().Assembly);
                }
            }

            if (assemblies != null)
            {
                asmList.AddRange(assemblies);
            }

            foreach (var asm in asmList)
            {
                var asmNs = asm.GetTypes()
                    .Where(t => string.IsNullOrWhiteSpace(t.Namespace) == false
                                && t.IsPublic == true)
                    .Select(t => t.Namespace)
                    .Distinct();

                foreach (var eans in asmNs)
                {
                    if (listOfUsings.Contains(eans))
                    {
                        continue;
                    }
                    listOfUsings.Add(eans);
                }
            }


            return listOfUsings;
        }


        /// <summary>
        ///     Converts a namespace into a set of additive usings
        /// </summary>
        /// <param name="trimStartNamespace">The trim start namespace.</param>
        /// <returns></returns>
        /// <example>
        /// Given the line "namespace Company.Product.Application.Module"
        /// This returns "using Company;using Company.Product;.using Company.Product.Application;using Company.Product.Application.Module;"
        /// </example>
        /// <remarks>
        ///     This may be needed when extracting class information. 
        /// </remarks>
        public static List<string> BuildStackedNamespacePaths(string trimStartNamespace)
        {
            var namespaceValue = trimStartNamespace.Replace("namespace", string.Empty).Trim();
            var endRemoved = namespaceValue.Split(' ');
            var parts = endRemoved[0].Split('.');
            var partsAsBuilt = new StringBuilder();
            var returnValue = new List<string>();
            foreach (var part in parts)
            {
                partsAsBuilt.Append(part);
                returnValue.Add(partsAsBuilt.ToString());
                partsAsBuilt.Append(".");
            }
            return returnValue;
        }


        /// <summary>
        /// Gets the rewrite file path.
        /// </summary>
        /// <param name="checkedSourcePath">The normalized path.</param>
        /// <returns></returns>
        public static string GetRewriteFilePath(string checkedSourcePath)
        {
            return $"{checkedSourcePath}.{Path.GetRandomFileName()}.{DEFAULT_REWRITE_TEMP_EXTENSION}";
        }

        /// <summary>
        /// Gets the rewrite assembly paths.
        /// </summary>
        /// <param name="checkedSourcePath">The checked source.</param>
        /// <returns></returns>
        public static AsmDetail GetRewriteAssemblyPaths(string checkedSourcePath)
        {
            return GetRewriteAssemblyPaths(checkedSourcePath, DEFAULT_DLL_EXTENSION);
        }

        /// <summary>
        ///     Gets the debug host paths.
        /// </summary>
        /// <param name="checkedSourcePath">The checked source path.</param>
        /// <returns></returns>
        public static AsmDetail GetDebugHostPaths(string checkedSourcePath)
        {
            return GetRewriteAssemblyPaths(checkedSourcePath, DEFAULT_EXE_EXTENSION);
        }

        private static AsmDetail GetRewriteAssemblyPaths(string checkedSourcePath, string extension)
        {
            var name = $"{Path.GetRandomFileName()}.{DEFAULT_REWRITE_EXTENSION}";
            var existingFileName = Path.GetFileName(checkedSourcePath);
            var basePath = $"{checkedSourcePath}.{name}";

            return new AsmDetail
            {
                AsmName = $"{existingFileName}.{name}",
                AsmPath = $"{basePath}.{extension}",
                PdbPath = $"{basePath}.{DEFAULT_PDB_EXTENSION}"
            };
        }
    }
}