namespace StingyJunk.Compilation.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bags;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;
    using Misc;

    public class Jennay
    {
        protected internal bool _UseExistingAsBase = false;

        public GenResult RunFawrest(string serviceContractsFolder, string csProjFile)
        {
            var result = new GenResult();
            result.Info($"Starting {DateTime.Now}");
            result.Info($"Scanning {serviceContractsFolder}");
            var serviceContractDefinitions = GetExistingServiceContractDefinitions(serviceContractsFolder, ref result);
            result.Info($"Found {serviceContractDefinitions.Count} to process");

            foreach (var existingServiceContract in serviceContractDefinitions)
            {
                var mcu = existingServiceContract.ExistingServiceContractSyntaxTree.GetCompilationUnitRoot();
                var newCompilationUnit = SyntaxBuilder.BuildAsyncNamespaceCompilationUnit(mcu);

                result.Debug($"new compilation unit: {newCompilationUnit.GetText()}");

                foreach (var namespaceDeclaration in mcu.Members.Where(m => m.Kind() == SyntaxKind.NamespaceDeclaration))
                {
                    var existingNamespaceDecl = (NamespaceDeclarationSyntax) namespaceDeclaration;
                    var newNamespaceDecl = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(existingNamespaceDecl.Name.ToString()));
                    result.Debug($"new namespace: {newNamespaceDecl.GetText()}");

                    var newTypes = new SyntaxList<MemberDeclarationSyntax>();
                    var interfaceDeclarations = existingNamespaceDecl.Members.Where(m => m.Kind() == SyntaxKind.InterfaceDeclaration).Select(m => m.WithoutTrivia());

                    //if this has the service contract attribute and it matches the file name, its the target we want to harvest 
                    // [OperationContract] from.

                    foreach (var interfaceDeclaration in interfaceDeclarations)
                    {
                        var existingInterfaceDecl = (InterfaceDeclarationSyntax) interfaceDeclaration;

                        if (existingInterfaceDecl.IsServiceContract() == false) { continue; }

                        var newInterfaceDecl = SyntaxBuilder.BuildAsyncInterfaceDeclFromSyncInterfaceDecl(existingServiceContract.ServiceContractName,
                            existingServiceContract.ServiceContractAsyncName, existingInterfaceDecl, _UseExistingAsBase);
                        result.Debug($"new interface: {newInterfaceDecl.GetText()}");

                        var newClientClassDecl = SyntaxBuilder.BuildClientBaseClassDecl(existingServiceContract.ServiceClientName,
                            existingServiceContract.ServiceContractAsyncName);
                        result.Debug($"new clientbase : {newClientClassDecl.GetText()}");

                        var interfaceMethodsToAdd = new SyntaxList<MemberDeclarationSyntax>();
                        var clientClassMethodsToAdd = new SyntaxList<MemberDeclarationSyntax>();

                        foreach (var methodDeclaration in existingInterfaceDecl.Members.Where(m => m.Kind() == SyntaxKind.MethodDeclaration).Select(m => m.WithoutTrivia()))
                        {
                            var existingMethodDecl = (MethodDeclarationSyntax) methodDeclaration;
                            if (existingMethodDecl.IsOperationContract() == false) { continue; }

                            var newInterfaceMethodDecl = SyntaxBuilder.BuildTaskAsyncWrapperForSyncInterfaceMethod(existingMethodDecl);
                            interfaceMethodsToAdd = interfaceMethodsToAdd.Add(newInterfaceMethodDecl);
                            result.Debug($"new interface method decl: {newInterfaceMethodDecl.GetText()}");

                            var newClientClassMethodDeclAsync = SyntaxBuilder.BuildTaskAsyncWrapperForSyncMethod(existingMethodDecl);
                            clientClassMethodsToAdd = clientClassMethodsToAdd.Add(newClientClassMethodDeclAsync);
                            result.Debug($"new client class method async decl: {newClientClassMethodDeclAsync.GetText()}");

                            if (_UseExistingAsBase)
                            {
                                var newClientClassMethodDecl = SyntaxBuilder.BuildSyncMethod(existingMethodDecl);
                                clientClassMethodsToAdd = clientClassMethodsToAdd.Add(newClientClassMethodDecl);
                                result.Debug($"new client class method decl: {newClientClassMethodDecl.GetText()}");
                            }
                        }

                        newInterfaceDecl = newInterfaceDecl.WithMembers(interfaceMethodsToAdd).WithoutTrivia();
                        newTypes = newTypes.Add(newInterfaceDecl);

                        newClientClassDecl = newClientClassDecl.WithMembers(clientClassMethodsToAdd).WithoutTrivia();
                        newTypes = newTypes.Add(newClientClassDecl);
                    }
                    newNamespaceDecl = newNamespaceDecl.WithMembers(newTypes).WithoutTrivia();

                    newCompilationUnit = newCompilationUnit.AddMembers(newNamespaceDecl)
                        .NormalizeWhitespace()
                        .WithAdditionalAnnotations(Formatter.Annotation);
                    if (newNamespaceDecl.Members.Any())
                    {
                        existingServiceContract.NewServiceContractCompilationUnitSyntax = newCompilationUnit;
                    }
                }
            }

            foreach (var gennedCode in BuildOutputText(serviceContractDefinitions))
            {
                result.AddGeneratedCode(gennedCode.Key, gennedCode.Value);
            }

            AttemptCompilation(csProjFile, ref result, result.GeneratedOutput);

            result.Info($"Completed {DateTime.Now}");
            return result;
        }

        private static List<InterfaceDefinition> GetExistingServiceContractDefinitions(string serviceContractsFolder, ref GenResult result, List<string> exclusions = null)
        {
            var syncInterfaceFiles = Directory.GetFiles(serviceContractsFolder, "I*.*");
            var interfacesToCreate = new List<InterfaceDefinition>();

            foreach (var syncInterface in syncInterfaceFiles)
            {
                var fi = new FileInfo(syncInterface);
                var fileName = fi.Name;
                var isExcluded = exclusions != null && exclusions.Any(e => e.Equals(fileName, StringComparison.OrdinalIgnoreCase));

                if (isExcluded)
                {
                    result.Debug($"Skipping file {syncInterface}");
                    continue;
                }

                var serviceContractName = Path.GetFileNameWithoutExtension(fileName);
                result.Debug($"Building definition for {serviceContractName} from {fileName}");
                var idef = new InterfaceDefinition(serviceContractName, fi.FullName)
                {
                    ExistingServiceContractSyntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(fi.FullName))
                };

                result.Debug($"Info definition {Environment.NewLine} {idef}");


                interfacesToCreate.Add(idef);
            }
            return interfacesToCreate;
        }

        private static void AttemptCompilation(string csProjFile, ref GenResult result, ReadOnlyDictionary<string, string> genText)
        {
            if (string.IsNullOrWhiteSpace(csProjFile) == false && File.Exists(csProjFile))
            {
                result.Info($"Using proj file {csProjFile} to determine references");


                var assemblyName = Path.GetRandomFileName();
                var references = CompilationHelpers.GetMetadataReferences(csProjFile).ToArray();
                result.Debug($"Found {references.Length} references");

                foreach (var genCode in genText)
                {
                    result.Info($"Compiling {genCode.Key}...");
                    var reTree = CSharpSyntaxTree.ParseText(genCode.Value);
                    var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                        .WithPlatform(Platform.AnyCpu) //get from proj file
                        .WithAssemblyIdentityComparer(DesktopAssemblyIdentityComparer.Default);
                    var compilation = CSharpCompilation.Create(assemblyName, new[] {reTree}, references, options);
                    var diags = compilation.GetDiagnostics();
                    result.Debug($"Compilation complete, found {diags.Length} Diagnostics");

                    result.AddDiags(diags);
                }
            }
            else
            {
                result.Info($"{nameof(csProjFile)} was either not specified or does not exist at path given. Skipping compilation");
            }
        }

        private static Dictionary<string, string> BuildOutputText(IEnumerable<InterfaceDefinition> interfacesToCreate)
        {
            var allOut = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var idef in interfacesToCreate.Where(i => i.NewServiceContractCompilationUnitSyntax != null))
            {
                var genOut = new StringBuilder();
                genOut.Append(idef.NewServiceContractCompilationUnitSyntax.GetText());
                genOut.AppendLine($"{Environment.NewLine}");
                var genText = genOut.ToString();
                allOut.Add(idef.ServiceContractAsyncName, genText);
            }

            return allOut;
        }
    }
}

