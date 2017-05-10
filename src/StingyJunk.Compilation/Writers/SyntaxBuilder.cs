namespace StingyJunk.Compilation.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Misc;

    /// <summary>
    ///     Builds commonly used combinations of syntax elements
    /// </summary>
    public static class SyntaxBuilder
    {
        #region "namespace"

        public static CompilationUnitSyntax NamespaceWrapper(string namespaceName, List<string> usingsList, MemberDeclarationSyntax[] members)
        {
            var usings = new List<UsingDirectiveSyntax>();
            foreach (var u in usingsList)
            {
                usings.Add(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(u).WithLeadingTrivia(SyntaxFactory.Space)));
            }

            return SyntaxFactory.CompilationUnit()
                .AddMembers(
                    SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(namespaceName)
                            .WithLeadingTrivia(SyntaxFactory.Space)
                            .WithTrailingTrivia(SyntaxFactory.Space, SyntaxFactory.CarriageReturnLineFeed))
                        .AddUsings(usings.ToArray())
                        .AddMembers(members));
        }


        /// <summary>
        ///     Applies any usings to the result that are in the mcu
        /// </summary>
        /// <param name="mcu"></param>
        /// <returns></returns>
        public static CompilationUnitSyntax BuildAsyncNamespaceCompilationUnit(CompilationUnitSyntax mcu = null)
        {
            var newCompilationUnit = SyntaxFactory.CompilationUnit();
            if (mcu == null)
            {
                return newCompilationUnit;
            }
            return newCompilationUnit.WithUsings(mcu.Usings).AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading.Tasks")));
        }

        #endregion

        #region "class"

        public static ClassDeclarationSyntax BuildClassWrapper(string className, MemberDeclarationSyntax[] fieldMembers,
            MemberDeclarationSyntax[] methods)
        {
            return SyntaxFactory.ClassDeclaration(className)
                .AsPublic()
                .WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.Space)
                .WithTrailingTrivia(SyntaxFactory.Space, SyntaxFactory.CarriageReturnLineFeed)
                .AddMembers(fieldMembers)
                .AddMembers(methods)
                .WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.Space)
                .WithTrailingTrivia(SyntaxFactory.Space, SyntaxFactory.CarriageReturnLineFeed);
        }

        public static InterfaceDeclarationSyntax BuildAsyncInterfaceDeclFromSyncInterfaceDecl(string serviceContractName, string serviceContractAsyncName,
            InterfaceDeclarationSyntax existingInterfaceDecl, bool useExistingAsBase)
        {
            var newInterfaceIdentifier = SyntaxFactory.Identifier(serviceContractAsyncName).WithoutTrivia();
            var newInterfaceDecl = existingInterfaceDecl.WithIdentifier(newInterfaceIdentifier)
                .WithMembers(new SyntaxList<MemberDeclarationSyntax>()); //Why not convert the members too?

            if (useExistingAsBase)
            {
                newInterfaceDecl = newInterfaceDecl.WithBaseList(SyntaxFactory.BaseList(SyntaxFactory.Token(SyntaxKind.ColonToken),
                    SyntaxFactory.SeparatedList<BaseTypeSyntax>()
                        .Add(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(serviceContractName)))));
            }

            return newInterfaceDecl.WithoutTrivia()
                .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken));
        }

        public static ClassDeclarationSyntax BuildClientBaseClassDecl(string newClientBaseName, string serviceContractName)
        {
            //svc util will generate these
            var constructors = CSharpSyntaxTree.ParseText($"public {newClientBaseName} () {{}}"
                                                          + $"public {newClientBaseName} (string endpointConfigurationName) : base(endpointConfigurationName) {{}}"
                                                          +
                                                          $"public {newClientBaseName} (string endpointConfigurationName, string remoteAddress) :base(endpointConfigurationName, remoteAddress) {{}}"
                                                          +
                                                          $"public {newClientBaseName} (string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress) {{}}"
                                                          + $"public {newClientBaseName} (Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) {{}}"
                                                          + " ");


            return SyntaxFactory.ClassDeclaration(newClientBaseName)
                .WithBaseList(SyntaxFactory.BaseList(SyntaxFactory.Token(SyntaxKind.ColonToken),
                    SyntaxFactory.SeparatedList<BaseTypeSyntax>()
                        .Add(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"ClientBase<{serviceContractName}>")))
                        .Add(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(serviceContractName)))))
                .AddMembers(constructors.GetCompilationUnitRoot().Members.ToArray())
                .AsPublic();
        }

        #endregion //#region "type"

        #region "method related"

        public static MethodDeclarationSyntax Method(string methodName, StatementSyntax statements, TypeSyntax returnType = null)
        {
            var identifier = BuildMethodIdentifier(methodName);
            var returnSyntax = returnType ?? ReturnVoid;
            var voidMain = SyntaxFactory.MethodDeclaration(returnSyntax, identifier)
                .WithBody(SyntaxFactory.Block(statements))
                .WithLeadingTrivia(SyntaxFactory.Space)
                .WithTrailingTrivia(SyntaxFactory.Space, SyntaxFactory.CarriageReturnLineFeed);

            return voidMain;
        }

        public static MethodDeclarationSyntax BuildTaskAsyncWrapperForSyncInterfaceMethod(MethodDeclarationSyntax existingMethodDecl)
        {
            var existingServiceActionName = existingMethodDecl.Identifier.ValueText;
            var existingAttributeLists = existingMethodDecl.AttributeLists;
            var asyncContractAttributes = BuildOperationContractAttributeForAsyncMethod(existingServiceActionName, existingAttributeLists);
            var newMethodIdentifier = BuildNewAsyncMethodIdentifier(existingMethodDecl);


            var newMethodDecl = existingMethodDecl.WithIdentifier(newMethodIdentifier)
                .WithAttributeLists(asyncContractAttributes)
                .AsReturnTask();
            return newMethodDecl;
        }

        public static MethodDeclarationSyntax BuildSyncMethod(MethodDeclarationSyntax existingMethodDecl)
        {
            var newMethodIdentifier = existingMethodDecl.Identifier;
            var newMethodReturnType = existingMethodDecl.ReturnType;

            var statements = BuildOperationContractStatements(existingMethodDecl, newMethodIdentifier, false);
            //Channel.VerifyItemsForAggregation(token, documentNumber, station, containerSerial, itemSerials, baseIdentifier, itemUom, containerSize, isPartial);

            var newMethodDecl = SyntaxFactory.MethodDeclaration(newMethodReturnType, newMethodIdentifier)
                .WithParameterList(existingMethodDecl.ParameterList)
                .WithBody(SyntaxFactory.Block(statements))
                .AsPublic();

            return newMethodDecl;
        }

        private static StatementSyntax BuildOperationContractStatements(MethodDeclarationSyntax methodDecl,
            SyntaxToken newMethodIdentifier, bool isAsync)
        {
            var paramList = string.Join(", ", methodDecl.ParameterList.Parameters.Select(p => p.Identifier.Value));
            var isVoid = methodDecl.IsVoidReturnType();
            string returnStatement;
            if (isAsync == false && isVoid)
            {
                returnStatement = "Channel.";
            }
            else if (isAsync == true && isVoid == true)
            {
                returnStatement = "await Channel.";
            }
            else if (isAsync == true)
            {
                returnStatement = "return await Channel.";
            }
            else
            {
                returnStatement = "return Channel.";
            }
            var statementText = $"{returnStatement}{newMethodIdentifier.ValueText}({paramList});";
            var statements = SyntaxFactory.ParseStatement(statementText);
            return statements;
        }

        public static MethodDeclarationSyntax BuildTaskAsyncWrapperForSyncMethod(MethodDeclarationSyntax existingMethodDecl)
        {
            var newMethodIdentifier = BuildNewAsyncMethodIdentifier(existingMethodDecl);
            var newMethodReturnType = BuildNewAsyncMethodReturnType(existingMethodDecl);

            var statements = BuildOperationContractStatements(existingMethodDecl, newMethodIdentifier, true);
            //return Channel.VerifyItemsForAggregationAsync(token, documentNumber, station, containerSerial, itemSerials, baseIdentifier, itemUom, containerSize, isPartial);

            var newMethodDecl = SyntaxFactory.MethodDeclaration(newMethodReturnType, newMethodIdentifier)
                .WithParameterList(existingMethodDecl.ParameterList)
                .WithBody(SyntaxFactory.Block(statements))
                .AsPublic()
                .AsVirtual()
                .AsAsync();

            return newMethodDecl;
        }

        public static TypeSyntax BuildNewAsyncMethodReturnType(MethodDeclarationSyntax existingMethodDecl)
        {
            var predefinedTypeSyntax = existingMethodDecl.ReturnType as PredefinedTypeSyntax;
            TypeSyntax newMethodReturnType;
            if (predefinedTypeSyntax != null && predefinedTypeSyntax.Keyword.IsKind(SyntaxKind.VoidKeyword))
            {
                newMethodReturnType = SyntaxFactory.ParseTypeName("Task");
            }
            else
            {
                newMethodReturnType = SyntaxFactory.GenericName("Task").AddTypeArgumentListArguments(existingMethodDecl.ReturnType.WithoutTrivia());
            }
            return newMethodReturnType;
        }

        public static SyntaxList<AttributeListSyntax> BuildOperationContractAttributeForAsyncMethod(string existingServiceActionName,
            SyntaxList<AttributeListSyntax> existingAttributeLists)
        {
            var actionAttributeValue = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(existingServiceActionName));
            var actionAttribute = SyntaxFactory.AttributeArgument(SyntaxFactory.NameEquals("Action"), null, actionAttributeValue);
            var replyAttributeValue = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal($"{existingServiceActionName}Response"));
            var replyActionAttribute = SyntaxFactory.AttributeArgument(SyntaxFactory.NameEquals("ReplyAction"), null, replyAttributeValue);

            var separatedSyntaxListForAttribArgs = new SeparatedSyntaxList<AttributeArgumentSyntax>();
            separatedSyntaxListForAttribArgs = separatedSyntaxListForAttribArgs.Add(actionAttribute);
            separatedSyntaxListForAttribArgs = separatedSyntaxListForAttribArgs.Add(replyActionAttribute);
            var attributeArgs = SyntaxFactory.AttributeArgumentList(separatedSyntaxListForAttribArgs);

            var operationContractAttribute = existingAttributeLists.First().Attributes.First();
            var newOperationContractAttribute = operationContractAttribute.WithArgumentList(attributeArgs);
            var separatedAttributeList = SyntaxFactory.SeparatedList<AttributeSyntax>();
            separatedAttributeList = separatedAttributeList.Add(newOperationContractAttribute);
            var attrList = SyntaxFactory.AttributeList(separatedAttributeList);
            var yetAnotherFuckingList = new SyntaxList<AttributeListSyntax>();
            yetAnotherFuckingList = yetAnotherFuckingList.Add(attrList);
            return yetAnotherFuckingList;
        }


        public static SyntaxToken BuildNewAsyncMethodIdentifier(MethodDeclarationSyntax existingMethodDecl)
        {
            var newMethodIdentifierText = $"{existingMethodDecl.Identifier}Async";
            var newMethodIdentifier = SyntaxFactory.Identifier(newMethodIdentifierText);
            return newMethodIdentifier;
        }

        public static PredefinedTypeSyntax ReturnVoid => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));

        public static SyntaxToken BuildMethodIdentifier(string methodName)
        {
            var methodIdentifier = SyntaxFactory.Identifier(SyntaxTriviaList.Create(SyntaxFactory.Space),
                methodName, SyntaxTriviaList.Create(SyntaxFactory.Space));
            return methodIdentifier;
        }

        #endregion //#region "method related"

        #region "field"

        public static FieldDeclarationSyntax BuildField(string scriptFilePath, Type type, string name)
        {
            var fieldTypeName = type.Name;
            var argList = SyntaxFactory.ArgumentList()
                .AddArguments(
                    SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(scriptFilePath))));
            var field = SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(fieldTypeName))
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(name))
                        .WithInitializer(
                            SyntaxFactory.EqualsValueClause(
                                SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(fieldTypeName))
                                    .WithArgumentList(argList)
                                    .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword)))))))
                .AsPublic();
            return field;
        }

        #endregion //#region "field"
    }
}