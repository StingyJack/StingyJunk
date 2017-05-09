namespace StingyJunk.Compilation.Misc
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class TypeExtensions
    {
        public static SyntaxTokenList AllAccModifiers(this ClassDeclarationSyntax classDeclarationSyntax)
        {
            return classDeclarationSyntax.Modifiers.Where(m => m.IsKind(SyntaxKind.PublicKeyword)
                                                               || m.IsKind(SyntaxKind.ProtectedKeyword)
                                                               || m.IsKind(SyntaxKind.InternalKeyword)
                                                               || m.IsKind(SyntaxKind.PrivateKeyword))
                .ToList();
        }

        public static SyntaxToken? FirstAccModifier(this ClassDeclarationSyntax classDeclarationSyntax)
        {
            return classDeclarationSyntax.Modifiers.FirstOrDefault(m => m.IsKind(SyntaxKind.PublicKeyword)
                                                                        || m.IsKind(SyntaxKind.ProtectedKeyword)
                                                                        || m.IsKind(SyntaxKind.InternalKeyword)
                                                                        || m.IsKind(SyntaxKind.PrivateKeyword));
        }

        public static ClassDeclarationSyntax AsPublic(this ClassDeclarationSyntax classDeclarationSyntax)
        {
            classDeclarationSyntax = StripAccessModifiers(classDeclarationSyntax);

            return classDeclarationSyntax.WithModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword).ToList());
        }

        public static ClassDeclarationSyntax StripAccessModifiers(ClassDeclarationSyntax classDeclarationSyntax)
        {
            var modifier = classDeclarationSyntax.FirstAccModifier();
            while (modifier.HasValue)
            {
                classDeclarationSyntax = classDeclarationSyntax.RemoveNode(modifier.Value.Parent, SyntaxRemoveOptions.KeepNoTrivia);
                modifier = classDeclarationSyntax.FirstAccModifier();
            }
            return classDeclarationSyntax;
        }


        public static SyntaxTokenList AllAccModifiers(this InterfaceDeclarationSyntax interfaceDeclarationSyntax)
        {
            return interfaceDeclarationSyntax.Modifiers.Where(m => m.IsKind(SyntaxKind.PublicKeyword)
                                                                   || m.IsKind(SyntaxKind.ProtectedKeyword)
                                                                   || m.IsKind(SyntaxKind.InternalKeyword)
                                                                   || m.IsKind(SyntaxKind.PrivateKeyword))
                .ToList();
        }

        public static SyntaxToken? FirstAccModifier(this InterfaceDeclarationSyntax interfaceDeclarationSyntax)
        {
            return interfaceDeclarationSyntax.Modifiers.FirstOrDefault(m => m.IsKind(SyntaxKind.PublicKeyword)
                                                                            || m.IsKind(SyntaxKind.ProtectedKeyword)
                                                                            || m.IsKind(SyntaxKind.InternalKeyword)
                                                                            || m.IsKind(SyntaxKind.PrivateKeyword));
        }

        public static InterfaceDeclarationSyntax AsPublic(this InterfaceDeclarationSyntax interfaceDeclarationSyntax)
        {
            interfaceDeclarationSyntax = StripAccessModifiers(interfaceDeclarationSyntax);

            return interfaceDeclarationSyntax.WithModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword).ToList());
        }

        public static InterfaceDeclarationSyntax StripAccessModifiers(InterfaceDeclarationSyntax interfaceDeclarationSyntax)
        {
            var modifier = interfaceDeclarationSyntax.FirstAccModifier();
            while (modifier.HasValue)
            {
                interfaceDeclarationSyntax = interfaceDeclarationSyntax.RemoveNode(modifier.Value.Parent, SyntaxRemoveOptions.KeepNoTrivia);
                modifier = interfaceDeclarationSyntax.FirstAccModifier();
            }
            return interfaceDeclarationSyntax;
        }


        public static ClassDeclarationSyntax AsSerializable(this ClassDeclarationSyntax classDeclarationSyntax)
        {
            return classDeclarationSyntax.AddAttributeLists(Attributes.Serializable);
        }
    }
}