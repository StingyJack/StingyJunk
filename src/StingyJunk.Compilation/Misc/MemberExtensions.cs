namespace StingyJunk.Compilation.Misc
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Writers;

    public static class MemberExtensions
    {
        #region "accessibility modifiers"

        public static SyntaxTokenList AllAccModifiers(this BaseMethodDeclarationSyntax methodDeclarationSyntax)
        {
            return methodDeclarationSyntax.Modifiers.Where(m => m.IsKind(SyntaxKind.PublicKeyword)
                                                                || m.IsKind(SyntaxKind.ProtectedKeyword)
                                                                || m.IsKind(SyntaxKind.InternalKeyword)
                                                                || m.IsKind(SyntaxKind.PrivateKeyword))
                .ToList();
        }

        public static SyntaxToken? FirstAccModifier(this BaseMethodDeclarationSyntax methodDeclarationSyntax)
        {
            return methodDeclarationSyntax.Modifiers.FirstOrDefault(m => m.IsKind(SyntaxKind.PublicKeyword)
                                                                         || m.IsKind(SyntaxKind.ProtectedKeyword)
                                                                         || m.IsKind(SyntaxKind.InternalKeyword)
                                                                         || m.IsKind(SyntaxKind.PrivateKeyword));
        }

        public static MethodDeclarationSyntax AsPublic(this MethodDeclarationSyntax methodDeclarationSyntax)
        {
            methodDeclarationSyntax = (MethodDeclarationSyntax) StripAccessModifiers(methodDeclarationSyntax);

            return methodDeclarationSyntax.WithModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword).ToList());
        }

        public static BaseMethodDeclarationSyntax StripAccessModifiers(BaseMethodDeclarationSyntax methodDeclarationSyntax)
        {
            var modifier = methodDeclarationSyntax.FirstAccModifier();
            while (modifier.HasValue)
            {
                methodDeclarationSyntax = methodDeclarationSyntax.RemoveNode(modifier.Value.Parent, SyntaxRemoveOptions.KeepNoTrivia);
                modifier = methodDeclarationSyntax.FirstAccModifier();
            }
            return methodDeclarationSyntax;
        }


        public static ConstructorDeclarationSyntax AsPublic(this ConstructorDeclarationSyntax constructorDeclarationSyntax)
        {
            constructorDeclarationSyntax = (ConstructorDeclarationSyntax) StripAccessModifiers(constructorDeclarationSyntax);
            return constructorDeclarationSyntax.WithModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword).ToList());
        }

        public static SyntaxToken? FirstAccModifier(this BaseFieldDeclarationSyntax baseFieldDeclarationSyntax)
        {
            return baseFieldDeclarationSyntax.Modifiers.FirstOrDefault(m => m.IsKind(SyntaxKind.PublicKeyword)
                                                                            || m.IsKind(SyntaxKind.ProtectedKeyword)
                                                                            || m.IsKind(SyntaxKind.InternalKeyword)
                                                                            || m.IsKind(SyntaxKind.PrivateKeyword));
        }

        public static BaseFieldDeclarationSyntax StripAccessModifiers(BaseFieldDeclarationSyntax baseFieldDeclarationSyntax)
        {
            var modifier = baseFieldDeclarationSyntax.FirstAccModifier();
            while (modifier.HasValue)
            {
                baseFieldDeclarationSyntax = baseFieldDeclarationSyntax.RemoveNode(modifier.Value.Parent, SyntaxRemoveOptions.KeepNoTrivia);
                modifier = baseFieldDeclarationSyntax.FirstAccModifier();
            }
            return baseFieldDeclarationSyntax;
        }

        public static FieldDeclarationSyntax AsPublic(this FieldDeclarationSyntax fieldDeclarationSyntax)
        {
            fieldDeclarationSyntax = (FieldDeclarationSyntax) StripAccessModifiers(fieldDeclarationSyntax);
            return fieldDeclarationSyntax.WithModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword).ToList());
        }

        public static FieldDeclarationSyntax AsStatic(this FieldDeclarationSyntax fieldDeclarationSyntax)
        {
            return fieldDeclarationSyntax.WithModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword).ToList());
        }

        #endregion //#region "accessibility modifiers"

        #region "behavioral modifiers"

        public static MethodDeclarationSyntax AsAsync(this MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return methodDeclarationSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
        }

        public static MethodDeclarationSyntax AsVirtual(this MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return methodDeclarationSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.VirtualKeyword));
        }

        public static MethodDeclarationSyntax AsStatic(this MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return methodDeclarationSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword));
        }

        #endregion //#region "behavioral modifiers"


        #region "return type modifiers"

        public static bool IsVoidReturnType(this MethodDeclarationSyntax methodDeclaration)
        {
            var predefinedTypeSyntax = methodDeclaration.ReturnType as PredefinedTypeSyntax;
            return predefinedTypeSyntax != null && predefinedTypeSyntax.Keyword.IsKind(SyntaxKind.VoidKeyword);
        }

        public static MethodDeclarationSyntax AsReturnVoid(this MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return methodDeclarationSyntax.WithReturnType(SyntaxBuilder.ReturnVoid);
        }

        public static MethodDeclarationSyntax AsReturnTask(this MethodDeclarationSyntax methodDeclarationSyntax)
        {
            var newMethodReturnType = methodDeclarationSyntax.IsVoidReturnType() 
                ? SyntaxFactory.ParseTypeName("Task") 
                : SyntaxFactory.GenericName("Task").AddTypeArgumentListArguments(methodDeclarationSyntax.ReturnType.WithoutTrivia());
            return methodDeclarationSyntax.WithReturnType(newMethodReturnType);
        }

        #endregion //#region "return type modifiers"
    }
}