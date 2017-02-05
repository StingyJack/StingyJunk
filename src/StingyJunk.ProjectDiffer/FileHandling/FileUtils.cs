namespace StingyJunk.ProjectDiffer.FileHandling
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;

    public static class FileUtils
    {
        public static FileCollection GetFiles(ImmutableList<FileInfo> fileInfos)
        {
            var returnValue = new FileCollection();

            var candidateFiles = new List<CandidateFile>();

            foreach (var fileInfo in fileInfos)
            {
                var pf = GetFileType(fileInfo);
                candidateFiles.Add(pf);
            }

            foreach (var solutionCandidate in candidateFiles.Where(s => s.FileType == FileType.Solution))
            {
                var solutionFile = new SolutionFile {FilePath = solutionCandidate.FilePath};
                var solutionProjects = GetSolutionProjects(solutionFile);

                foreach (var solutionProject in solutionProjects)
                {
                    solutionFile.AddProject(solutionProject);
                    returnValue.Add(solutionProject);
                }
                returnValue.Add(solutionFile);
            }

            foreach (var projectCandidate in candidateFiles.Where(s => s.FileType == FileType.Project))
            {
                if (returnValue.Any<ProjectFile>(projectCandidate.FilePath).IsEmpty)
                {
                    var pf = new ProjectFile {FilePath = projectCandidate.FilePath};
                    returnValue.Add(pf);
                }
            }

            foreach (var unknownFile in candidateFiles.Where(s => s.FileType == FileType.Unknown))
            {
                returnValue.Add(unknownFile, ReplaceAction.Error);
            }

            return returnValue;
        }

        private static ImmutableList<ProjectFile> GetSolutionProjects(SolutionFile solutionFile)
        {
            var solutionProjects = new List<ProjectFile>();

            var extracts = DataExtractionUtils.ExtractFromFile(solutionFile.FilePath, "Project(", "EndProject", StringComparison.OrdinalIgnoreCase,
                "Global");
            // Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "AnotherClassLibrary", "AnotherClassLibrary\AnotherClassLibrary.csproj", "{95238CB6-E50C-446E-93A6-48FFA6B8E431}"\r\nEndProject\r\n
            var solutionFolder = Path.GetDirectoryName(solutionFile.FilePath);

            if (Directory.Exists(solutionFolder) == false)
            {
                throw new InvalidOperationException($"directory for {solutionFile.FilePath} cant be found");
            }

            foreach (var extract in extracts)
            {
                var elements = extract.Split(',');
                var unrootedProjFile = elements[1].Replace("\"", string.Empty).Trim();
                var projFilePath = Path.Combine(solutionFolder, unrootedProjFile);
                if (File.Exists(projFilePath))
                {
                    solutionProjects.Add(new ProjectFile
                        {
                            FilePath = projFilePath,
                            SolutionPath = solutionFile.FilePath
                        }
                    );
                }
            }

            return solutionProjects.ToImmutableList();
        }

        public static CandidateFile GetFileType(FileInfo fileInfo)
        {
            var cf = new CandidateFile
            {
                FilePath = fileInfo.FullName,
                FileType = FileType.Unknown
            };

            using (var sr = fileInfo.OpenText())
            {
                while (sr.EndOfStream == false)
                {
                    var line = sr.ReadLine()?.Trim();
                    if (line == null)
                    {
                        throw new ArgumentNullException(nameof(line), "No idea why this is even permitted.");
                    }

                    if (line.StartsWith("<Project ", StringComparison.OrdinalIgnoreCase))
                    {
                        cf.FileType = FileType.Project;
                        break;
                    }
                    if (line.StartsWith("Microsoft Visual Studio Solution File", StringComparison.OrdinalIgnoreCase))
                    {
                        cf.FileType = FileType.Solution;
                        break;
                    }
                }
            }

            return cf;
        }
    }
}