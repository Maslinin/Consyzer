using System.Linq;
using System.Collections.Generic;

namespace Consyzer.Checkers.Models.Extensions;

internal static class FileExistenceExtensions
{
    public static IEnumerable<FileExistenceInfo> ToFileExistenceInfos(this IEnumerable<FileExistenceStatus> fileExistenceStatuses, IEnumerable<string> filePaths)
    {
        return fileExistenceStatuses.Zip(filePaths, (fileExistence, filePath) => new FileExistenceInfo
        {
            ExistenceStatus = fileExistence,
            FilePath = filePath
        });
    }

    public static IEnumerable<FileExistenceStatus> ToMinFileExistenceStatuses(this IFileExistenceChecker fileChecker, IEnumerable<string> filePaths)
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