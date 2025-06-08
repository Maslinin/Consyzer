namespace Consyzer.Core.Models;

internal sealed class AnalysisFileClassification
{
    public required IEnumerable<FileInfo> EcmaModules { get; init; }
    public required IEnumerable<FileInfo> NonEcmaModules { get; init; }
    public required IEnumerable<FileInfo> EcmaAssemblies { get; init; }
    public required IEnumerable<FileInfo> NonEcmaAssemblies { get; init; }
}