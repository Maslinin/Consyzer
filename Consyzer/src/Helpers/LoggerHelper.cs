using System.Linq;
using System.Collections.Generic;
using Consyzer.Logger;
using Consyzer.AnalyzerEngine.Analyzers;
using Consyzer.AnalyzerEngine.Analyzers.Searchers;
using Consyzer.AnalyzerEngine.Analyzers.SyntaxModels;
using Consyzer.AnalyzerEngine.CommonModels;
using Consyzer.AnalyzerEngine.Helpers;

namespace Consyzer.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class LoggerHelper
    {
        public static void LoggingFileExtensionsForAnalysis(IEnumerable<string> filesExtensions)
        {
            NLogger.Info($"Specified binary file extensions for analysis: {string.Join(", ", filesExtensions.Select(e => $"'{e}'"))}.");
        }


        public static void LoggingBaseFileInfo(IEnumerable<BinaryFileInfo> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($"\t[{item.Index}]File: '{item.File.BaseFileInfo.Name}', Creation Time: '{item.File.BaseFileInfo.CreationTime}'.");
            }
        }

        public static void LoggingBaseAndHashFileInfo(IEnumerable<MetadataAnalyzer> metadataAnalyzers)
        {
            foreach (var item in metadataAnalyzers.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($"\t[{item.Index}]File: '{item.File.BinaryInfo.BaseFileInfo.Name}', Creation Time: '{item.File.BinaryInfo.BaseFileInfo.CreationTime}', " +
                    $"SHA256 Hash Sum: '{item.File.BinaryInfo.HashInfo.SHA256Sum}'.");
            }
        }

        public static void LoggingImportedMethodsInfoForEachBinary(IEnumerable<MetadataAnalyzer> metadataAnalyzers)
        {
            foreach (var item in metadataAnalyzers.Select((File, i) => (File, i)))
            {
                var importedMethods = item.File.GetImportedMethodsInfo().ToList();

                NLogger.Info($"\t[{item.i}]File '{item.File.BinaryInfo.BaseFileInfo.FullName}': ");
                if (importedMethods.Any())
                {
                    LoggerHelper.LoggingImportedMethodsInfo(importedMethods);
                }
                else
                {
                    NLogger.Info($"\t\tThere is no any imported methods from other assemblies in the file.");
                }
            }
        }

        public static void LoggingImportedMethodsInfo(IEnumerable<ImportedMethodInfo> importedMethods)
        {
            foreach (var import in importedMethods.Select((Signature, i) => (Signature, i)))
            {
                NLogger.Info($"\t\t[{import.i}]Method '{import.Signature.SignatureInfo.GetMethodLocation()}':");
                NLogger.Info($"\t\t\tMethod Signature: '{import.Signature.SignatureInfo.GetBaseMethodSignature()}',");
                NLogger.Info($"\t\t\tDLL Location: '{import.Signature.DllLocation}',");
                NLogger.Info($"\t\t\tDLL Import Arguments: '{import.Signature.DllImportArguments}'.");
            }
        }

        public static void LoggingBinariesExistStatus(IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            foreach (var item in binaryLocations.Select((Location, i) => (Location, i)))
            {
                if (BinarySearcher.CheckBinaryExistInSourceAndSystemFolder(item.Location, analysisFolder, defaultBinaryExtension) is BinarySearcherStatusCodes.BinaryNotExists)
                {
                    NLogger.Error($"\t[{item.i}]File '{item.Location}': does NOT exist!");
                }
                else
                {
                    NLogger.Info($"\t[{item.i}]File '{item.Location}': exist.");
                }
            }
        }

        public static void LoggingBinaryExistAndNonExistCount(IEnumerable<string> binaryLocations, string analysisFolder)
        {
            NLogger.Info($"Total: {AnalyzerHelper.GetExistsBinaries(binaryLocations, analysisFolder).Count()} exist, " +
                $"{AnalyzerHelper.GetNotExistsBinaries(binaryLocations, analysisFolder).Count()} do not exist.");
        }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class LoggerCheckerHelper
    {
        public static bool CheckAndLoggingFilesCorrect(IEnumerable<BinaryFileInfo> binaryFiles)
        {
            var notMetadataFiles = binaryFiles.GetFilesNotContainsMetadata();

            if (notMetadataFiles.Any())
            {
                NLogger.Info("The following files were excluded from analysis because they DO NOT contain metadata:");
                LoggerHelper.LoggingBaseFileInfo(notMetadataFiles);

                if (binaryFiles.Count() == notMetadataFiles.Count())
                {
                    NLogger.Warn("All found files do NOT contain metadata.");
                    return false;
                }
            }

            var notMetadataAssemblyFiles = binaryFiles.GetNotMetadataAssemblyFiles();
            var differentFiles = notMetadataFiles.Count() > notMetadataAssemblyFiles.Count() ? notMetadataFiles.Except(notMetadataAssemblyFiles) : notMetadataAssemblyFiles.Except(notMetadataFiles);
            if (differentFiles.Any())
            {
                NLogger.Info("The following files were excluded from analysis because they are NOT assembly files:");
                LoggerHelper.LoggingBaseFileInfo(differentFiles);

                if (binaryFiles.Count() == differentFiles.Count())
                {
                    NLogger.Warn("All found files contain metadata, but are NOT assembly files.");
                    return false;
                }
            }

            return true;
        }

        public static bool CheckAndLoggingBinaryFilesExist(IEnumerable<BinaryFileInfo> binaryFiles)
        {
            if (binaryFiles.Any())
                return true;

            NLogger.Warn("Binary files for analysis with the specified extensions were not found.");
            return false;
        }

        public static bool CheckAndLoggingDllLocationsExist(IEnumerable<string> binaryLocations)
        {
            if (binaryLocations.Any())
                return true;

            NLogger.Info("All files are missing imported methods from other assemblies.");
            return false;
        }
    }
}
