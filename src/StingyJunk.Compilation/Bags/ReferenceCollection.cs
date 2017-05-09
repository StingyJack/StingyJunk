namespace StingyJunk.Compilation.Bags
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Misc;

    /// <summary>
    ///     Holds a collection of references
    /// </summary>
    public class ReferenceCollection
    {
        private readonly Dictionary<string, Assembly> _internalDictionary = new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

        public void Add(params Assembly[] assemblies)
        {
            if (assemblies == null) { return; }
            foreach (var assembly in assemblies)
            {
                if (_internalDictionary.ContainsKey(assembly.FullName))
                {
                    return;
                }
                _internalDictionary.Add(assembly.FullName, assembly);
            }
        }

        public void Add(params Type[] containedTypes)
        {
            if (containedTypes == null) { return; }
            foreach (var ct in containedTypes)
            {
                var asm = ct.Assembly;
                Add(asm);
            }
        }

        public void Add(IEnumerable<Assembly> list)
        {
            if (list == null) { return; }
            foreach (var asm in list)
            {
                Add(asm);
            }
        }

        public void Add(IEnumerable<MetadataReference> metadataReferences)
        {
            if (metadataReferences == null) { return; }
            foreach (var mr in metadataReferences)
            {
                var properties = mr.GetType().GetProperties();
                var filePathProp = properties.FirstOrDefault(p => p.Name.Equals("FilePath", StringComparison.OrdinalIgnoreCase));
                if (filePathProp == null)
                {
                    continue;
                }
                var rawValue = filePathProp.GetValue(mr);
                if (rawValue == null)
                {
                    continue;
                }
                var filePath = rawValue.ToString();
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    continue;
                }
                try
                {
                    var fullPath = Path.GetFullPath(filePath);
                    var asm = Assembly.ReflectionOnlyLoadFrom(fullPath);
                    Add(asm);
                }
                catch (FileLoadException flex)
                {
                    if (flex.Message.IndexOf("already loaded", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        //this is fine
                        continue;
                    }
                    Trace.TraceError($"Failed to load ref from {filePath}. {flex}");
                }
                catch (Exception e)
                {
                    Trace.TraceError($"Failed to load metadata ref from {filePath}. {e}");
                }
            }
        }

        /// <summary>
        ///     The refernce collection as assemblies
        /// </summary>
        /// <returns></returns>
        public List<Assembly> AsAssemblies()
        {
            return new List<Assembly>(_internalDictionary.Values);
        }

        /// <summary>
        ///     The reference collection as metadata references
        /// </summary>
        /// <returns></returns>
        public List<MetadataReference> AsMetadataReferences()
        {
            return new List<MetadataReference>(AsAssemblies().ToMetadataReferences());
        }
    }
}