namespace Consyzer.Options;

internal sealed class AnalysisOptions
{
    public required string AnalysisDirectory { get; init; }
    public required string SearchPattern { get; init; }
    public required OutputFormat OutputFormats { get; init; } = OutputFormat.Console;

    [Flags]
    public enum OutputFormat
    {
        Console = 1 << 0,
        Csv = 1 << 1,
        Json = 1 << 2
    }
}