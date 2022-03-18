using System;
using System.IO;
using NLog;

namespace Consyzer.Logger
{
    /// <summary>
    /// Static class that provides static methods for event logging
    /// </summary>
    public static class NLogger
    {
        /// <summary>
        /// Gets an instance inherited from ILogger
        /// </summary>
        private readonly static ILogger Logger;
        /// <summary>
        /// Gets the path to the logging folder
        /// </summary>
        public static string LogDirPath { get; }

        static NLogger()
        {
            try
            {
                LogDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(LogDirPath))
                {
                    Directory.CreateDirectory(LogDirPath);
                }

                string logFilePath = Path.Combine(LogDirPath, $"{DateTime.Now:MM.dd.yyyy}.txt");

                var fileTarget = new NLog.Targets.FileTarget("ConsyzerLogger")
                {
                    FileName = logFilePath,
                    DeleteOldFileOnStartup = false,
                    Layout = "${message}"
                };
                var consoleTarget = new NLog.Targets.ConsoleTarget("ConsyzerLogger")
                {
                    Layout = "${message}"
                };

                var logConfig = new NLog.Config.LoggingConfiguration();
                logConfig.AddRuleForAllLevels(fileTarget);
                logConfig.AddRuleForAllLevels(consoleTarget);
                LogManager.Configuration = logConfig;

                Logger = LogManager.GetLogger("DLLConsyzerLogger");
            }
            catch(Exception e)
            {
                throw new AggregateException("Error initializing static class fields", e);
            }
        }

        /// <summary>
        /// Logs a message with the specified text into a file at the <b>LogDirPath</b> path with the specified logging level.<br/> 
        /// The logging level is set to <b>Info</b> if it is not set.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        public static void Log(string message, LogLevel logLevel)
        {
            Logger.Log(logLevel, $"[{DateTime.Now:HH:mm:ss}: {logLevel.Name.ToUpper()}] {message}");
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