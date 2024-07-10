using NLog.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Consyzer.Options;
using Consyzer.Filters;
using Consyzer.Helpers;
using Consyzer.Checkers;
using Consyzer.Cryptography;
using Consyzer.Checkers.Models.Extensions;
using Consyzer.Extractors.Models.Extensions;

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
    .Configure<CommandLineOptions>(configuration)
    .BuildServiceProvider();

var options = serviceProvider.GetRequiredService<IOptions<CommandLineOptions>>().Value;

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

logger.LogInformation("{analysisParams}", LogMessageBuilderHelper.BuildAnalysisParamsLog(options));

var files = FileSearchHelper.GetFilesByCommaSeparatedSearchPatterns(options.AnalysisDirectory, options.SearchPattern);
if (!files.Any())
{
    logger.LogWarning("Files for analysis matching the specified search pattern have not been found.");
    return UnexpectedBehaviorExitCode;
}

logger.LogInformation("Files for analysis matching the specified search pattern have been found:{newLine}{baseFileInfo}",
    Environment.NewLine, LogMessageBuilderHelper.BuildBaseFileInfoLog(files));

if (!fileMetadataChecker.ContainsOnlyMetadata(files)) return SuccessExitCode;
if (!fileMetadataChecker.ContainsOnlyMetadataAssemblies(files)) return SuccessExitCode;

var metadataAssemblyFiles = fileFilter.GetMetadataAssemblyFiles(files);

logger.LogInformation("The following assembly files containing metadata have been found:{newLine}{fileAndHashInfo}",
    Environment.NewLine, LogMessageBuilderHelper.BuildBaseAndHashFileInfoLog(metadataAssemblyFiles, fileHasher));

var importedMethodInfos = metadataAssemblyFiles
    .ToImportedMethodExtractors()
    .ToImportedMethodInfos();

logger.LogInformation("Information about external methods from unmanaged assemblies being analyzed:{newLine}{importedMethodsInfo}",
    Environment.NewLine, LogMessageBuilderHelper.GetExternalMethodsInfoForEachFileLog(importedMethodInfos));

var dllLocations = importedMethodInfos.ToDllLocations();

if (!dllLocations.Any())
{
    logger.LogInformation("All analyzed files DO NOT contain external methods from any unmanaged assemblies.");
    return SuccessExitCode;
}

var fileExistenceStatuses = fileExistenceChecker.ToMinFileExistenceStatuses(dllLocations);
var fileExistenceInfos = fileExistenceStatuses.ToFileExistenceInfos(dllLocations);

logger.LogInformation("The presence of unmanaged assemblies in the locations specified in the DllImport attribute:{newLine}{fileExistenceInfo}",
    Environment.NewLine, LogMessageBuilderHelper.BuildFileExistenceInfoLog(fileExistenceInfos));

var existingFiles = fileExistenceStatuses.CountExistingFiles();
var nonExistingFiles = fileExistenceStatuses.CountNonExistingFiles();

logger.LogInformation("OUTCOME: {existingFiles} exist, {nonExistingFiles} DO NOT exist.", existingFiles, nonExistingFiles);

return (int)fileExistenceChecker.GetMaxFileExistanceStatus(dllLocations);