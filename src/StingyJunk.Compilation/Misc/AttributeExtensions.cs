namespace StingyJunk.Compilation.Misc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class AttributeExtensions
    {
        public static bool IsServiceContract(this InterfaceDeclarationSyntax existingInterfaceDecl)
        {
            var serviceContractAttributeNames = new List<string> {nameof(ServiceContractAttribute), nameof(ServiceContractAttribute).Replace(nameof(Attribute), string.Empty)};

            var attributes = existingInterfaceDecl.AttributeLists.SelectMany(a => a.Attributes.Select(b => (IdentifierNameSyntax) b.Name)).ToList();

            var isServiceContract = HasMatchingAttribute(attributes, serviceContractAttributeNames);

            return isServiceContract;
        }

        public static bool HasMatchingAttribute(IEnumerable<IdentifierNameSyntax> attributes, List<string> targetAttributeNames)
        {
            var isServiceContract = false;
            foreach (var attribute in attributes)
            {
                if (targetAttributeNames.Any(t => t.Equals(attribute.Identifier.ValueText)))
                {
                    isServiceContract = true;
                }
            }

            return isServiceContract;
        }

        public static bool IsOperationContract(this MethodDeclarationSyntax methodDeclaration)
        {
            var operationContractAttributeNames = new List<string>
            {
                nameof(OperationContractAttribute),
                nameof(OperationContractAttribute).Replace(nameof(Attribute), string.Empty)
            };

            var attributes = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes.Select(b => (IdentifierNameSyntax) b.Name)).ToList();

            var isServiceContract = HasMatchingAttribute(attributes, operationContractAttributeNames);

            return isServiceContract;
        }
    }

    public static class Attributes
    {
        public static AttributeListSyntax Serializable
        {
            get
            {
                var list = new SeparatedSyntaxList<AttributeSyntax>();
                list = list.Add(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(typeof(SerializableAttribute).Name)));
                var attrs = SyntaxFactory.AttributeList(list);
                return attrs;
            }
        }
    }
}