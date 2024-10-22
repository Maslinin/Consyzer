using NLog.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Consyzer.Options;
using Consyzer.Filters;
using Consyzer.Logging;
using Consyzer.Helpers;
using Consyzer.Checkers;
using Consyzer.Cryptography;
using Consyzer.Checkers.Models.Extensions;

const int SuccessExitCode = 0;
const int UnexpectedBehaviorExitCode = -1;

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddNLog();
    })
    .AddScoped<IFileHasher, Sha256FileHasher>()
    .AddScoped<IMetadataFileFilter, EcmaMetadataFileFilter>()
    .AddScoped<IFileMetadataChecker, FileMetadataChecker>()
    .AddScoped<IFileExistenceChecker, DllExistenceChecker>()
    .Configure<AnalysisOptions>(configuration)
    .BuildServiceProvider();

var options = serviceProvider.GetRequiredService<IOptions<AnalysisOptions>>().Value;

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
var fileHasher = serviceProvider.GetRequiredService<IFileHasher>();
var fileFilter = serviceProvider.GetRequiredService<IMetadataFileFilter>();
var fileMetadataChecker = serviceProvider.GetRequiredService<IFileMetadataChecker>();
var fileExistenceChecker = serviceProvider.GetRequiredService<IFileExistenceChecker>();

if (!Directory.Exists(options.AnalysisDirectory))
{
    logger.LogError("A non-existent directory was specified for analysis.");
    return UnexpectedBehaviorExitCode;
}

if (string.IsNullOrEmpty(options.SearchPattern))
{
    logger.LogError("An invalid file search pattern was specified for analysis.");
    return UnexpectedBehaviorExitCode;
}

logger.LogInformation("{analysisParams}", OptionsLogMessageFormatter.GetAnalysisOptionsLog(options));

var fileInfos = FileSearchHelper.GetFilesByCommaSeparatedSearchPatterns(options.AnalysisDirectory, options.SearchPattern);
if (!fileInfos.Any())
{
    logger.LogWarning("Files for analysis matching the specified search pattern have not been found.");
    return UnexpectedBehaviorExitCode;
}

logger.LogInformation("Files for analysis matching the specified search pattern have been found:{newLine}{baseFileInfo}",
    Environment.NewLine, FileMetadataLogMessageFormatter.GetBaseFileInfoLog(fileInfos));

if (!fileMetadataChecker.ContainsOnlyMetadata(fileInfos)) return SuccessExitCode;
if (!fileMetadataChecker.ContainsOnlyMetadataAssemblies(fileInfos)) return SuccessExitCode;

var metadataAssemblyFiles = fileFilter.GetMetadataAssemblyFiles(fileInfos);

logger.LogInformation("The following assembly fileInfos containing metadata have been found:{newLine}{fileAndHashInfo}",
    Environment.NewLine, FileMetadataLogMessageFormatter.GetExtendedFileInfoLog(metadataAssemblyFiles, fileHasher));

var importedMethodInfos = metadataAssemblyFiles
    .ToImportedMethodExtractors()
    .ToImportedMethodInfos();

logger.LogInformation("Information about external methods from unmanaged assemblies being analyzed:{newLine}{importedMethodsInfo}",
    Environment.NewLine, AnalysisLogMessageFormatter.GetExternalMethodsInfoForEachFileLog(importedMethodInfos));

var dllLocations = importedMethodInfos.ToDllLocations();

if (!dllLocations.Any())
{
    logger.LogInformation("All analyzed fileInfos DO NOT contain external methods from any unmanaged assemblies.");
    return SuccessExitCode;
}

var fileExistenceStatuses = fileExistenceChecker.GetMinFileExistenceStatuses(dllLocations);
var fileExistenceInfos = fileExistenceStatuses.ToFileExistenceInfos();

logger.LogInformation("The presence of unmanaged assemblies in the locations specified in the DllImport attribute:{newLine}{fileExistenceInfo}",
    Environment.NewLine, AnalysisLogMessageFormatter.GetFileExistenceInfoLog(fileExistenceInfos));

var existingFiles = fileExistenceInfos.CountExistingFiles();
var nonExistingFiles = fileExistenceInfos.CountNonExistingFiles();

logger.LogInformation("OUTCOME: {existingFiles} exist, {nonExistingFiles} DO NOT exist.", existingFiles, nonExistingFiles);

return (int)fileExistenceChecker.GetMaxFileExistanceStatus(dllLocations);