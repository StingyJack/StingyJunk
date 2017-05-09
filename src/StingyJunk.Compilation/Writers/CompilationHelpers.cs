namespace StingyJunk.Compilation.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Bags;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Emit;
    using Microsoft.CodeAnalysis.MSBuild;
    using Microsoft.CodeAnalysis.Scripting;
    using Misc;

    public static class CompilationHelpers
    {
        #region "compilation"

        public static ScriptCompilationResult GetCompilationForLoadDirective(CompileDirection compileDirection, string path, AsmDetail asmDetail,
            List<string> namespaces, IEnumerable<MetadataReference> metadataReferences)
        {
            var targetType = GetResolutionTargetType(path);
            var referencedCompilation = new ScriptCompilationResult();

            if (compileDirection == CompileDirection.EverythingBuiltAsClassesAndReffed)
            {
                if (targetType == ResolutionTargetType.Cs)
                {
                    var compilationUnit = GetRootMainCompilationUnit(FileUtilities.GetFileContent(path), CsRewriter.DefaultParseOptions);
                    referencedCompilation = CompileAndWriteAssembly(path, ImmutableList.Create(compilationUnit.SyntaxTree),
                        asmDetail, namespaces, metadataReferences);
                }
            }
            else //compileDirection == CompileDirection.OnlyClassesBuiltAsScriptsAndReffed
            {
                if (targetType == ResolutionTargetType.Cs)
                {
                    var extraction = CsRewriter.ExtractCompilationDetailFromClassFile(path);
                    referencedCompilation = CompileAndWriteScript(path, extraction.CompilationTargets.First(),
                        asmDetail, namespaces, metadataReferences);
                }
                else
                {
                    var compilationUnit = GetRootMainCompilationUnit(FileUtilities.GetFileContent(path));
                    referencedCompilation = CompileAndWriteAssembly(path, ImmutableList.Create(compilationUnit.SyntaxTree),
                        asmDetail, namespaces, metadataReferences);
                }
            }

            Debug.Assert(referencedCompilation != null, "load directive compilation should not be null",
                $"path '{path}' - compileDirection {compileDirection} - targetType {targetType}\n\n\n");

            return referencedCompilation;
        }

        public static ScriptCompilationResult CompileScript(string pathToScriptFile,
            ImmutableList<SyntaxTree> compilationSources, AsmDetail detailsToUseForTarget,
            IEnumerable<string> additionalUsings = null, IEnumerable<MetadataReference> additionalReferences = null)
        {
            var compilationPrep = PrepareCompilation(compilationSources, detailsToUseForTarget, additionalUsings, additionalReferences);

            var scriptCompilation = new ScriptCompilationResult();
            using (var dllStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var emitResult = compilationPrep.Compilation.Emit(dllStream, pdbStream);

                //success sometimes was false when there are warnings, but i didnt write it down
                // so maybe it was a specific kind. Or I should pay more attention.
                scriptCompilation.CompilationResult = emitResult;

                if (emitResult.Diagnostics.Count(d => d.Severity == DiagnosticSeverity.Error) == 0)
                {
                    scriptCompilation.IsCompiled = true;
                    dllStream.Seek(0, SeekOrigin.Begin);
                    pdbStream.Seek(0, SeekOrigin.Begin);
                    scriptCompilation.AssemblyBytes = dllStream.ToArray();
                    scriptCompilation.PdbBytes = pdbStream.ToArray();

                    scriptCompilation.AssemblyFilePath = detailsToUseForTarget.AsmPath;
                    scriptCompilation.PdbFilePath = detailsToUseForTarget.PdbPath;
                    scriptCompilation.FoundNamespaces.AddRange(compilationPrep.Usings);
                    scriptCompilation.FoundMetadataReferences.AddRange(compilationPrep.MetadataReferences);
                }
            }
            return scriptCompilation;
        }

        public static ScriptCompilationResult CompileAndWriteScript(string pathToScriptFile,
            SyntaxTree compilationSource, AsmDetail detailsToUseForTarget,
            IEnumerable<string> additionalUsings = null, IEnumerable<MetadataReference> additionalReferences = null)
        {
            var compilationPrep = PrepareScriptCompilation(compilationSource, detailsToUseForTarget, additionalUsings, additionalReferences);

            var emitResult = compilationPrep.Compilation.Emit(detailsToUseForTarget.AsmPath, detailsToUseForTarget.PdbPath);

            return BuildScriptCompilationResult(detailsToUseForTarget, emitResult, compilationPrep);
        }

        /// <summary>
        ///     Compiles the and writes to a PE.
        /// </summary>
        /// <param name="pathToScriptFile">The path to script file.</param>
        /// <param name="compilationSources">The compilation sources.</param>
        /// <param name="detailsToUseForTarget">The details to use for target.</param>
        /// <param name="additionalUsings">The additional usings.</param>
        /// <param name="additionalReferences">The additional references.</param>
        /// <param name="outputKind">Defaults to Dll if not specified</param>
        /// <returns></returns>
        public static ScriptCompilationResult CompileAndWriteAssembly(string pathToScriptFile,
            ImmutableList<SyntaxTree> compilationSources, AsmDetail detailsToUseForTarget,
            IEnumerable<string> additionalUsings = null, IEnumerable<MetadataReference> additionalReferences = null,
            OutputKind? outputKind = null)
        {
            var compilationPrep = PrepareCompilation(compilationSources, detailsToUseForTarget, additionalUsings, additionalReferences, outputKind);

            var emitResult = compilationPrep.Compilation.Emit(detailsToUseForTarget.AsmPath, detailsToUseForTarget.PdbPath);

            return BuildScriptCompilationResult(detailsToUseForTarget, emitResult, compilationPrep);
        }

        #endregion //#region "compilation"

        #region  "builders"

        public static CompilationUnitSyntax WrapScriptInStandardClass(CompilationUnitSyntax strippedScript,
            string namespaceName, string className, string methodName, string scriptFilePath, List<string> usings)
        {
            var scriptStatements = SyntaxFactory.ParseStatement(strippedScript.GetText().ToString());
            var voidMain = BuildVoidMainWaitForDebugger();
            var scriptMethod = SyntaxBuilder.Method(methodName, scriptStatements).AsPublic();
            var injectedContextField = SyntaxBuilder.BuildField(scriptFilePath, typeof(object), "_context");
            var wrappingClass = SyntaxBuilder.BuildClassWrapper(className,
                    new MemberDeclarationSyntax[] {injectedContextField}, new MemberDeclarationSyntax[] {voidMain, scriptMethod})
                .AsSerializable();

            var wrappingNamespace = SyntaxBuilder.NamespaceWrapper(namespaceName, usings,
                new MemberDeclarationSyntax[] {wrappingClass});

            return wrappingNamespace;
        }

        public static MethodDeclarationSyntax BuildVoidMainWaitForDebugger()
        {
            //add this
            var debuggerWait = CSharpSyntaxTree.ParseText(@"
public static void Main()
{
    var waitMax = 10*1000;
    var waited = 0;
    var waitInterval = 100;

    while (System.Diagnostics.Debugger.IsAttached == false && waited < waitMax)
    {
        System.Threading.Thread.Sleep(waitInterval);
        waited += waitInterval;
    }

    if (waitMax < waitInterval)
    {
        throw new Exception(""Failed to get debugger attached in time"");
    }

    new ScriptyDebugCls().ScriptyDebugMeth();
}");

            var dwMcu = GetRootMainCompilationUnit(debuggerWait);
            var memberDecl = (MethodDeclarationSyntax) dwMcu.Members.First();
            var statements = memberDecl.Body;

            var method = SyntaxBuilder.Method("Main", statements).AsPublic().AsStatic().AsReturnVoid();
            return method;
        }

        private static ScriptCompilationResult BuildScriptCompilationResult(AsmDetail detailsToUseForTarget, EmitResult emitResult,
            PreparedCompilation compilationPrep)
        {
            var result = new ScriptCompilationResult();
            result.CompilationResult = emitResult;

            if (emitResult.Diagnostics.Count(d => d.Severity == DiagnosticSeverity.Error) == 0)
            {
                result.IsCompiled = true;
                result.AssemblyFilePath = detailsToUseForTarget.AsmPath;
                result.PdbFilePath = detailsToUseForTarget.PdbPath;
                result.FoundNamespaces.AddRange(compilationPrep.Usings);
                result.FoundMetadataReferences.AddRange(compilationPrep.MetadataReferences);
            }

            return result;
        }

        #endregion //#region  "builders"

        #region "compilation assistants"

        private static CommonCompilationDetails BuildCommonCompilationDetails(IEnumerable<string> additionalUsings = null,
            IEnumerable<MetadataReference> additionalReferences = null)
        {
            var listOfUsings = new List<string>();
            if (additionalUsings != null)
            {
                listOfUsings.AddRange(additionalUsings);
            }

            var metadataReferences = new List<MetadataReference>();
            var executingAssembly = Assembly.GetExecutingAssembly();
            var callingAssembly = Assembly.GetCallingAssembly();

            metadataReferences.Add(MetadataReference.CreateFromFile(executingAssembly.Location));
            metadataReferences.Add(MetadataReference.CreateFromFile(callingAssembly.Location));

            if (additionalReferences != null)
            {
                foreach (var ar in additionalReferences)
                {
                    if (metadataReferences.Any(mr => mr.Display.Equals(ar.Display, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }
                    metadataReferences.Add(ar);
                }
            }


            return new CommonCompilationDetails
            {
                ListOfUsings = listOfUsings,
                MetadataReferences = metadataReferences.ToArray()
            };
        }

        private static PreparedCompilation PrepareCompilation(ImmutableList<SyntaxTree> compilationSources, AsmDetail detailsToUseForTarget,
            IEnumerable<string> additionalUsings = null, IEnumerable<MetadataReference> additionalReferences = null, OutputKind? outputKind = null)
        {
            var commonDetails = BuildCommonCompilationDetails(additionalUsings, additionalReferences);

            var outputKindForOptions = OutputKind.DynamicallyLinkedLibrary;

            if (outputKind.HasValue)
            {
                outputKindForOptions = outputKind.Value;
            }

            var options = new CSharpCompilationOptions(outputKindForOptions)
                .WithUsings(commonDetails.ListOfUsings)
                .WithAssemblyIdentityComparer(DesktopAssemblyIdentityComparer.Default);

            var compilation = CSharpCompilation.Create(detailsToUseForTarget.AsmName,
                compilationSources.ToArray(), commonDetails.MetadataReferences);

            return new PreparedCompilation
            {
                Options = options,
                Compilation = compilation,
                MetadataReferences = commonDetails.MetadataReferences,
                Usings = commonDetails.ListOfUsings
            };
        }


        private static PreparedCompilation PrepareScriptCompilation(SyntaxTree compilationSource, AsmDetail detailsToUseForTarget,
            IEnumerable<string> additionalUsings = null, IEnumerable<MetadataReference> additionalReferences = null)
        {
            var commonDetails = BuildCommonCompilationDetails(additionalUsings, additionalReferences);

            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithUsings(commonDetails.ListOfUsings);

            var compilation = CSharpCompilation.CreateScriptCompilation(detailsToUseForTarget.AsmName)
                //.WithReferences(commonDetails.MetadataReferences)
                .AddReferences(commonDetails.MetadataReferences)
                .WithOptions(options)
                .AddSyntaxTrees(compilationSource);


            return new PreparedCompilation
            {
                Options = options,
                Compilation = compilation,
                MetadataReferences = commonDetails.MetadataReferences,
                Usings = commonDetails.ListOfUsings
            };
        }

        /// <summary>
        /// Gets the main compilation unit for the root of the syntax tree. 
        /// </summary>
        /// <param name="scriptCode">The script code.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// null if unable to get the requested item
        /// </returns>
        public static CompilationUnitSyntax GetRootMainCompilationUnit(string scriptCode, CSharpParseOptions options = null)
        {
            if (options == null)
            {
                options = CsRewriter.DefaultScriptParseOptions;
            }

            var mainSyntaxTree = CSharpSyntaxTree.ParseText(scriptCode, options);

            return GetRootMainCompilationUnit(mainSyntaxTree);
        }

        /// <summary>
        /// Gets the main compilation unit for the root of the syntax tree. 
        /// </summary>
        /// <param name="syntaxTree">The syntax tree.</param>
        /// <returns>
        /// null if unable to get 
        /// </returns>
        public static CompilationUnitSyntax GetRootMainCompilationUnit(SyntaxTree syntaxTree)
        {
            SyntaxNode mainSyntaxTreeRoot;
            if (syntaxTree.TryGetRoot(out mainSyntaxTreeRoot) == false)
            {
                return null;
            }

            var mainCompilationUnit = mainSyntaxTreeRoot as CompilationUnitSyntax;
            if (mainCompilationUnit == null)
            {
                return null;
            }
            return mainCompilationUnit;
        }

        public static CSharpParseOptions GetParseOptions(CompileDirection compileDirection)
        {
            return compileDirection == CompileDirection.EverythingBuiltAsClassesAndReffed
                ? CsRewriter.DefaultParseOptions
                : CsRewriter.DefaultScriptParseOptions;
        }

        public static Assembly WriteScriptCompilationAsAssemblyAndLoadIt(ScriptCompilationResult compResult)
        {
            if (compResult.IsCompiled == false)
            {
                throw new CompilationErrorException("Failed to compile", compResult.CompilationResult.Diagnostics);
            }

            if (FileUtilities.WriteFile(compResult.AssemblyFilePath, compResult.AssemblyBytes) == false)
            {
                throw new IOException("Failed to write assembly dll to disk");
            }

            if (FileUtilities.WriteFile(compResult.PdbFilePath, compResult.PdbBytes) == false)
            {
                throw new IOException("Failed to write assembly pdb to disk");
            }


            return Assembly.LoadFile(compResult.AssemblyFilePath);
        }

        public static IEnumerable<Assembly> GetReferencedAssemblies(CompilationUnitSyntax executingScriptCompilationSource, string scriptFolder)
        {
            var assms = new List<Assembly>();
            foreach (var refDirective in executingScriptCompilationSource.GetReferenceDirectives())
            {
                var path = FileUtilities.BuildFullPath(scriptFolder, refDirective.File.ValueText);
                assms.Add(Assembly.ReflectionOnlyLoadFrom(path));
            }
            return assms;
        }

        public static Tuple<CompilationUnitSyntax, List<Assembly>> ExtractReferenceDirectives(CompilationUnitSyntax compilationUnit, string baseFolder)
        {
            var assms = new List<Assembly>();
            var refDirectives = compilationUnit.GetReferenceDirectives();

            foreach (var refDirective in refDirectives)
            {
                var path = FileUtilities.BuildFullPath(baseFolder, refDirective.File.ValueText);
                assms.Add(Assembly.ReflectionOnlyLoadFrom(path));
                compilationUnit = compilationUnit.RemoveNode(refDirective, SyntaxRemoveOptions.KeepNoTrivia);
            }
            return new Tuple<CompilationUnitSyntax, List<Assembly>>(compilationUnit, assms);
        }

        public static List<Assembly> GetReferencedAssemblies(Assembly targetAssembly)
        {
            var result = new List<Assembly>();

            foreach (var asm in targetAssembly.GetReferencedAssemblies())
            {
                var rol = Assembly.ReflectionOnlyLoad(asm.FullName);
                result.Add(rol);
            }
            return result;
        }

        public static IEnumerable<MetadataReference> GetMetadataReferences(string csprojFilePath)
        {
            var wks = MSBuildWorkspace.Create();
            wks.LoadMetadataForReferencedProjects = true;
            var proj = wks.OpenProjectAsync(csprojFilePath);
            Task.WaitAll(proj);
            return proj.Result.MetadataReferences;
        }

        #endregion //#region "compilation assistants"

        #region "intercept handling"

        public static ResolutionTargetType GetResolutionTargetType(string resolutionCandidateFilePath)
        {
            if (resolutionCandidateFilePath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            {
                return ResolutionTargetType.Cs;
            }
            if (resolutionCandidateFilePath.EndsWith(".csx", StringComparison.OrdinalIgnoreCase))
            {
                return ResolutionTargetType.Csx;
            }
            return ResolutionTargetType.Other;
        }

        #endregion // #region "file handling"
    }
}