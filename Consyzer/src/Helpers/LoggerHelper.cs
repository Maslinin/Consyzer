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

        public static bool LoggingAnalyzedFilesValidityCheck(IEnumerable<BinaryFileInfo> binaryFiles)
        {
            var metadataFiles = binaryFiles.GetFilesContainsMetadata();
            var unsuitableFiles = binaryFiles.GetFilesNotContainsMetadata();

            if (metadataFiles.Count() == unsuitableFiles.Count())
            {
                NLogger.Warn("All found files do NOT contain metadata.");
                return true;
            }
            if (unsuitableFiles.Any())
            {
                NLogger.Warn("The following files were excluded from analysis because they DO NOT contain metadata:");
                LoggerHelper.LoggingBaseFileInfo(unsuitableFiles);
            }

            unsuitableFiles = metadataFiles.GetNotMetadataAssemblyFiles();
            if (metadataFiles.Count() == unsuitableFiles.Count())
            {
                NLogger.Warn("All found files contain metadata, but are NOT assembly files.");
                return true;
            }
            if (unsuitableFiles.Any())
            {
                NLogger.Warn("The following files were excluded from analysis because they are NOT assembly files:");
                LoggerHelper.LoggingBaseFileInfo(unsuitableFiles);
            }

            return false;
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
                    NLogger.Info($"\t\tThere are no imported methods from other assemblies in the file.");
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

        public static void LoggingBinariesExistsStatus(IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            foreach (var item in binaryLocations.Select((Location, i) => (Location, i)))
            {
                if (BinarySearcher.CheckBinaryExist(item.Location, analysisFolder, defaultBinaryExtension) is BinarySearcherStatusCodes.BinaryNotExists)
                {
                    NLogger.Error($"\t[{item.i}]{item.Location}: NOT exist!");
                }
                else
                {
                    NLogger.Info($"\t[{item.i}]{item.Location}: exist.");
                }
            }
        }

        public static void LoggingBinaryExistsAndNonExistsCount(IEnumerable<string> binaryLocations, string analysisFolder)
        {
            NLogger.Info($"Total: {binaryLocations.GetExistsBinaries(analysisFolder).Count()} exists, {binaryLocations.GetNotExistsBinaries(analysisFolder).Count()} not exists.");
        }
    }
}
