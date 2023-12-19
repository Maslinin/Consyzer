namespace Consyzer.Checkers;

internal interface IFileMetadataChecker
{
    bool ContainsOnlyMetadata(IEnumerable<FileInfo> files);
    bool ContainsOnlyMetadataAssemblies(IEnumerable<FileInfo> files);
}