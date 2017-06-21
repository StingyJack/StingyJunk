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

