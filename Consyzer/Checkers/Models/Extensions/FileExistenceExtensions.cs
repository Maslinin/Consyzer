namespace Consyzer.Checkers.Models.Extensions;

internal static class FileExistenceExtensions
{
    public static IDictionary<string, FileExistenceStatus> GetMinFileExistenceStatuses(this IFileExistenceChecker fileChecker, IEnumerable<string> filePaths)
    {
        return filePaths.ToDictionary(k => k, v => fileChecker.GetMinFileExistanceStatus(v));
    }

    public static int CountExistingFiles(this IEnumerable<FileExistenceInfo> fileExistenceInfos)
    {
        return fileExistenceInfos.Count(x => x.ExistenceStatus != FileExistenceStatus.FileDoesNotExist);
    }

    public static int CountNonExistingFiles(this IEnumerable<FileExistenceInfo> fileExistenceInfos)
    {
        return fileExistenceInfos.Count(x => x.ExistenceStatus == FileExistenceStatus.FileDoesNotExist);
    }
}