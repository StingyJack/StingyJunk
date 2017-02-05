namespace StingyJunk.ProjectDiffer
{
    using System.Collections.Generic;
    using CommandLine;

    [Verb("check", HelpText="Checks the files and reports differences")]
    public class CheckOptions
    {
       
        [Value(0, Min=1,Max=256, Required = false, HelpText = "The proj (or sln) files to check" )]
        public  IEnumerable<string> FilesToCheck { get; set; }
       
        [Option('b', "targets", HelpText = "Check for these targeted platforms", Required = false, Separator = ',')]
        public IEnumerable<string> CompareTargetPlatforms { get; set; }

        [Option('f', "frameworks", HelpText = "Check for these targeted frameworks", Required = false, Separator = ',')]
        public IEnumerable<string> CompareTargetFrameworks { get; set; }

        [Option('i', "ignoreExtraEntries", SetName = "hardMode", HelpText = "Ignores any entries other than those requested. The default is true", Default= true, Required = false)]
        public bool IngoreExtraEntries { get; set; }

        [Option('h', "reportExtraEntries",SetName="hardMode",  HelpText = "Ignores any entries other than those requested. The default is false", Default = false, Required = false)]
        public bool ReportExtraEntries { get; set; }

    }
}