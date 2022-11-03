namespace Consyzer.Searchers
{
    internal enum FileExistanceStatusCode
    {
        FileExistsAtAnalysisPath,
        FileExistsAtAbsolutePath,
        FileExistsAtRelativePath,
        FileExistsAtSystemFolder,
        FileDoesNotExists
    }
}
