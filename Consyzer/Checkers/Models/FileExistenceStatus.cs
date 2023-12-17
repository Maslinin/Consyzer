namespace Consyzer.Checkers.Models;

internal enum FileExistenceStatus
{
    FileExistsAtAnalysisPath,
    FileExistsAtAbsolutePath,
    FileExistsAtRelativePath,
    FileExistsAtSystemDirectory,
    FileDoesNotExist
}