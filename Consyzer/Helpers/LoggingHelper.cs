using NLog;
using NLog.Targets;

namespace Consyzer.Helpers;

internal static class LoggingHelper
{
    public static string? GetCurrentLogFilePath()
    {
        var config = LogManager.Configuration;
        if (config == null) return null;

        var fileTarget = config.AllTargets
            .OfType<FileTarget>()
            .FirstOrDefault();

        if (fileTarget == null) return null;

        var logEventInfo = new LogEventInfo { TimeStamp = DateTime.UtcNow };
        return fileTarget.FileName.Render(logEventInfo);
    }
}