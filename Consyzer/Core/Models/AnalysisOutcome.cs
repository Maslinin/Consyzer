namespace Consyzer.Core.Models;

internal sealed class AnalysisOutcome
{
    public required IEnumerable<AssemblyMetadata> AssemblyMetadataList { get; init; }
    public required IEnumerable<PInvokeMethodGroup> PInvokeGroups { get; init; }
    public required IEnumerable<LibraryPresence> LibraryPresences { get; init; }
    public required AnalysisSummary Summary { get; init; }
}
