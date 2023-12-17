namespace Consyzer.Checkers.Models;

internal sealed class FileExistenceInfo
{
    public string FilePath { get; set; }
    public FileExistenceStatus ExistenceStatus { get; set; }
}