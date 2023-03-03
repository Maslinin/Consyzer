using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.File;
using Consyzer.Metadata;
using Consyzer.Cryptography;
using Consyzer.Metadata.Models;
using Log = Consyzer.Logging.LogService;

namespace Consyzer.Logging
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class AnalysisStatusLogger
    {
        public static void LogAnalysisParams(string analysisDirectory, IEnumerable<string> fileExtensions)
        {
            string separatedExtensions = string.Join(", ", fileExtensions.Select(e => $"'{e}'"));
            Log.Debug($"Path for analysis: '{analysisDirectory}'.");
            Log.Debug($"Specified file extensions for analysis: {separatedExtensions}.");
        }

        public static void LogBaseFileInfo(IEnumerable<FileInfo> binaryFiles)
        {
            foreach ((FileInfo file, int index) in binaryFiles.Select((f, i) => (f, i)))
            {
                Log.Info($"\t[{index}]File: '{file.Name}':");
                Log.Info($"\t\tCreation Time: '{file.CreationTime}'.");
            }
        }

        public static void LogBaseAndHashFileInfo(IEnumerable<MetadataAnalyzer> fileInfos)
        {
            foreach ((MetadataAnalyzer file, int index) in fileInfos.Select((f, i) => (f, i)))
            {
                var hashInfo = FileHashInfo.CalculateHash(file.FileInfo);

                Log.Info($"\t[{index}]File: '{file.FileInfo.Name}':");
                Log.Info($"\t\tCreation Time: '{file.FileInfo.CreationTime}',");
                Log.Info($"\t\tSHA256 Hash Sum: '{hashInfo.SHA256Sum}'.");
            }
        }

        public static void LogImportedMethodsInfoForEachFile(IEnumerable<MetadataAnalyzer> metadataAnalyzers)
        {
            foreach ((MetadataAnalyzer file, int index) in metadataAnalyzers.Select((f, i) => (f, i)))
            {
                var importedMethods = file.GetImportedMethodsInfo();

                Log.Info($"\t[{index}]File '{file.FileInfo.FullName}': ");
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
            foreach ((ImportedMethodInfo info, int index) in importedMethods.Select((m, i) => (m, i)))
            {
                Log.Info($"\t\t[{index}]Method '{info.Signature.MethodLocation}':");
                Log.Info($"\t\t\tMethod Signature: '{info.Signature.BaseMethodSignature}',");
                Log.Info($"\t\t\tDLL Location: '{info.DllLocation}',");
                Log.Info($"\t\t\tDLL Import Args: '{info.DllImportArgs}'.");
            }
        }

        public static void LogFilesExistStatus(FileExistenceChecker fileSearcher, IEnumerable<string> binaryLocations)
        {
            int existingFiles = 0, nonExistingFiles = 0;

            foreach ((string location, int index) in binaryLocations.Select((l, i) => (l, i)))
            {
                bool notExists = fileSearcher.GetMinFileExistanceStatus(location) is FileExistenceStatus.FileDoesNotExist;
                if (notExists)
                {
                    Log.Error($"\t[{index}]File '{location}' does NOT exist!");
                    ++nonExistingFiles;
                }
                else
                {
                    Log.Info($"\t[{index}]File '{location}' exists.");
                    ++existingFiles;
                }
            }

            Log.Info($"TOTAL: {existingFiles} exists, {nonExistingFiles} does not exist.");
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
            var differentFiles = notMetadataFiles.Except(notMetadataAssemblyFiles, new FileInfoEqualityComparer());
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
