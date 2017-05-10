namespace StingyJunk.Compilation.Bags
{
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class InterfaceDefinition
    {
        public InterfaceDefinition(string serviceContractName, string serviceContractFilePath)
        {
            ServiceContractName = serviceContractName;
            ServiceContractFilePath = serviceContractFilePath;
        }

        public string ServiceContractName { get; }
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string ServiceContractFilePath { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        public string ServiceContractAsyncName => $"{ServiceContractName}Async";

        public string ClientContractAsyncName => $"{ServiceContractName}AsyncClient";
        public string ClientContractFilePath { get; set; }

        public string ServiceClientName => $"{ServiceContractName.Remove(0, 1)}AsyncClient";
        public string ServiceClientFilePath { get; set; }

        public SyntaxTree ExistingServiceContractSyntaxTree { get; set; }
        public CompilationUnitSyntax NewServiceContractCompilationUnitSyntax { get; set; }
        public CompilationUnitSyntax NewClientCompilationUnitSyntax { get; set; }


        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var prop in GetType().GetProperties().OrderBy(p => p.Name))
            {
                if (prop.Name == nameof(ExistingServiceContractSyntaxTree) || prop.Name == nameof(NewServiceContractCompilationUnitSyntax))
                {
                    continue;
                }
                result.AppendLine($"\t\t{prop.Name} : {prop.GetValue(this)}");
            }
            return result.ToString();
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