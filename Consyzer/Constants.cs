using Consyzer.Helpers;

namespace Consyzer;

internal static class Constants
{
    public static class Report
    {
        public static class Directory
        {
            public static string Base => Environment.CurrentDirectory;
            public const string Destination = "Reports";
            public static string FullPath => Path.Combine(Base, Destination);
        }

        public static class Format
        {
            public const string Timestamp = "yyyyMMdd_HHmmss";
        }

        public static class FileExtension
        {
            public const string Json = ".json";
            public const string Csv = ".csv";
            public const string Xml = ".xml";
        }

        public static class FileName
        {
            public const string Prefix = "report_";
            public const string ReserveIdentifier = "fallback";
            public static string Identifier => Path.GetFileNameWithoutExtension(LoggingHelper.GetCurrentLogFilePath() ?? ReserveIdentifier);
            public static string Json => $"{Prefix}{Identifier}{FileExtension.Json}";
            public static string Csv => $"{Prefix}{Identifier}{FileExtension.Csv}";
            public static string Xml => $"{Prefix}{Identifier}{FileExtension.Xml}";
        }
    }
}
