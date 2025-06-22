namespace Consyzer.Options;

internal sealed class AnalysisOptions
{
    public required string AnalysisDirectory { get; set; }
    public required string SearchPattern { get; set; }
    public required bool RecursiveSearch { get; set; }
    public required OutputFormats Formats { get; set; } = OutputFormats.Console;
}