using System.IO;
using System.Linq;
using System.Collections.Generic;
using Log = Consyzer.Logging.NLogger;
using Consyzer.Helpers;
using Consyzer.Searchers;
using Consyzer.Cryptography;
using Consyzer.AnalyzerEngine.Analyzers;
using Consyzer.AnalyzerEngine.Analyzers.Models;

namespace Consyzer.Logging
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class LoggerHelper
    {
        public static void LoggingPathToBinariesForAnalysis(string analysisFolder)
        {
            Log.Info($"Path for analysis: '{analysisFolder}'.");
        }

        public static void LoggingFilesExtensionsForAnalysis(IEnumerable<string> filesExtensions)
        {
            Log.Info($"Specified binary file extensions for analysis: {string.Join(", ", filesExtensions.Select(e => $"'{e}'"))}.");
        }


        public static void LoggingBaseFileInfo(IEnumerable<FileInfo> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                Log.Info($"\t[{item.Index}]File: '{item.File.Name}', Creation Time: '{item.File.CreationTime}'.");
            }
        }

        public static void LoggingBaseAndHashFileInfo(IEnumerable<MetadataAnalyzer> fileInfos)
        {
            foreach (var item in fileInfos.Select((File, Index) => (File, Index)))
            {
                var hashInfo = FileHashInfo.CalculateHash(item.File.FileInfo);
                Log.Info($"\t[{item.Index}]File: '{item.File.FileInfo.Name}', Creation Time: '{item.File.FileInfo.CreationTime}', " +
                    $"SHA256 Hash Sum: '{hashInfo.SHA256Sum}'.");
            }
        }

        public static void LoggingImportedMethodsInfoForEachBinary(IEnumerable<ImportedMethodsAnalyzer> metadataAnalyzers)
        {
            foreach (var item in metadataAnalyzers.Select((File, i) => (File, i)))
            {
                var importedMethods = item.File.GetImportedMethodsInfo().ToList();

                Log.Info($"\t[{item.i}]File '{item.File.FileInfo.FullName}': ");
                if (importedMethods.Any())
                {
                    LoggingImportedMethodsInfo(importedMethods);
                }
                else
                {
                    Log.Info($"\t\tThere is no any imported methods from other assemblies in the file.");
                }
            }
        }

        public static void LoggingImportedMethodsInfo(IEnumerable<ImportedMethodInfo> importedMethods)
        {
            foreach (var import in importedMethods.Select((Signature, i) => (Signature, i)))
            {
                Log.Info($"\t\t[{import.i}]Method '{import.Signature.SignatureInfo.GetMethodLocation()}':");
                Log.Info($"\t\t\tMethod Signature: '{import.Signature.SignatureInfo.GetBaseMethodSignature()}',");
                Log.Info($"\t\t\tDLL Location: '{import.Signature.DllLocation}',");
                Log.Info($"\t\t\tDLL Import Arguments: '{import.Signature.DllImportArguments}'.");
            }
        }

        public static void LoggingBinariesExistStatus(IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            var fileSearcher = new FileSearcher(analysisFolder);

            foreach (var item in binaryLocations.Select((Location, i) => (Location, i)))
            {
                if (fileSearcher.GetFileLocation(item.Location, defaultBinaryExtension) is FileExistsStatusCodes.FileNotExists)
                {
                    Log.Error($"\t[{item.i}]File '{item.Location}' does NOT exist!");
                }
                else
                {
                    Log.Info($"\t[{item.i}]File '{item.Location}' exist.");
                }
            }
        }

        public static void LoggingExistAndNonExistBinariesCount(IEnumerable<string> binaryLocations, string analysisFolder)
        {
            Log.Info($"Total: {AnalyzerHelper.GetExistsBinaries(binaryLocations, analysisFolder).Count()} exist, " +
                $"{AnalyzerHelper.GetNotExistsBinaries(binaryLocations, analysisFolder).Count()} do not exist.");
        }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class LoggerCheckerHelper
    {
        public static bool CheckAndLoggingFilesCorrect(IEnumerable<FileInfo> binaryFiles)
        {
            var notMetadataFiles = MetadataFilter.GetNotMetadataFiles(binaryFiles);

            if (notMetadataFiles.Any())
            {
                Log.Info("The following files were excluded from analysis because they DO NOT contain metadata:");
                LoggerHelper.LoggingBaseFileInfo(notMetadataFiles);

                if (binaryFiles.Count() == notMetadataFiles.Count())
                {
                    Log.Warn("All found files do NOT contain metadata.");
                    return false;
                }
            }

            var notMetadataAssemblyFiles = MetadataFilter.GetNotMetadataAssemblyFiles(binaryFiles);
            var differentFiles = notMetadataFiles.Where(f => !notMetadataAssemblyFiles.Select(f => f.FullName).Contains(f.FullName));
            if (differentFiles.Any())
            {
                Log.Info("The following files were excluded from analysis because they are NOT assembly files:");
                LoggerHelper.LoggingBaseFileInfo(differentFiles);

                if (binaryFiles.Count() == differentFiles.Count())
                {
                    Log.Warn("All found files contain metadata, but are NOT assembly files.");
                    return false;
                }
            }

            return true;
        }

        public static bool CheckAndLoggingBinaryFilesExist(IEnumerable<FileInfo> binaryFiles)
        {
            if (binaryFiles.Any())
                return true;

            Log.Warn("Binary files for analysis with the specified extensions were not found.");
            return false;
        }

        public static bool CheckAndLoggingAnyBinariesExist(IEnumerable<string> binaryLocations)
        {
            if (binaryLocations.Any())
                return true;

            Log.Info("All files are missing imported methods from other assemblies.");
            return false;
        }
    }
}
