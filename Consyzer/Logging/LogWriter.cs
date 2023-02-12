using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.Helpers;
using Consyzer.Metadata;
using Consyzer.Metadata.Models;
using Consyzer.Cryptography;
using Log = Consyzer.Logging.NLogger;
using static Consyzer.Constants;

namespace Consyzer.Logging
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class LogWriter
    {
        public static void LogAnalysisParams(string analysisDirectory, IEnumerable<string> fileExtensions)
        {
            string separatedExtensions = string.Join(", ", fileExtensions.Select(e => $"'{e}'"));
            Log.Debug($"Path for analysis: '{analysisDirectory}'.");
            Log.Debug($"Specified binary file extensions for analysis: {separatedExtensions}.");
        }

        public static void LogBaseFileInfo(IEnumerable<FileInfo> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                Log.Info($"\t[{item.Index}]File: '{item.File.Name}':");
                Log.Info($"\t\tCreation Time: '{item.File.CreationTime}'.");
            }
        }

        public static void LogBaseAndHashFileInfo(IEnumerable<MetadataAnalyzer> fileInfos)
        {
            foreach (var item in fileInfos.Select((File, Index) => (File, Index)))
            {
                var hashInfo = FileHashInfo.CalculateHash(item.File.FileInfo);

                Log.Info($"\t[{item.Index}]File: '{item.File.FileInfo.Name}':");
                Log.Info($"\t\tCreation Time: '{item.File.FileInfo.CreationTime}',");
                Log.Info($"\t\tSHA256 Hash Sum: '{hashInfo.SHA256Sum}'.");
            }
        }

        public static void LogImportedMethodsInfoForEachFile(IEnumerable<MetadataAnalyzer> metadataAnalyzers)
        {
            foreach (var item in metadataAnalyzers.Select((File, i) => (File, i)))
            {
                var importedMethods = item.File.GetImportedMethodsInfo().ToList();

                Log.Info($"\t[{item.i}]File '{item.File.FileInfo.FullName}': ");
                if (importedMethods.Any())
                {
                    LogImportedMethodsInfo(importedMethods);
                }
                else
                {
                    Log.Info($"\t\tThere is no any imported methods from other assemblies in the file.");
                }
            }
        }

        public static void LogImportedMethodsInfo(IEnumerable<ImportedMethodInfo> importedMethods)
        {
            foreach (var import in importedMethods.Select((Info, I) => (Info, I)))
            {
                Log.Info($"\t\t[{import.I}]Method '{import.Info.Signature.GetMethodLocation()}':");
                Log.Info($"\t\t\tMethod Signature: '{import.Info.Signature.GetBaseMethodSignature()}',");
                Log.Info($"\t\t\tDLL Location: '{import.Info.DllLocation}',");
                Log.Info($"\t\t\tDLL Import Args: '{import.Info.DllImportArgs}'.");
            }
        }

        public static void LogFilesExistStatus(FileSearcher fileSearcher, IEnumerable<string> binaryLocations, string defaultBinaryExtension = ".dll")
        {
            int existingFiles = 0, nonExistingFiles = 0;

            foreach (var item in binaryLocations.Select((Location, i) => (Location, i)))
            {
                bool status = fileSearcher.GetMinFileExistanceStatusCode(item.Location, defaultBinaryExtension) is FileExistanceStatusCode.FileDoesNotExists;
                if (!status)
                {
                    Log.Info($"\t[{item.i}]File '{item.Location}' exists.");
                    ++existingFiles;
                }
                else
                {
                    Log.Error($"\t[{item.i}]File '{item.Location}' does NOT exist!");
                    ++nonExistingFiles;
                }
            }

            Log.Info($"{existingFiles} exists, {nonExistingFiles} does not exist.");
        }

        public static bool CheckAndLogCorrectFiles(IEnumerable<FileInfo> files)
        {
            var notMetadataFiles = MetadataFileFilter.GetNotMetadataFiles(files);
            if (notMetadataFiles.Any())
            {
                Log.Info("The following files were excluded from analysis because they DO NOT contain metadata:");
                LogBaseFileInfo(notMetadataFiles);

                if (files.Count() == notMetadataFiles.Count())
                {
                    Log.Warn("All found files do NOT contain metadata.");
                    return false;
                }
            }

            var notMetadataAssemblyFiles = MetadataFileFilter.GetNotMetadataAssemblyFiles(files);
            var differentFiles = notMetadataFiles.Where(f => !notMetadataAssemblyFiles.Select(f => f.FullName).Contains(f.FullName));
            if (differentFiles.Any())
            {
                Log.Info("The following files were excluded from analysis because they are NOT assembly files:");
                LogBaseFileInfo(differentFiles);

                if (files.Count() == differentFiles.Count())
                {
                    Log.Warn("All found files contain metadata, but are NOT assembly files.");
                    return false;
                }
            }

            return true;
        }

    }
}
