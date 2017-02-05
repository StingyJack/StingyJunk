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

        public void Load()
        {
         
        }

        //get configurations
        //get target frameworks
        //apply changes (does this need a filewatcher to prevent unintended corruption?
    }
}