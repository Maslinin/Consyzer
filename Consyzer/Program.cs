using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Consyzer;
using Consyzer.Helpers;
using Consyzer.Logging;
using Consyzer.Options;
using Consyzer.Analyzers;
using Consyzer.Core.Models;
using Consyzer.Core.Checkers;
using Consyzer.Core.Resources;
using Consyzer.Core.Extractors;
using Consyzer.Core.Cryptography;
using Consyzer.DependencyInjection;
using static Consyzer.Constants.Search;

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var rawOptions = configuration.Get<AnalysisOptions>()!;

var serviceProvider = new ServiceCollection()
    // Logging
    .AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddNLog();
    })
    .AddSingleton<IAnalysisLogBuilder, AnalysisLogBuilder>()

    // Reporting
    .AddReportWriters(rawOptions.Formats)

    // Analyzers
    .AddScoped<IAnalyzer<IEnumerable<FileInfo>, AnalysisFileClassification>, FileClassificationAnalyzer>()
    .AddScoped<IAnalyzer<IEnumerable<FileInfo>, IEnumerable<AssemblyMetadata>>, AssemblyMetadataAnalyzer>()
    .AddScoped<IAnalyzer<IEnumerable<FileInfo>, IEnumerable<PInvokeMethodGroup>>, PInvokeMethodAnalyzer>()
    .AddScoped<IAnalyzer<IEnumerable<PInvokeMethodGroup>, IEnumerable<LibraryPresence>>, LibraryPresenceAnalyzer>()
    .AddScoped<IAnalyzer<IEnumerable<LibraryPresence>, int>, ExitCodeAnalyzer>()

    // Extractors
    .AddScoped<IExtractor<FileInfo, IEnumerable<PInvokeMethod>>, PInvokeMethodExtractor>()
    .AddScoped<IExtractor<MethodDefinition, MethodSignature>, MethodSignatureExtractor>()
    .AddScoped<IExtractor<FileInfo, AssemblyMetadata>, AssemblyMetadataExtractor>()

    // Checkers
    .AddScoped<IFileClassificationChecker<AnalysisFileClassification>, FileClassificationChecker>()

    // Cryptography
    .AddSingleton<IFileHasher, Sha256FileHasher>()

    // Resources
    .AddSingleton<IResourceAccessor<FileInfo, Stream>, FileStreamAccessor>()
    .AddSingleton<IResourceAccessor<FileInfo, PEReader>, PEReaderAccessor>()

    // Orchestrator
    .AddSingleton<AnalysisOrchestrator>()

    // Options
    .Configure<AnalysisOptions>(configuration)
    .Configure<AppOptions>(configuration)

    .BuildServiceProvider();

var options = serviceProvider.GetRequiredService<IOptions<AnalysisOptions>>().Value;
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

if (string.IsNullOrWhiteSpace(options.AnalysisDirectory))
{
    logger.LogError("{Parameter} parameter is missing.", nameof(options.AnalysisDirectory));
    return (int)AppFailureCode.NoAnalysisDirectory;
}

if (string.IsNullOrWhiteSpace(options.SearchPattern))
{
    logger.LogError("{Parameter} parameter is missing.", nameof(options.SearchPattern));
    return (int)AppFailureCode.NoSearchPattern;
}

var orchestrator = serviceProvider.GetRequiredService<AnalysisOrchestrator>();

var files = FileSearchHelper.GetFilesBySeparatedPatterns(options.AnalysisDirectory, options.SearchPattern, PatternSeparator, options.RecursiveSearch);
return orchestrator.Run(files);