using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Consyzer.Extractors;
using Consyzer.Cryptography;
using Consyzer.Extractors.Models;

namespace Consyzer.Logging
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class AnalysisStatusLogger
    {
        public static string GetAnalysisParamsLog(string analysisDirectory, IEnumerable<string> fileExtensions)
        {
            string separatedExtensions = string.Join(", ", fileExtensions.Select(e => $"'{e}'"));
            return 
                $"Path for analysis: '{analysisDirectory}'." +
                $"\nSpecified file extensions for analysis: {separatedExtensions}.";
        }

        public static string GetNotMetadataFilesLog(IEnumerable<FileInfo> files, out bool filesAreIncorrect)
        {
            var log = new StringBuilder();

            var notMetadataFiles = MetadataFileFilter.GetNonMetadataFiles(files);
            if (notMetadataFiles.Any())
            {
                log.AppendLine("The following files were excluded from analysis because they DO NOT contain metadata:");
                log.AppendLine(GetBaseFileInfoLog(notMetadataFiles));
            }

            filesAreIncorrect = notMetadataFiles.Count() == files.Count();
            if (filesAreIncorrect)
            {
                log.AppendLine("All found files DO NOT contain metadata.");
            }
            else
            {
                log.AppendLine("All found files contain metadata.");
            }

            return log.ToString().TrimEnd('\n', '\r');
        }

        public static string GetNotMetadataAssemblyFilesLog(IEnumerable<FileInfo> files, out bool filesAreIncorrect)
        {
            var log = new StringBuilder();

            var notMetadataFiles = MetadataFileFilter.GetNonMetadataFiles(files);
            var notMetadataAssemblyFiles = MetadataFileFilter.GetNonMetadataAssemblyFiles(files);
            var remainingFiles = notMetadataFiles.Where(f => !notMetadataAssemblyFiles.Any(a => a.Name == f.Name));

            if (remainingFiles.Any())
            {
                log.AppendLine("The following files were excluded from analysis because they ARE NOT assembly files:");
                log.AppendLine(GetBaseFileInfoLog(remainingFiles));
            }

            filesAreIncorrect = remainingFiles.Count() == files.Count();
            if (filesAreIncorrect)
            {
                log.AppendLine("All found files contain metadata, but ARE NOT assembly files.");
            }
            else
            {
                log.AppendLine("All metadata files found are assemblies.");
            }

            return log.ToString().TrimEnd('\n', '\r');
        }

        public static string GetBaseFileInfoLog(IEnumerable<FileInfo> files)
        {
            return string.Join(Environment.NewLine, files.Select((f, i) =>
            {
                return 
                    $"\t[{i}]File: '{f.Name}': {Environment.NewLine}" +
                    $"\t\tCreation Time: '{f.CreationTime}',";
            }));
        }

        public static string GetBaseAndHashFileInfoLog(IEnumerable<FileInfo> files)
        {
            return string.Join(Environment.NewLine, files.Select((f, i) =>
            {
                var hashInfo = FileHashInfo.CalculateHash(f);
                return 
                    $"\t[{i}]File: '{f.Name}': {Environment.NewLine}" +
                    $"\t\tCreation Time: '{f.CreationTime}', {Environment.NewLine}" +
                    $"\t\tSHA256 Hash Sum: '{hashInfo.SHA256Sum}'.";
            }));
        }

        public static string GetImportedMethodsInfoForEachFileLog(IEnumerable<ImportedMethodExtractor> metadataAnalyzers)
        {
            return string.Join(Environment.NewLine, metadataAnalyzers.Select((file, index) =>
            {
                var importedMethods = file.GetImportedMethodsInfo();
                var methodInfoText = importedMethods.Any()
                    ? GetImportedMethodsInfoLog(importedMethods)
                    : "\t\tThere is no any imported methods from other assemblies in the file.";

                return $"\t[{index}]File '{file.FileInfo.FullName}': {Environment.NewLine}{methodInfoText}";
            }));
        }

        public static string GetImportedMethodsInfoLog(IEnumerable<ImportedMethodInfo> importedMethods)
        {
            return string.Join(Environment.NewLine, importedMethods.Select((info, index) =>
                $"\t\t[{index}]Method '{info.Signature.MethodLocation}': {Environment.NewLine}" +
                $"\t\t\tMethod Signature: '{info.Signature.BaseMethodSignature}', {Environment.NewLine}" +
                $"\t\t\tDLL Location: '{info.DllLocation}', {Environment.NewLine}" +
                $"\t\t\tDLL Import Args: '{info.DllImportArgs}'."));
        }

        public static string GetFilesExistStatusLog(FileExistenceChecker fileSearcher, IEnumerable<string> fileLocations)
        {
            var fileStatuses = fileLocations
                .Select((location, index) => (Index: index, 
                Location: location, 
                Status: fileSearcher.GetMinFileExistanceStatus(location)));

            var existingFiles = fileStatuses.Count(x => x.Status != FileExistenceStatus.FileDoesNotExist);
            var nonExistingFiles = fileStatuses.Count(x => x.Status == FileExistenceStatus.FileDoesNotExist);

            var log = new StringBuilder();
            foreach (var fileStatus in fileStatuses)
            {
                var statusText = fileStatus.Status != FileExistenceStatus.FileDoesNotExist ? "exists" : "DOES NOT exist";
                log.AppendLine($"\t[{fileStatus.Index}]File '{fileStatus.Location}' {statusText}.");
            }
            log.Append($"TOTAL: {existingFiles} exists, {nonExistingFiles} DO NOT exist.");

            return log.ToString();
        }

    }
}
