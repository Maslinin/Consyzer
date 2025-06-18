using Consyzer.Helpers;

namespace Consyzer;

internal static class Constants
{
    public static class Search
    {
        public const char PatternSeparator = ',';
    }

    public static class LibraryPresence
    {
        public static class Variable
        {
            public const string Path = "PATH";
        }

        public static class Extension
        {
            public const string WindowsExtension = ".dll";
            public const string LinuxExtension = ".so";
            public const string MacExtension = ".dylib";
        }
    }

    public static class Report
    {
        public static class Directory
        {
            public const string Destination = "Reports";

            public static string Base => Environment.CurrentDirectory;
            public static string FullPath => Path.Combine(Base, Destination);
        }

        public static class Extension
        {
            public const string Json = ".json";
            public const string Csv = ".csv";
            public const string Xml = ".xml";
        }

        public static class Name
        {
            public const string Prefix = "report_";
            public const string ReserveIdentifier = "fallback";

            public static string Json => $"{Prefix}{Identifier}{Extension.Json}";
            public static string Csv => $"{Prefix}{Identifier}{Extension.Csv}";
            public static string Xml => $"{Prefix}{Identifier}{Extension.Xml}";
            public static string Identifier => Path.GetFileNameWithoutExtension(LoggingHelper.GetCurrentLogFilePath() ?? ReserveIdentifier);
        }
    }
}