using Consyzer.Options;
using Consyzer.Core.Models;

namespace Consyzer.Logging;

internal interface IAnalysisLogBuilder
{
    string BuildAnalysisOptionsLog(AnalysisOptions options);
    string BuildFoundFilesLog(IEnumerable<FileInfo> files);
    string BuildFileClassificationLog(AnalysisFileClassification fileClassification);
    string BuildEcmaAssemblyMetadataLog(IEnumerable<AssemblyMetadata> metadataList);
    string BuildPInvokeMethodGroupsLog(IEnumerable<PInvokeMethodsGroup> groups);
    string BuildDllPresenceLog(IEnumerable<DllPresence> presences);

    string BuildFinalSummaryLog(
        AnalysisFileClassification fileClassification,
        IEnumerable<DllPresence> dllPresenceList,
        IEnumerable<PInvokeMethodsGroup> pinvokeGroups);
}