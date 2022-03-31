using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.Logger;
using Consyzer.AnalyzerEngine.Helpers;
using Consyzer.AnalyzerEngine.Analyzer;
using Consyzer.AnalyzerEngine.Analyzer.Searchers;
using Consyzer.AnalyzerEngine.Analyzer.SyntaxModels;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.Helpers
{
    public static class LoggerHelper
    {

        #region LoggingBaseFileInfo

        public static void LoggingBaseFileInfo(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($"[{item.Index}]Name: {item.File.BaseFileInfo.Name}, Creation Time: {item.File.BaseFileInfo.CreationTime}.");
            }
        }

        public static void LoggingBaseFileInfo(this IEnumerable<MetadataAnalyzer> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($"[{item.Index}]Name: {item.File.BinaryInfo.BaseFileInfo.Name}, Creation Time: {item.File.BinaryInfo.BaseFileInfo.CreationTime}.");
            }
        }

        #endregion

        #region LoggingBaseAndHashFileInfo

        public static void LoggingBaseAndHashFileInfo(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($@"[{item.Index}]Name: {item.File.BaseFileInfo.Name}, Creation Time: {item.File.BaseFileInfo.CreationTime},
                            SHA256HashSum: {item.File.HashInfo.SHA256Sum}.");
            }
        }

        public static void LoggingBaseAndHashFileInfo(this IEnumerable<MetadataAnalyzer> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($@"[{item.Index}]Name: {item.File.BinaryInfo.BaseFileInfo.Name}, Creation Time: {item.File.BinaryInfo.BaseFileInfo.CreationTime},
                            SHA256HashSum: {item.File.BinaryInfo.HashInfo.SHA256Sum}.");
            }
        }

        #endregion

        public static void LoggingImportedMethodsInfo(this IEnumerable<ImportedMethodInfo> importedMethods)
        {
            foreach (var import in importedMethods.Select((Signature, i) => (Signature, i)))
            {
                NLogger.Info($"[Method[{import.i}]]:");
                NLogger.Info($"\tMethod Location: {import.Signature.SignatureInfo.GetMethodLocation()}");
                NLogger.Info($"\tMethod Signature: {import.Signature.SignatureInfo.GetBaseMethodSignature()}");
                NLogger.Info($"\tDLL Location: {import.Signature.DllLocation}");
                NLogger.Info($"\tDLL Import Arguments: {import.Signature.DllImportArguments}");
            }
        }

        public static void LoggingImportedMethodsInfoForEachBinary(this IEnumerable<MetadataAnalyzer> metadataAnalyzers)
        {
            foreach (var item in metadataAnalyzers.Select((File, i) => (File, i)))
            {
                var importedMethods = item.File.GetImportedMethodsInfo().ToList();

                NLogger.Info($"[{item.i}]File {item.File.BinaryInfo.BaseFileInfo.FullName}: ");
                importedMethods.LoggingImportedMethodsInfo();
            }
        }

        public static void LoggingBinariesExistsStatus(this IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            foreach (var item in binaryLocations.Select((Location, i) => (Location, i)))
            {
                if (BinarySearcher.CheckBinaryExist(item.Location, analysisFolder, defaultBinaryExtension) is BinarySearcherStatusCodes.BinaryNotExists)
                {
                    NLogger.Error($"[{item.i}]{item.Location}: NOT exist!");
                }
                else
                {
                    NLogger.Info($"[{item.i}]{item.Location}: exist.");
                }
            }
        }
    }
}
