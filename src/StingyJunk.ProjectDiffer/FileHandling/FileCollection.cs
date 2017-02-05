namespace StingyJunk.ProjectDiffer.FileHandling
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;

    public class FileCollection
    {
        private readonly OicDic<BaseFile> _allFiles = new OicDic<BaseFile>();

        private readonly OicList _allKeys = new OicList();

        public void Add(BaseFile baseFile, ReplaceAction replaceAction = ReplaceAction.Unspecified)
        {
            var dictKey = baseFile.FilePath;
            if (_allFiles.Has(dictKey))
            {
                if (replaceAction == ReplaceAction.Replace)
                {
                    _allFiles[dictKey] = baseFile;
                    return ;
                }
                if (replaceAction == ReplaceAction.Error)
                {
                    throw new InvalidOperationException($"File {dictKey} already registered in the collection");
                }
                return;
            }

            _allFiles.Add(dictKey, baseFile);
        }

        public ImmutableList<T> All<T>()
        {
            return _allFiles.Values.OfType<T>().ToImmutableList();
        }

        public ImmutableList<T> Any<T>(string filePathMatch)
        {
            return _allFiles.Values.Where(f => f.FilePath.Equals(filePathMatch, StringComparison.OrdinalIgnoreCase)).OfType<T>().ToImmutableList();
        }

    }
}