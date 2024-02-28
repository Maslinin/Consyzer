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

var options = host.Services.GetRequiredService<IOptions<CommandLineOptions>>().Value;

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var fileHasher = host.Services.GetRequiredService<IFileHasher>();
var fileFilter = host.Services.GetRequiredService<IMetadataFileFilter>();
var fileMetadataChecker = host.Services.GetRequiredService<IFileMetadataChecker>();
var fileExistenceChecker = host.Services.GetRequiredService<IFileExistenceChecker>();

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

var fileInfoImportedMethodInfosPairs = metadataAssemblyFiles
    .ToImportedMethodExtractors()
    .ToImportedMethodInfos()
    .ToFileInfoImportedMethodInfosDictionary(metadataAssemblyFiles);

logger.LogInformation("Information about external functions from unmanaged assemblies being analyzed:{newLine}{importedMethodsInfo}",
    Environment.NewLine, LogMessageBuilderHelper.GetImportedMethodsInfoForEachFileLog(fileInfoImportedMethodInfosPairs));

var dllLocations = fileInfoImportedMethodInfosPairs
    .ToImportedMethodInfos()
    .ToDllLocations();

if (!dllLocations.Any())
{
    logger.LogInformation("All analyzed files DO NOT contain external functions from any unmanaged assemblies.");
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