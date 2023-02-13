using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Consyzer.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class IOHelper
    {
        public static IEnumerable<FileInfo> GetFilesFrom(string pathToFiles, IEnumerable<string> fileExtensions)
        {
            var dirInfo = new DirectoryInfo(pathToFiles);
            return dirInfo.GetFiles().Where(f => fileExtensions.Contains(f.Extension));
        }

        public static string AddExtensionToFile(string filePath, string fileExtension)
        {
            return Path.HasExtension(filePath) ? filePath : Path.ChangeExtension(filePath, fileExtension);
        }

    }
}
