using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Consyzer.Logging;
using Consyzer.Options;
using Consyzer.Analyzers;
using Consyzer.Core.Models;

namespace Consyzer;

internal sealed class AnalysisOrchestrator(
    ILogger<AnalysisOrchestrator> logger,
    IOptions<AnalysisOptions> options,
    IAnalysisLogBuilder logBuilder,
    IAnalyzer<IEnumerable<FileInfo>, AnalysisFileClassification> fileClassifier,
    IAnalyzer<IEnumerable<FileInfo>, IEnumerable<AssemblyMetadata>> metadataAnalyzer,
    IAnalyzer<IEnumerable<FileInfo>, IEnumerable<PInvokeMethodGroup>> pinvokeAnalyzer,
    IAnalyzer<IEnumerable<string>, IEnumerable<DllPresence>> dllPresenceAnalyzer,
    IAnalyzer<IEnumerable<DllPresence>, int> exitCodeAnalyzer
)
{
    public int Run(IEnumerable<FileInfo> files)
    {
        logger.LogInformation("{Message}", logBuilder.BuildAnalysisOptionsLog(options.Value));

        if (!files.Any())
        {
            logger.LogError("No files found by the given search pattern.");
            return (int)AppFailureCode.NoFilesFound;
        }

        logger.LogInformation("{Message}", logBuilder.BuildFoundFilesLog(files));

        var fileClassification = fileClassifier.Analyze(files);
        logger.LogInformation("{Message}", logBuilder.BuildFileClassificationLog(fileClassification));

        if (!fileClassification.EcmaAssemblies.Any())
        {
            logger.LogError("No valid ECMA assemblies found.");
            return (int)AppFailureCode.AllFilesInvalid;
        }

        var metadataList = metadataAnalyzer.Analyze(fileClassification.EcmaAssemblies);
        logger.LogInformation("{Message}", logBuilder.BuildEcmaAssemblyMetadataLog(metadataList));

        var pInvokedMethodGroups = pinvokeAnalyzer.Analyze(fileClassification.EcmaAssemblies);

        logger.LogInformation("{Message}", logBuilder.BuildPInvokeMethodGroupsLog(pInvokedMethodGroups));

        var distinctDlls = pInvokedMethodGroups
            .SelectMany(g => g.Methods.Select(m => m.ImportName))
            .Distinct(StringComparer.OrdinalIgnoreCase);

        var dllPresence = dllPresenceAnalyzer.Analyze(distinctDlls);
        logger.LogInformation("{Message}", logBuilder.BuildDllPresenceLog(dllPresence));

        var exitCode = exitCodeAnalyzer.Analyze(dllPresence);

        logger.LogInformation(
            "{Message}",
            logBuilder.BuildFinalSummaryLog(
                fileClassification,
                dllPresence,
                pInvokedMethodGroups
            )
        );

        return exitCode;
    }
}
