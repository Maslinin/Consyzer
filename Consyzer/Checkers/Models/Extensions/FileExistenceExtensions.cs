namespace Consyzer.Checkers.Models.Extensions;

internal static class FileExistenceExtensions
{
    public static IEnumerable<FileExistenceStatus> GetMinFileExistenceStatuses(this IFileExistenceChecker fileChecker, IEnumerable<string> filePaths)
    {
        return filePaths.Select(fileChecker.GetMinFileExistanceStatus);
    }

    public static int CountExistingFiles(this IEnumerable<FileExistenceStatus> fileExistenceStatuses)
    {
        return fileExistenceStatuses.Count(x => x != FileExistenceStatus.FileDoesNotExist);
    }

    public static int CountNonExistingFiles(this IEnumerable<FileExistenceStatus> fileExistenceStatuses)
    {
        return fileExistenceStatuses.Count(x => x == FileExistenceStatus.FileDoesNotExist);
    }
}