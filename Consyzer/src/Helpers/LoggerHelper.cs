using System.Linq;
using System.Collections.Generic;
using Consyzer.Logger;
using Consyzer.AnalyzerEngine.Analyzers;
using Consyzer.AnalyzerEngine.Analyzers.Searchers;
using Consyzer.AnalyzerEngine.Analyzers.SyntaxModels;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class LoggerHelper
    {

        public static void LoggingBaseFileInfo(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($"\t[{item.Index}]Name: {item.File.BaseFileInfo.Name}, Creation Time: {item.File.BaseFileInfo.CreationTime}.");
            }
        }

        public static void LoggingBaseAndHashFileInfo(this IEnumerable<MetadataAnalyzer> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($"\t[{item.Index}]Name: {item.File.BinaryInfo.BaseFileInfo.Name}, Creation Time: {item.File.BinaryInfo.BaseFileInfo.CreationTime}, " +
                    $"SHA256 Hash Sum: {item.File.BinaryInfo.HashInfo.SHA256Sum}.");
            }
        }

        public static void LoggingImportedMethodsInfoForEachBinary(this IEnumerable<MetadataAnalyzer> metadataAnalyzers)
        {
            foreach (var item in metadataAnalyzers.Select((File, i) => (File, i)))
            {
                var importedMethods = item.File.GetImportedMethodsInfo().ToList();

                NLogger.Info($"\t[{item.i}]File {item.File.BinaryInfo.BaseFileInfo.FullName}: ");
                if (importedMethods.Any())
                {
                    importedMethods.LoggingImportedMethodsInfo();
                }
                else
                {
                    NLogger.Info($"\t\tThere are no imported methods from other assemblies in the file.");
                }
            }
        }

        public static void LoggingImportedMethodsInfo(this IEnumerable<ImportedMethodInfo> importedMethods)
        {
            foreach (var import in importedMethods.Select((Signature, i) => (Signature, i)))
            {
                NLogger.Info($"\t\t[Method[{import.i}]]:");
                NLogger.Info($"\t\t\tMethod Location: {import.Signature.SignatureInfo.GetMethodLocation()}");
                NLogger.Info($"\t\t\tMethod Signature: {import.Signature.SignatureInfo.GetBaseMethodSignature()}");
                NLogger.Info($"\t\t\tDLL Location: {import.Signature.DllLocation}");
                NLogger.Info($"\t\t\tDLL Import Arguments: {import.Signature.DllImportArguments}");
            }
        }

        public static void LoggingBinariesExistsStatus(this IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
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
    }
}
