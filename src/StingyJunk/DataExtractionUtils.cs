namespace StingyJunk
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Text;

    public static class DataExtractionUtils
    {
        /// <summary>
        /// Extracts from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="startingText">The starting text.</param>
        /// <param name="endingText">The ending text.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <param name="stopAtText">The stop at text.</param>
        /// <returns></returns>
        public static ImmutableList<string> ExtractFromFile(string filePath,
            string startingText, string endingText, StringComparison stringComparison, string stopAtText = null)
        {
            var extracts = new List<string>();

            using (var sr = File.OpenText(filePath))
            {
                var extractInProgress = false;
                var sb = new StringBuilder();
                while (sr.EndOfStream == false)
                {
                    var line = sr.ReadLine();
                    if (line == null)
                    {
                        continue;
                    }

                    line = line.Trim();

                    if (stopAtText != null && line.StartsWith(stopAtText, stringComparison))
                    {
                        if (extractInProgress)
                        {
                            extracts.Add(sb.ToString());
                            extractInProgress = false;
                        }
                        break;
                    }

                    if (line.StartsWith(startingText, stringComparison))
                    {
                        extractInProgress = true;
                        sb.AppendLine(line);
                        continue;
                    }

                    if (extractInProgress == true)
                    {
                        sb.AppendLine(line);
                        if (line.EndsWith(endingText, stringComparison))
                        {
                            extracts.Add(sb.ToString());
                            sb.Length = 0;
                            extractInProgress = false;
                        }
                    }
                } //while sr.Read

                if (extractInProgress)
                {
                    extracts.Add(sb.ToString());
                }
            } //using sr
            return extracts.ToImmutableList();
        }
    }
}