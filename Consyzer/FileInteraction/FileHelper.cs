using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Consyzer.FileInteraction
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class FileHelper
    {
        public const string DefaultExtension = ".dll";

        public static IEnumerable<FileInfo> GetFilesFrom(string pathToFiles, IEnumerable<string> fileExtensions)
        {
            return new DirectoryInfo(pathToFiles)
                .GetFiles()
                .Where(f => fileExtensions.Contains(Path.GetExtension(f.FullName)));
        }

        public static string AddExtensionToFile(string filePath, string fileExtension)
        {
            return Path.HasExtension(filePath) ? filePath : Path.ChangeExtension(filePath, fileExtension);
        }

        public static string GetAbsolutePath(string folder, string filePath)
        {
            return IsAbsolutePath(filePath) ? filePath : Path.Combine(folder, filePath);
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
