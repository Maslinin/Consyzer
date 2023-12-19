using NLog.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Consyzer.Options;
using Consyzer.Filters;
using Consyzer.Helpers;
using Consyzer.Checkers;
using Consyzer.Cryptography;
using Consyzer.Options.Models;
using Consyzer.Checkers.Models.Extensions;
using Consyzer.Extractors.Models.Extensions;

const int SuccessExitCode = 0;
const int UnexpectedBehaviorExitCode = -1;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScoped<IFileHasher, Sha256FileHasher>();
        services.AddScoped<IMetadataFileFilter, EcmaMetadataFileFilter>();
        services.AddScoped<IFileMetadataChecker, FileMetadataChecker>();
        services.AddScoped<IFileExistenceChecker, DllExistenceChecker>();
        services.AddSingleton<IConfigureOptions<CommandLineOptions>, CommandLineArgsConfigureOptions>();

        services.Configure<CommandLineOptions>(hostContext.Configuration);
    })
    .ConfigureLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddNLog();
    })
    .Build();

var options = host.Services.GetRequiredService<IOptions<CommandLineOptions>>();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var fileHasher = host.Services.GetRequiredService<IFileHasher>();
var fileFilter = host.Services.GetRequiredService<IMetadataFileFilter>();
var fileMetadataChecker = host.Services.GetRequiredService<IFileMetadataChecker>();
var fileExistenceChecker = host.Services.GetRequiredService<IFileExistenceChecker>();

await host.StartAsync();

if (!Directory.Exists(options.Value.AnalysisDirectory))
{
    logger.LogWarning("Invalid directory for analysis is specified.");
    return UnexpectedBehaviorExitCode;
}

if (string.IsNullOrEmpty(options.Value.SearchPattern))
{
    logger.LogWarning("Invalid file search template for analysis is specified.");
    return UnexpectedBehaviorExitCode;
}

logger.LogInformation("{analysisParams}", LogMessageBuilderHelper.BuildAnalysisParamsLog(options.Value));

var files = FileSearchHelper.GetFilesByCommaSeparatedSearchPatterns(options.Value.AnalysisDirectory, options.Value.SearchPattern);
if (!files.Any())
{
    logger.LogWarning("Files for analysis corresponding to the specified search pattern were not found.");
    return UnexpectedBehaviorExitCode;
}

logger.LogInformation("Files for analysis corresponding to the specified search pattern were found:{newLine}{baseFileInfo}",
    Environment.NewLine, LogMessageBuilderHelper.BuildBaseFileInfoLog(files));

if (!fileMetadataChecker.ContainsOnlyMetadata(files)) return SuccessExitCode;
if (!fileMetadataChecker.ContainsOnlyMetadataAssemblies(files)) return SuccessExitCode;

var metadataAssemblyFiles = fileFilter.GetMetadataAssemblyFiles(files);

logger.LogInformation("The following assembly files containing metadata were found:{newLine}{fileAndHashInfo}",
    Environment.NewLine, LogMessageBuilderHelper.BuildBaseAndHashFileInfoLog(metadataAssemblyFiles, fileHasher));

var fileInfoAndImportedMethodInfos = metadataAssemblyFiles
    .ToImportedMethodExtractors()
    .ToImportedMethodInfos()
    .ToFileInfoImportedMethodInfosDictionary(metadataAssemblyFiles);

logger.LogInformation("Information about imported methods from other assemblies in the analyzed files:{newLine}{importedMethodsInfo}",
    Environment.NewLine, LogMessageBuilderHelper.GetImportedMethodsInfoForEachFileLog(fileInfoAndImportedMethodInfos));

var dllLocations = fileInfoAndImportedMethodInfos
    .ToImportedMethodInfos()
    .ToDllLocations();

if (!dllLocations.Any())
{
    logger.LogInformation("All files DO NOT contain methods from other assemblies.");
    return SuccessExitCode;
}

logger.LogInformation("The presence of files at the received DLL import paths:");

var fileExistenceStatuses = fileExistenceChecker.ToMinFileExistenceStatuses(dllLocations);
var fileExistenceInfos = fileExistenceStatuses.ToFileExistenceInfos(dllLocations);

logger.LogInformation("{fileExistenceInfo}", LogMessageBuilderHelper.BuildFileExistenceInfoLog(fileExistenceInfos));

var existingFiles = fileExistenceStatuses.CountExistingFiles();
var nonExistingFiles = fileExistenceStatuses.CountNonExistingFiles();

logger.LogInformation("TOTAL: {existingFiles} exists, {nonExistingFiles} DO NOT exist.", existingFiles, nonExistingFiles);

return (int)fileExistenceChecker.GetMaxFileExistanceStatus(dllLocations);