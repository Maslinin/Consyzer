using Consyzer.Core.Models;

namespace Consyzer.Reporting.Writers.Sections;

internal static class ReportSections
{
    public static readonly string AssemblyMetadataList = $"[{nameof(AnalysisOutcome.AssemblyMetadataList)}]";
    public static readonly string PInvokeGroups = $"[{nameof(AnalysisOutcome.PInvokeGroups)}]";
    public static readonly string LibraryPresences = $"[{nameof(AnalysisOutcome.LibraryPresences)}]";
    public static readonly string Summary = $"[{nameof(AnalysisOutcome.Summary)}]";
}