using Consyzer.Logging;
using Consyzer.Options;
using Consyzer.Reporting;
using Consyzer.Analyzers;
using Consyzer.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Consyzer;

internal sealed class AnalysisOrchestrator(
    ILogger<AnalysisOrchestrator> logger,
    IAnalysisLogBuilder analysisLogBuilder,
    IOptions<AnalysisOptions> analysisOptions,
    IAnalyzer<IEnumerable<FileInfo>, AnalysisFileClassification> fileClassificationAnalyzer,
    IAnalyzer<IEnumerable<FileInfo>, IEnumerable<AssemblyMetadata>> metadataAnalyzer,
    IAnalyzer<IEnumerable<FileInfo>, IEnumerable<PInvokeMethodGroup>> pinvokeAnalyzer,
    IAnalyzer<IEnumerable<PInvokeMethodGroup>, IEnumerable<LibraryPresence>> libraryPresenceAnalyzer,
    IAnalyzer<IEnumerable<LibraryPresence>, int> exitCodeAnalyzer,
    IEnumerable<IReportWriter> reportWriters
)
{
    public int Run(IEnumerable<FileInfo> files)
    {
        logger.LogDebug("{Message}", analysisLogBuilder.BuildAnalysisOptionsLog(analysisOptions.Value));

        logger.LogInformation("Analysis started.");

        if (!files.Any())
        {
            logger.LogError("No files found by the given search pattern.");
            return (int)AppFailureCode.NoFilesFound;
        }

        logger.LogInformation("{Message}", analysisLogBuilder.BuildFoundFilesLog(files));

        var fileClassification = fileClassificationAnalyzer.Analyze(files);
        logger.LogInformation("{Message}", analysisLogBuilder.BuildFileClassificationLog(fileClassification));

        if (!fileClassification.EcmaAssemblies.Any())
        {
            logger.LogError("No valid ECMA assemblies found.");
            return (int)AppFailureCode.AllFilesInvalid;
        }

        logger.LogInformation("Starting metadata analysis of {Count} ECMA assemblies...", fileClassification.EcmaAssemblies.Count());
        var metadataList = metadataAnalyzer.Analyze(fileClassification.EcmaAssemblies);

        logger.LogInformation("Analyzing P/Invoke methods...");
        var pInvokeGroups = pinvokeAnalyzer.Analyze(fileClassification.EcmaAssemblies);

        logger.LogInformation("Checking library presence for {Count} P/Invoke method groups...", pInvokeGroups.Count());
        var libraryPresences = libraryPresenceAnalyzer.Analyze(pInvokeGroups);

        var summary = new AnalysisSummary
        {
            TotalFiles = files.Count(),
            EcmaAssemblies = metadataList.Count(),
            AssembliesWithPInvoke = pInvokeGroups.Count(),
            TotalPInvokeMethods = pInvokeGroups.Sum(g => g.Methods.Count()),
            MissingLibraries = libraryPresences.Count(l => l.LocationKind == LibraryLocationKind.Missing)
        };

        var outcome = new AnalysisOutcome
        {
            AssemblyMetadataList = metadataList,
            PInvokeMethodGroups = pInvokeGroups,
            LibraryPresences = libraryPresences,
            Summary = summary
        };

        foreach (var writer in reportWriters)
        {
            logger.LogInformation("Generating report using {WriterType}...", writer.GetType().Name);
            var destination = writer.Write(outcome);
            logger.LogInformation("Report successfully written to {Destination}.", destination);
        }

        logger.LogInformation("Analysis completed. Report(s) successfully generated.");

        return exitCodeAnalyzer.Analyze(libraryPresences);
    }
}