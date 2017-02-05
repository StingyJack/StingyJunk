namespace StingyJunk.ProjectDiffer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using CommandLine;
    using FileHandling;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(".NET Project File Differ ");
            Parser.Default.ParseArguments<CheckOptions>(args)
                .WithParsed(options =>
                {
                    HandleCheckOptions(options);
                })
                .WithNotParsed(errors =>
                {
                    HandleErrors(errors);
                }); // errors is a sequence of type IEnumerable<Error>
            Console.ReadLine();
        }

        private static void Owl(string message, bool isErr = false)
        {
            if (isErr)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        private static void HandleCheckOptions(CheckOptions options)
        {
            var files = VerifyFilesExist(options);
            ReportProjectDiffs(options, files);
        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {
            foreach (var err in errors)
            {
                Owl($"Error:{err.Tag} : StopsProcessing: {err.StopsProcessing}", true);
            }
        }

        private static ImmutableList<FileInfo> VerifyFilesExist(CheckOptions cmdArgs)
        {
            var returnValue = new List<FileInfo>();
            foreach (var fileName in cmdArgs.FilesToCheck)
            {
                if (File.Exists(fileName) == false)
                {
                    throw new FileNotFoundException("pls giff real proj", fileName);
                }
                returnValue.Add(new FileInfo(fileName));
            }
            return returnValue.ToImmutableList();
        }


        public static void ReportProjectDiffs(CheckOptions args, ImmutableList<FileInfo> fileInfos)
        {
            //is file for a solution or project
            var fileCollection = FileUtils.GetFiles(fileInfos);

            var solutions = fileCollection.All<SolutionFile>();
            var solutionProjects = fileCollection.All<ProjectFile>().Where(p => p.IsPartOfASolution).ToImmutableList();
            var looseProjects = fileCollection.All<ProjectFile>().Where(p => p.IsPartOfASolution == false).ToImmutableList();
            var unknownFiles = fileCollection.All<CandidateFile>();

            if (unknownFiles.IsEmpty == false)
            {
                unknownFiles.ForEach(f => Owl($"Unknown File: {f.FilePath}"));
            }

            Owl($"Found {solutions.Count} solutions with {solutionProjects.Count} total projects, and {looseProjects.Count} loose projects");

        }
    }
}