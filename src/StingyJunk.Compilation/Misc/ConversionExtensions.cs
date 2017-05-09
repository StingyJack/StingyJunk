namespace StingyJunk.Compilation.Misc
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using Microsoft.CodeAnalysis;

    public static class ConversionExtensions
    {
        /// <summary>
        ///     Converts the assemblies to metadata references.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public static IEnumerable<MetadataReference> ToMetadataReferences(this IEnumerable<Assembly> assemblies)
        {
            var result = new List<MetadataReference>();

            foreach (var assembly in assemblies)
            {
                result.Add(MetadataReference.CreateFromFile(assembly.Location));
            }

            return result;
        }

        public static string ToSingleString(this IReadOnlyDictionary<string, string> rod)
        {
            var sb = new StringBuilder();

            foreach (var item in rod)
            {
                sb.AppendLine($"//ITEM: {item.Key}");
                sb.AppendLine($"//VALUE:{Environment.NewLine}{item.Value}");
            }

            return sb.ToString();
        }

        public static SyntaxTokenList ToList(this SyntaxToken token)
        {
            var stl = new SyntaxTokenList();
            stl = stl.Add(token);
            return stl;
        }

        public static SyntaxTokenList ToList(this IEnumerable<SyntaxToken> enumerable)
        {
            var stl = new SyntaxTokenList();
            stl = stl.AddRange(enumerable);
            return stl;
        }
    }
}