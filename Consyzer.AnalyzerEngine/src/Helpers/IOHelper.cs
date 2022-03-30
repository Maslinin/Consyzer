using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.AnalyzerEngine.Helpers
{
    public static class IOHelper
    {
        public static IEnumerable<BinaryFileInfo> GetBinaryFilesInfoFrom(string pathToBinaryFiles, IEnumerable<string> binaryExtensions = null)
        {
            if (!Directory.Exists(pathToBinaryFiles))
            {
                throw new DirectoryNotFoundException("The directory specified for the analysis does not exist.");
            }
            if (binaryExtensions is null)
            {
                binaryExtensions = new List<string> { ".exe", ".dll" };
            }

            try
            {
                var dirInfo = new DirectoryInfo(pathToBinaryFiles).EnumerateFiles().Where(f => binaryExtensions.Contains(f.Extension));

                return dirInfo.Select(i => new BinaryFileInfo(i.FullName));
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException($"You do not have read permissions in the specified folder: {pathToBinaryFiles}.", e);
            }
        }

        public static bool IsAbsolutePath(string path)
        {
            return Path.IsPathRooted(path);
        }
    }
}
