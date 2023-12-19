namespace Consyzer.Helpers;

internal static class FileSearchHelper
{
    public static IEnumerable<FileInfo> GetFilesByCommaSeparatedSearchPatterns(string directory, string searchPatterns)
    {
        return searchPatterns
            .Split(',')
            .SelectMany(p => GetFilesBySearchPattern(directory, p));
    }

    public static IEnumerable<FileInfo> GetFilesBySearchPattern(string directory, string searchPattern)
    {
        return Directory.EnumerateFiles(directory, searchPattern.Trim())
            .Select(f => new FileInfo(f));
    }
}