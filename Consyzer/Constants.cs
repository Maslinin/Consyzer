using Consyzer.Options;
using Consyzer.Helpers;
using Consyzer.Core.Models;

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

            public static readonly string Base = Environment.CurrentDirectory;
            public static readonly string FullPath = Path.Combine(Base, Destination);
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

            public static readonly string Json = $"{Prefix}{Identifier}{Extension.Json}";
            public static readonly string Csv = $"{Prefix}{Identifier}{Extension.Csv}";
            public static readonly string Identifier = Path.GetFileNameWithoutExtension(LoggingHelper.GetCurrentLogFilePath() ?? ReserveIdentifier);
        }
    }

    internal static class OutputStructure
    {
        public static class Section
        {
            public const string AnalysisOptions = $"[{nameof(Options.AnalysisOptions)}]";
            public const string FilesFound = "[FilesFound]";
            public const string FileClassification = $"[{nameof(AnalysisFileClassification)}]";
            public const string NotEcma = $"[{nameof(AnalysisFileClassification.NonEcmaModules)}]";
            public const string NotAssemblies = $"[{nameof(AnalysisFileClassification.NonEcmaAssemblies)}]";
            public const string EcmaAssemblies = $"[{nameof(AnalysisFileClassification.EcmaAssemblies)}]";

            public const string AssemblyMetadataList = $"[{nameof(AnalysisOutcome.AssemblyMetadataList)}]";
            public const string PInvokeGroups = $"[{nameof(AnalysisOutcome.PInvokeGroups)}]";
            public const string LibraryPresences = $"[{nameof(AnalysisOutcome.LibraryPresences)}]";
            public const string Summary = $"[{nameof(AnalysisOutcome.Summary)}]";
        }

        public static class Label
        {
            public static class Options
            {
                public const string AnalysisDirectory = nameof(AnalysisOptions.AnalysisDirectory);
                public const string SearchPattern = nameof(AnalysisOptions.SearchPattern);
            }

            public static class Assembly
            {
                public const string File = nameof(AssemblyMetadata.File);
                public const string Version = nameof(AssemblyMetadata.Version);
                public const string CreationDateUtc = nameof(AssemblyMetadata.CreationDateUtc);
                public const string Sha256 = nameof(AssemblyMetadata.Sha256);
            }

            public static class PInvoke
            {
                public const string File = nameof(PInvokeMethodGroup.File);
                public const string Signature = nameof(PInvokeMethod.Signature);
                public const string ImportName = nameof(PInvokeMethod.ImportName);
                public const string ImportFlags = nameof(PInvokeMethod.ImportFlags);
            }

            public static class Summary
            {
                public const string TotalFiles = nameof(AnalysisSummary.TotalFiles);
                public const string EcmaAssemblies = nameof(AnalysisSummary.EcmaAssemblies);
                public const string AssembliesWithPInvoke = nameof(AnalysisSummary.AssembliesWithPInvoke);
                public const string TotalPInvokeMethods = nameof(AnalysisSummary.TotalPInvokeMethods);
                public const string MissingLibraries = nameof(AnalysisSummary.MissingLibraries);
            }
        }
    }
}