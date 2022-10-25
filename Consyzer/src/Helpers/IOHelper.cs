using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Consyzer.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class IOHelper
    {
        public static IEnumerable<FileInfo> GetBinaryFilesInfoFrom(string pathToFiles, IEnumerable<string> binaryExtensions = null)
        {
            if (!Directory.Exists(pathToFiles))
            {
                throw new DirectoryNotFoundException("The directory specified for the analysis does not exist.");
            }
            binaryExtensions ??= new List<string> { ".exe", ".dll" };

            try
            {
                var dirInfo = new DirectoryInfo(pathToFiles);
                var fileInfos = dirInfo.GetFiles().Where(f => binaryExtensions.Contains(f.Extension));

                return fileInfos;
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException($"You do not have read permissions in the specified folder: {pathToFiles}.", e);
            }
        }
    }
}
