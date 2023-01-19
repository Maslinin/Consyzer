using System.IO;

namespace Consyzer.Searchers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class PathHelper
    {
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
