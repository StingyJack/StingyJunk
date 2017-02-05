namespace StingyJunk.ProjectDiffer.FileHandling
{
    using System.Collections.Immutable;

    public class SolutionFile : BaseFile
    {
        private readonly OicDic<ProjectFile> _projectFiles = new OicDic<ProjectFile>();

        public void AddProject(ProjectFile projectFile, bool replaceExisting = false)
        {
            var key = projectFile.FilePath;

            if (_projectFiles.Has(key))
            {
                if (replaceExisting == true)
                {
                    _projectFiles[key] = projectFile;
                    return;
                }
                return;
            }
            _projectFiles.Add(key, projectFile);
        }

        public ImmutableList<ProjectFile> ChildProjectFiles()
        {
            return _projectFiles.Values.ToImmutableList();
        }
    }
}