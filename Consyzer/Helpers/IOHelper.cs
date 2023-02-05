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

        public static string AddExtensionToFile(string filePath, string fileExtension)
        {
            if (!Path.HasExtension(filePath))
            {
                return $"{filePath}{fileExtension}";
            }

            return filePath;
        }

        public static string ToAbsolutePath(string folder, string filePath)
        {
            if (!IsAbsolutePath(filePath))
            {
                return Path.Combine(folder, filePath);
            }

            return filePath;
        }

        public static bool IsAbsolutePath(string filePath)
        {
            return Path.IsPathFullyQualified(filePath);
        }

        public static bool IsRelativePath(string filePath)
        {
            return Path.IsPathRooted(filePath);
        }
    }
}
