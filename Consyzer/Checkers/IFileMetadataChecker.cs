namespace Consyzer.Checkers;

internal interface IFileMetadataChecker
{
    bool ContainsOnlyMetadata(IEnumerable<FileInfo> fileInfos);
    bool ContainsOnlyMetadataAssemblies(IEnumerable<FileInfo> fileInfos);
}