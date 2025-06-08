namespace Consyzer.Helpers;

internal static class FileSearchHelper
{
    public static IEnumerable<FileInfo> GetFilesByCommaSeparatedPatterns(string directory, string searchPatterns)
    {
        return searchPatterns
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .SelectMany(p => GetFilesByPattern(directory, p));
    }

    public static IEnumerable<FileInfo> GetFilesByPattern(string directory, string searchPattern)
    {
        return Directory.EnumerateFiles(directory, searchPattern.Trim())
            .Select(f => new FileInfo(f));
    }
}