/*
namespace Acsis.SM.Common.AlsoDeleteWhenDone
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;
    using Acsis.Common.ServiceReturnClasses;
    using Acsis.Common.UserSecurity;
    using Interfaces;
    using TableEntities;

    public class deleteme : ServiceClientBase<IAggregationServiceAsync>, IAggregationServiceClientAsync
    {
        protected UserToken Token { get; set; }
        public deleteme(UserToken token)
        {
            Token = token;
        }

        public async Task<Tuple<ServiceExecResult, SMSerializedItemAssociation, List<SMSerializedItemAssociation>>> VerifyItemsForAggregationAsync(string documentNumber,
            string station, string containerSerial, List<string> itemSerials,
            string baseIdentifier, string itemUom, int containerSize, bool isPartial)
        {
            var result = await GetServiceProxy().VerifyItemsForAggregationAsync(Token, documentNumber, station, containerSerial, itemSerials, baseIdentifier, itemUom, containerSize, isPartial);
            return result;
        }
    }



    /*
     *  [ServiceContract( Namespace="X", Name="TheContract" )]
       public interface IAsyncContractForClientAndService
       {
           [OperationContract]
           Task<TResponse> SendReceiveAsync( TRequest req );
       }



       [ServiceBehavior (InstanceContextMode = InstanceContextMode.Single, // (1)
                         // also works with InstanceContextMode.PerSession or PerCall
                         ConcurrencyMode     = ConcurrencyMode.Multiple,   // (2)
                         UseSynchronizationContext = true)]                // (3)

       public MyService : IAsyncContractForClientAndService
       {
           public async Task<TResponse> SendReceiveAsync( TRequest req )
           {
               DoSomethingSynchronous();
               await SomethingAsynchronous(); 
               // await lets other clients call the service here or at any await in
               // subfunctions. Calls from clients execute 'interleaved'.
               return new TResponse( ... );
           }
       }
     

    public abstract class ServiceClientBase<T> : ClientBase<T> where T : class
    {
        protected object _lockObject = new object();
        protected T _service;
        protected string _serviceUrl = "http://localhost/SPDMServiceHost/Services/AggregationService.svc";

        public T GetChannel(Binding binding, EndpointAddress endpoint)
        {
            var factory = new ChannelFactory<T>(binding, endpoint);
            var channel = factory.CreateChannel();
            return channel;
        }


        protected virtual T GetServiceProxy()
        {
            lock (_lockObject)
            {
                if (_service == null)
                {
                    var binding = new BasicHttpBinding();
                    var endpoint = new EndpointAddress(_serviceUrl);
                    _service = GetChannel(binding, endpoint);
                }
            }

            return _service;
        }

    }
}
*/