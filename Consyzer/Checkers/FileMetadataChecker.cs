using Microsoft.Extensions.Logging;
using Consyzer.Filters;
using Consyzer.Logging;

namespace Consyzer.Checkers;

internal sealed class FileMetadataChecker : IFileMetadataChecker
{
    private readonly ILogger _logger;
    private readonly IMetadataFileFilter _fileFilter;

    public FileMetadataChecker(ILogger<FileMetadataChecker> logger, IMetadataFileFilter fileFilter)
    {
        this._logger = logger;
        this._fileFilter = fileFilter;
    }

    public bool ContainsOnlyMetadata(IEnumerable<FileInfo> fileInfos)
    {
        var nonMetadataFiles = this._fileFilter.GetNonMetadataFiles(fileInfos);

        if (nonMetadataFiles.Any())
        {
            this._logger.LogInformation("The following fileInfos have been excluded from the analysis because they DO NOT contain metadata:{newLine}{baseFileInfo}",
                Environment.NewLine, FileMetadataLogMessageFormatter.GetBaseFileInfoLog(nonMetadataFiles));
        }

        if (nonMetadataFiles.Count() == fileInfos.Count())
        {
            this._logger.LogInformation("All found fileInfos DO NOT contain metadata.");
            return false;
        }

        this._logger.LogInformation("All found fileInfos contain metadata.");

        return true;
    }

    public bool ContainsOnlyMetadataAssemblies(IEnumerable<FileInfo> fileInfos)
    {
        var nonMetadataAssemblyFiles = this._fileFilter.GetNonMetadataAssemblyFiles(fileInfos);

        if (nonMetadataAssemblyFiles.Any())
        {
            this._logger.LogInformation("The following fileInfos have been excluded from the analysis because they ARE NOT metadata assembly fileInfos:{newLine}{baseFileInfo}",
                Environment.NewLine, FileMetadataLogMessageFormatter.GetBaseFileInfoLog(nonMetadataAssemblyFiles));
        }

        if (nonMetadataAssemblyFiles.Count() == fileInfos.Count())
        {
            this._logger.LogInformation("All found fileInfos contain metadata, but ARE NOT assembly fileInfos.");
            return false;
        }

        this._logger.LogInformation("All metadata fileInfos found are assemblies.");

        return true;
    }
}