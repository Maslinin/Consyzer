namespace Consyzer.Core.Models;

internal sealed record class LibraryPresence
{
    public required string LibraryName { get; init; }
    public required string? ResolvedPath { get; init; }
    public required LibraryLocationKind LocationKind { get; init; }
}

internal enum LibraryLocationKind
{
    InAnalyzedDirectory = 0,
    InEnvironmentPath = 1,
    InSystemDirectory = 2,
    OnAbsolutePath = 3,
    OnRelativePath = 4,
    Missing = 5
}