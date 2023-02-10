using System;
using NLog;

namespace Consyzer.Logging
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class NLogger
    {
        private readonly static ILogger _logger = LogManager
            .Setup()
            .LoadConfigurationFromFile(optional: false)
            .GetCurrentClassLogger();

        public static void Log(string message, LogLevel logLevel)
        {
            _logger.Log(logLevel, message);
        }

        public static void Error(string message) => Log(message, LogLevel.Error);
        public static void Warn(string message) => Log(message, LogLevel.Warn);
        public static void Info(string message) => Log(message, LogLevel.Info);
        public static void Debug(string message) => Log(message, LogLevel.Debug);
        public static void Trace(string message) => Log(message, LogLevel.Trace);

    }
}