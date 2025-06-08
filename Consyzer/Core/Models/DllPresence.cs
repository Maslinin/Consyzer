namespace Consyzer.Core.Models;

internal sealed class DllPresence
{
    public required string DllName { get; init; }
    public required string? ResolvedPath { get; init; }
    public required DllLocationKind LocationKind { get; init; }
}

internal enum DllLocationKind
{
    InAnalyzedDirectory = 0,
    InEnvironmentPath = 1,
    InSystemDirectory = 2,
    OnAbsolutePath = 3,
    OnRelativePath = 4,
    Missing = 5
}