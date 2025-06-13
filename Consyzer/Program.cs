using Consyzer;
using Consyzer.Analyzers;
using Consyzer.Core.Checkers;
using Consyzer.Core.Cryptography;
using Consyzer.Core.Extractors;
using Consyzer.Core.Models;
using Consyzer.Core.Resources;
using Consyzer.Helpers;
using Consyzer.Logging;
using Consyzer.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var serviceProvider = new ServiceCollection()
    // Logging
    .AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddNLog();
    })
    .AddSingleton<IAnalysisLogBuilder, AnalysisLogBuilder>()

    // Options
    .Configure<AnalysisOptions>(configuration)

    // Analyzers
    .AddSingleton<IAnalyzer<IEnumerable<FileInfo>, AnalysisFileClassification>, FileClassificationAnalyzer>()
    .AddSingleton<IAnalyzer<IEnumerable<FileInfo>, IEnumerable<AssemblyMetadata>>, AssemblyMetadataAnalyzer>()
    .AddSingleton<IAnalyzer<IEnumerable<FileInfo>, IEnumerable<PInvokeMethodsGroup>>, PInvokeMethodAnalyzer>()
    .AddSingleton<IAnalyzer<IEnumerable<string>, IEnumerable<DllPresence>>, DllPresenceAnalyzer>()
    .AddSingleton<IAnalyzer<IEnumerable<DllPresence>, int>, ExitCodeAnalyzer>()

    // Checkers
    .AddSingleton<IFileClassificationChecker<AnalysisFileClassification>, FileClassificationChecker>()

    // Extractors
    .AddSingleton<IExtractor<FileInfo, IEnumerable<PInvokeMethodsGroup>>, PInvokeMethodExtractor>()
    .AddSingleton<IExtractor<MethodDefinition, MethodSignature>, MethodSignatureExtractor>()
    .AddSingleton<IExtractor<FileInfo, AssemblyMetadata>, AssemblyMetadataExtractor>()

    //Cryptography
    .AddSingleton<IFileHasher, Sha256FileHasher>()

    //Resources
    .AddSingleton<IResourceAccessor<FileInfo, Stream>, FileStreamAccessor>()
    .AddSingleton<IResourceAccessor<FileInfo, PEReader>, PEReaderAccessor>()

    // Orchestrator
    .AddSingleton<AnalysisOrchestrator>()

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

var files = FileSearchHelper.GetFilesByCommaSeparatedPatterns(options.AnalysisDirectory, options.SearchPattern);
return orchestrator.Run(files);