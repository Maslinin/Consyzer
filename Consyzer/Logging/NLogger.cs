using System;
using System.Globalization;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Consyzer.Logging
{
    /// <summary>
    /// Static class that provides static methods for event logging
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class NLogger
    {
        private readonly static ILogger _logger = Initialize();
        public static string LogDirPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        private static ILogger Initialize()
        {
            ILogger logger;

            string logFilePath = Path.Combine(LogDirPath, $"{DateTime.Now:MM.dd.yyyy}.txt");

            var fileTarget = new FileTarget()
            {
                FileName = logFilePath,
                Layout = "${message}"
            };
            var consoleTarget = new ConsoleTarget()
            {
                Layout = "${message}"
            };

            var logConfig = new LoggingConfiguration();
            logConfig.AddRuleForAllLevels(fileTarget);
            logConfig.AddRuleForAllLevels(consoleTarget);
            LogManager.Configuration = logConfig;

            logger = LogManager.GetLogger("ConsyzerLogger");

            return logger;
        }

        public static void Log(string message, LogLevel logLevel)
        {
            _logger.Log(logLevel, $"[{DateTime.Now:HH:mm:ss}::{logLevel.Name.ToUpper()}] {message}");
        }

        #region Logging methods by logging levels

        public static void Error(string message) => Log(message, LogLevel.Error);
        public static void Warn(string message) => Log(message, LogLevel.Warn);
        public static void Info(string message) => Log(message, LogLevel.Info);
        public static void Debug(string message) => Log(message, LogLevel.Debug);
        public static void Trace(string message) => Log(message, LogLevel.Trace);

        #endregion

    }
}