using System;
using System.Linq;
using System.Text;

namespace StingyJunk.Analyzers.Config
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Threading;
    
    using Microsoft.CodeAnalysis.Diagnostics;

    public class ConfigParser
    {

        public ForbiddenReferences GetForbiddenReferencesFromConfig(AnalyzerOptions options, CancellationToken cancellationToken)
        {
            var returnValue = new ForbiddenReferences();

            if (options == null || options.AdditionalFiles.Length == 0)
            {
                return returnValue;
            }

            var demFiles = options.AdditionalFiles;

            var configFile = demFiles.FirstOrDefault(
                file => Path.GetFileName(file.Path)
                    .Equals("ForbiddenReferences.xml", StringComparison.OrdinalIgnoreCase));

            if (configFile == null)
            {
                return returnValue;
            }

            try
            {
                var sourceText = configFile.GetText(cancellationToken);
                var stream = new MemoryStream();
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    sourceText.Write(writer, cancellationToken);
                }
                stream.Position = 0;

                var dcs = new DataContractSerializer(typeof(ForbiddenReferences));
                var verboten = dcs.ReadObject(stream);
                var obj = verboten as ForbiddenReferences;
                return obj;
            }
            catch (Exception)
            {
                // ignored, maybe toss a null back for that?
            }

            return returnValue;
        }

    }
}
