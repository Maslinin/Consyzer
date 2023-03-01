namespace Consyzer.File
{
    internal enum FileExistenceStatus
    {
        FileExistsAtAnalysisPath,
        FileExistsAtAbsolutePath,
        FileExistsAtRelativePath,
        FileExistsAtSystemFolder,
        FileDoesNotExist
    }
}
