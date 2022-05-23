using System;
using System.IO;
using NLog;

namespace Consyzer.Logger
{
    /// <summary>
    /// Static class that provides static methods for event logging
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class NLogger
    {
        /// <summary>
        /// Gets an instance inherited from ILogger
        /// </summary>
        private readonly static ILogger Logger;
        /// <summary>
        /// Gets the path to the logging folder
        /// </summary>
        public static string LogDirPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        static NLogger()
        {
            if (!Directory.Exists(NLogger.LogDirPath))
            {
                Directory.CreateDirectory(NLogger.LogDirPath);
            }

            string logFilePath = Path.Combine(NLogger.LogDirPath, $"{DateTime.Now:MM.dd.yyyy}.txt");

            var fileTarget = new NLog.Targets.FileTarget()
            {
                FileName = logFilePath,
                DeleteOldFileOnStartup = false,
                Layout = "${message}"
            };
            var consoleTarget = new NLog.Targets.ConsoleTarget()
            {
                Layout = "${message}"
            };

            var logConfig = new NLog.Config.LoggingConfiguration();
            logConfig.AddRuleForAllLevels(fileTarget);
            logConfig.AddRuleForAllLevels(consoleTarget);
            LogManager.Configuration = logConfig;

            NLogger.Logger = LogManager.GetLogger("ConsyzerLogger");
        }

        /// <summary>
        /// Logs a message with the specific text into a file at the <b>LogDirPath</b> path with the specified logging level.<br/> 
        /// The logging level is set to <b>Info</b> otherwise.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        public static void Log(string message, LogLevel logLevel)
        {
            Logger.Log(logLevel, $"[{DateTime.Now:HH:mm:ss}::{logLevel.Name.ToUpper()}] {message}");
        }

        #region Logging methods by logging levels

        /// <summary>
        /// Logs a message with the specified text into a file at the <b>LogDirPath</b> path with the <b>Debug</b> logging level.<br/> 
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message) => NLogger.Log(message, LogLevel.Debug);

        /// <summary>
        /// Logs a message with the specified text into a file at the <b>LogDirPath</b> path with the <b>Info</b> logging level.<br/> 
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message) => NLogger.Log(message, LogLevel.Info);

        /// <summary>
        /// Logs a message with the specified text into a file at the <b>LogDirPath</b> path with the <b>Trace</b> logging level.<br/> 
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message) => NLogger.Log(message, LogLevel.Trace);

        /// <summary>
        /// Logs a message with the specified text into a file at the <b>LogDirPath</b> path with the <b>Warn</b> logging level.<br/> 
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message) => NLogger.Log(message, LogLevel.Warn);

        /// <summary>
        /// Logs a message with the specified text into a file at the <b>LogDirPath</b> path with the <b>Error</b> logging level.<br/> 
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message) => NLogger.Log(message, LogLevel.Error);

        #endregion

    }
}