using Microsoft.Extensions.Logging;
using Consyzer.Filters;
using Consyzer.Helpers;

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

    public bool ContainsOnlyMetadata(IEnumerable<FileInfo> files)
    {
        var nonMetadataFiles = this._fileFilter.GetNonMetadataFiles(files);

        if (nonMetadataFiles.Any())
        {
            this._logger.LogInformation("The following files were excluded from analysis because they DO NOT contain metadata:{newLine}{baseFileInfo}",
                Environment.NewLine, LogMessageBuilderHelper.BuildBaseFileInfoLog(nonMetadataFiles));
        }

        if (nonMetadataFiles.Count() == files.Count())
        {
            this._logger.LogInformation("All found files DO NOT contain metadata.");
            return false;
        }

        this._logger.LogInformation("All found files contain metadata.");

        return true;
    }

    public bool ContainsOnlyMetadataAssemblies(IEnumerable<FileInfo> files)
    {
        var nonMetadataAssemblyFiles = this._fileFilter.GetNonMetadataAssemblyFiles(files);

        if (nonMetadataAssemblyFiles.Any())
        {
            this._logger.LogInformation("The following files were excluded from analysis because they ARE NOT assembly files:{newLine}{baseFileInfo}",
                Environment.NewLine, LogMessageBuilderHelper.BuildBaseFileInfoLog(nonMetadataAssemblyFiles));
        }

        if (nonMetadataAssemblyFiles.Count() == files.Count())
        {
            this._logger.LogInformation("All found files contain metadata, but ARE NOT assembly files.");
            return false;
        }

        this._logger.LogInformation("All metadata files found are assemblies.");

        return true;
    }
}