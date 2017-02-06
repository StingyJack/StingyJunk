namespace StingyJunk.ProjectDiffer.FileHandling
{
    using System;

    public class ProjectFile :BaseFile
    {
        public bool IsPartOfASolution
        {
            get { return string.IsNullOrWhiteSpace(SolutionPath) == false; }
        }

        public string SolutionPath { get; set; }
        
    }
}