namespace StingyJunk.ProjectDiffer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using CommandLine;
    using FileHandling;
    using Microsoft.Build.Evaluation;
    using Microsoft.CodeAnalysis;

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
            var projectsOnly = fileCollection.All<ProjectFile>();
            var solutionProjects = projectsOnly.Where(p => p.IsPartOfASolution).ToImmutableList();
            var looseProjects = projectsOnly.Where(p => p.IsPartOfASolution == false).ToImmutableList();
            var unknownFiles = fileCollection.All<CandidateFile>();

            if (unknownFiles.IsEmpty == false)
            {
                unknownFiles.ForEach(f => Owl($"Unknown File: {f.FilePath}"));
            }

            Owl($"Found {solutions.Count} solutions with {solutionProjects.Count} total projects, and {looseProjects.Count} loose projects");
            if (fileInfos.Count != projectsOnly.Count)
            {
                //mismatch ask to continue or not
            }
            var collection = new ProjectCollection();
            //collection.DefaultToolsVersion = "4.0";
            //var project = new Project(collection);
            foreach (var project in projectsOnly)
            {

                //interrogate project
                var pj = collection.LoadProject(project.FilePath);
                //get configurations
                //get target frameworks
                var props = pj.Properties.Where(p => (p.Name.IndexOf("config", StringComparison.OrdinalIgnoreCase) >= 0) || (p.Name.IndexOf("target", StringComparison.OrdinalIgnoreCase) >= 0));
                var projectDefaultTargetFramework = pj.Properties.FirstOrDefault(p => p.Name == "TargetFrameworkVersion");

                Owl(string.Join(",", props.Select(p => $"prop: {p.Name}")));
                foreach (var pc in pj.Xml.PropertyGroups)
                {
                    Owl($"{pc.Condition}");
                }

                //"v4.6.1"
                var shortName = GetTargetFrameworkShortname(projectDefaultTargetFramework?.EvaluatedValue);
                foreach (var wantedTarget in args.CompareTargetFrameworks)
                {
                    if (string.Equals(shortName, wantedTarget, StringComparison.OrdinalIgnoreCase))
                    {
                        //at least one
                    }
                }




                //apply changes (does this need a filewatcher to prevent unintended corruption?

            }

        }

        private static string GetTargetFrameworkShortname(string evaluatedValue)
        {
            
        }
    }
}