namespace StingyJunk.Compilation.Bags
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;

    public class GenResult
    {
        private readonly List<string> _debugLog = new List<string>();
        private readonly List<string> _infoLog = new List<string>();
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();
        private readonly Dictionary<string, string> _generatedOutput = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public ReadOnlyCollection<string> DebugMessages => new ReadOnlyCollection<string>(_debugLog);
        public ReadOnlyCollection<string> InfoMessages => new ReadOnlyCollection<string>(_infoLog);
        public ReadOnlyCollection<Diagnostic> Diagnostics => new ReadOnlyCollection<Diagnostic>(_diagnostics);
        public ReadOnlyDictionary<string, string> GeneratedOutput => new ReadOnlyDictionary<string, string>(_generatedOutput);

        public void AddGeneratedCode(string outputKey, string outputCode)
        {
            _generatedOutput.Add(outputKey, outputCode);
        }

        public void AddDiags(IEnumerable<Diagnostic> diags)
        {
            _diagnostics.AddRange(diags);
        }

        public void Debug(string message)
        {
            _debugLog.Add($"DEBUG - {message}");
            _infoLog.Add($"INFO -{message}");
        }

        public void Info(string message)
        {
            _infoLog.Add($"INFO -{message}");
        }

        public string GeneratedOutputAsSingle()
        {
            var usings = new List<string>();
            var nonUsings = new StringBuilder();

            foreach (var item in _generatedOutput)
            {
                foreach (var line in item.Value.Split(new[] {Environment.NewLine}, StringSplitOptions.None))
                {
                    if (line.StartsWith("using "))
                    {
                        var usingPart = line.Replace("using ", string.Empty).Replace(";", string.Empty).Trim();
                        if (usings.Any(u => u.Equals(usingPart, StringComparison.OrdinalIgnoreCase)))
                        {
                            continue;
                        }
                        usings.Add(usingPart);
                    }
                    else
                    {
                        nonUsings.AppendLine(line);
                    }
                }
            }

            var usingStatements = string.Join(Environment.NewLine, usings.Select(u => $"using {u};"));
            return $"{usingStatements}{Environment.NewLine}{Environment.NewLine}{nonUsings}";
        }
    }
}