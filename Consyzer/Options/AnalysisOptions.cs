namespace Consyzer.Options;

internal sealed class AnalysisOptions
{
    public required string AnalysisDirectory { get; init; }
    public required string SearchPattern { get; init; }
    public required bool Recursive { get; init; }
    public required OutputFormats Formats { get; init; } = OutputFormats.Console;
}