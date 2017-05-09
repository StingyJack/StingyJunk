namespace StingyJunk.Compilation.Misc
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    ///     A collection of filesystem utils that are either Soft where exceptions written to trace and 
    /// not rethown or standard where exceptions are wrtitten to trace and rethrown 
    /// </summary>
    public static class FileUtilities
    {
        /// <summary>
        ///     Removes the file if present. 
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static void RemoveIfPresentSoft(string fileName)
        {
            RemoveIfPresent(fileName, true);
        }

        /// <summary>
        ///     Removes the file if present. 
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static void RemoveIfPresent(string fileName)
        {
            RemoveIfPresent(fileName, false);
        }

        private static void RemoveIfPresent(string fileName, bool soft)
        {
            if (File.Exists(fileName) == false)
            {
                return;
            }
            try
            {
                File.Delete(fileName);
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failed to remove file '{fileName}', {e}");
                if (soft == false)
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///     Gets the content of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetFileContentSoft(string fileName)
        {
            return GetFileContent(fileName, true);
        }

        /// <summary>
        ///     Gets the content of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetFileContent(string fileName)
        {
            return GetFileContent(fileName, false);
        }

        private static string GetFileContent(string fileName, bool soft)
        {
            try
            {
                return File.ReadAllText(fileName);
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failed to get content for file '{fileName}', {e}");
                if (soft == false)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        /// <summary>
        ///     Writes the assembly.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static bool WriteFile(string targetPath, byte[] bytes)
        {
            return WriteFile(targetPath, bytes, false);
        }

        /// <summary>
        ///     Writes the assembly.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static bool WriteFileSoft(string targetPath, byte[] bytes)
        {
            return WriteFile(targetPath, bytes, true);
        }

        /// <summary>
        /// Writes the assembly.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="soft">if set to <c>true</c> [soft].</param>
        /// <returns></returns>
        public static bool WriteFile(string targetPath, byte[] bytes, bool soft)
        {
            try
            {
                File.WriteAllBytes(targetPath, bytes);
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failed to write assembly for '{targetPath}', {e}");
                if (soft == false)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        ///     Gets the directory for the file. 
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">thrown if path cant be found</exception>
        public static string GetDirectory(string filePath)
        {
            var path = Path.GetDirectoryName(filePath);
            if (path == null)
            {
                throw new DirectoryNotFoundException($"Cant resolve directory for '{filePath}'");
            }
            return path;
        }


        public static string BuildFullPath(string part1, params string[] parts)
        {
            var builtPath = part1;

            foreach (var part in parts)
            {
                builtPath = Path.Combine(builtPath, part);
            }

            return Path.GetFullPath(builtPath);
        }
    }
}