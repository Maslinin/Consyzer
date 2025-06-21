namespace Consyzer.Core.Models;

internal sealed class LibraryPresence
{
    public required string LibraryName { get; init; }
    public required string? ResolvedPath { get; init; }
    public required LibraryLocationKind LocationKind { get; init; }
}

internal enum LibraryLocationKind
{
    InAnalyzedDirectory = 0,
    InSystemDirectory = 1,
    InEnvironmentPath = 2,
    OnAbsolutePath = 3,
    OnRelativePath = 4,
    Missing = 5
}