using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Consyzer.File;
using Consyzer.Metadata;
using Consyzer.Cryptography;
using Consyzer.Metadata.Models;

namespace Consyzer.Logging
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class AnalysisStatusLogger
    {
        public static string GetAnalysisParamsLog(string analysisDirectory, IEnumerable<string> fileExtensions)
        {
            string separatedExtensions = string.Join(", ", fileExtensions.Select(e => $"'{e}'"));
            return $"Path for analysis: '{analysisDirectory}'.\nSpecified file extensions for analysis: {separatedExtensions}.";
        }

        public static string GetNotCorrectFilesLog(IEnumerable<FileInfo> files, out bool filesAreIncorrect)
        {
            var log = new StringBuilder();

            var notMetadataFiles = MetadataFileFilter.GetNotMetadataFiles(files);
            if (notMetadataFiles.Any())
            {
                log.AppendLine("The following files were excluded from analysis because they DO NOT contain metadata:");
                log.AppendLine(GetBaseFileInfoLog(notMetadataFiles));

                if (files.Count() == notMetadataFiles.Count())
                {
                    log.AppendLine("All found files DO NOT contain metadata.");
                    filesAreIncorrect = true;
                    return log.ToString().TrimEnd('\n', '\r');
                }
            }

            var notMetadataAssemblyFiles = MetadataFileFilter.GetNotMetadataAssemblyFiles(files);
            var differentFiles = notMetadataFiles.Where(f => !notMetadataAssemblyFiles.Any(a => a.Name == f.Name));
            if (differentFiles.Any())
            {
                log.AppendLine("The following files were excluded from analysis because they ARE NOT assembly files:");
                log.AppendLine(GetBaseFileInfoLog(differentFiles));

                if (files.Count() == differentFiles.Count())
                {
                    log.AppendLine("All found files contain metadata, but ARE NOT assembly files.");
                    filesAreIncorrect = true;
                    return log.ToString().TrimEnd('\n', '\r');
                }
            }

            filesAreIncorrect = false;
            return log.ToString().TrimEnd('\n', '\r');
        }

        public static string GetBaseFileInfoLog(IEnumerable<FileInfo> files)
        {
            var log = new StringBuilder();
            foreach ((FileInfo file, int index) in files.Select((f, i) => (f, i)))
            {
                var hashInfo = FileHashInfo.CalculateHash(file);

                log.AppendLine($"\t[{index}]File: '{file.Name}':");
                log.AppendLine($"\t\tCreation Time: '{file.CreationTime}',");
            }
            return log.ToString().TrimEnd('\n', '\r');
        }

        public static string GetBaseAndHashFileInfoLog(IEnumerable<FileInfo> files)
        {
            var log = new StringBuilder();
            foreach ((FileInfo file, int index) in files.Select((f, i) => (f, i)))
            {
                var hashInfo = FileHashInfo.CalculateHash(file);

                log.AppendLine($"\t[{index}]File: '{file.Name}':");
                log.AppendLine($"\t\tCreation Time: '{file.CreationTime}',");
                log.AppendLine($"\t\tSHA256 Hash Sum: '{hashInfo.SHA256Sum}'.");
            }
            return log.ToString().TrimEnd('\n', '\r');
        }

        public static string GetImportedMethodsInfoForEachFileLog(IEnumerable<MetadataAnalyzer> metadataAnalyzers)
        {
            var log = new StringBuilder();
            foreach ((MetadataAnalyzer file, int index) in metadataAnalyzers.Select((f, i) => (f, i)))
            {
                var importedMethods = file.GetImportedMethodsInfo();

                log.AppendLine($"\t[{index}]File '{file.FileInfo.FullName}': ");
                if (importedMethods.Any())
                {
                    log.AppendLine(GetImportedMethodsInfoLog(importedMethods));
                }
                else
                {
                    log.AppendLine($"\t\tThere is no any imported methods from other assemblies in the file.");
                }
            }
            return log.ToString().TrimEnd('\n', '\r');
        }

        public static string GetImportedMethodsInfoLog(IEnumerable<ImportedMethodInfo> importedMethods)
        {
            var log = new StringBuilder();
            foreach ((ImportedMethodInfo info, int index) in importedMethods.Select((m, i) => (m, i)))
            {
                log.AppendLine($"\t\t[{index}]Method '{info.Signature.MethodLocation}':");
                log.AppendLine($"\t\t\tMethod Signature: '{info.Signature.BaseMethodSignature}',");
                log.AppendLine($"\t\t\tDLL Location: '{info.DllLocation}',");
                log.AppendLine($"\t\t\tDLL Import Args: '{info.DllImportArgs}'.");
            }
            return log.ToString().TrimEnd('\n', '\r');
        }

        public static string GetFilesExistStatusLog(FileExistenceChecker fileSearcher, IEnumerable<string> fileLocations)
        {
            int existingFiles = 0, nonExistingFiles = 0;

            var log = new StringBuilder();
            foreach ((string location, int index) in fileLocations.Select((l, i) => (l, i)))
            {
                bool notExists = fileSearcher.GetMinFileExistanceStatus(location) is FileExistenceStatus.FileDoesNotExist;
                if (notExists)
                {
                    log.AppendLine($"\t[{index}]File '{location}' DOES NOT exist!");
                    ++nonExistingFiles;
                }
                else
                {
                    log.AppendLine($"\t[{index}]File '{location}' exists.");
                    ++existingFiles;
                }
            }
            log.Append($"TOTAL: {existingFiles} exists, {nonExistingFiles} DO NOT exist.");

            return log.ToString().TrimEnd('\n', '\r');
        }

    }
}
