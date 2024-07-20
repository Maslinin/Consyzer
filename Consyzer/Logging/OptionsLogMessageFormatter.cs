using Consyzer.Options;

namespace Consyzer.Logging;

internal static class OptionsLogMessageFormatter
{
    public static string GetCommandLineOptionsLog(CommandLineOptions arguments)
    {
        string analysisDirectory = $"The specified directory for analysis: '{arguments.AnalysisDirectory}'.";
        string searchPattern = $"The specified file search pattern for analysis: {arguments.SearchPattern}.";

        return $"{analysisDirectory}{Environment.NewLine}{searchPattern}";
    }
}
