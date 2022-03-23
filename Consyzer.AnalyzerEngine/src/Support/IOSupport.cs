using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.CommonModels.FileInfoModels;

namespace Consyzer.AnalyzerEngine.Support
{
    public static class IOSupport
    {
        public static IEnumerable<BinaryFileInfo> GetBinaryFilesInfo(string pathToBinaryFiles, string[] filesExtensions)
        {
            if (!Directory.Exists(pathToBinaryFiles))
            {
                throw new DirectoryNotFoundException("The directory specified for the analysis does not exist");
            }
            if (filesExtensions is null)
            {
                throw new ArgumentNullException($"The \"{nameof(filesExtensions)}\" argument: there is no reference to the object");
            }

            try
            {
                var dirInfo = new DirectoryInfo(pathToBinaryFiles).EnumerateFiles().Where(f => filesExtensions.Contains(f.Extension));

                return dirInfo.Select(i => new BinaryFileInfo(i.FullName));
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException($"You do not have read permissions in the specified folder: {pathToBinaryFiles}", e);
            }
        }

        public static bool IsAbsolutePath(string path)
        {
            return path.Contains(Path.DirectorySeparatorChar) || path.Contains(Path.AltDirectorySeparatorChar);
        }
    }
}
