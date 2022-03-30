using System;
using System.Linq;
using System.Collections.Generic;
using Consyzer.Logger;
using Consyzer.AnalyzerEngine.Analyzer;
using Consyzer.AnalyzerEngine.Analyzer.Searchers;
using Consyzer.AnalyzerEngine.Analyzer.SyntaxModels;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.Helpers
{
    public static class DisplayHelper
    {
        #region DisplayBaseFileInfo

        public static void DisplayBaseFileInfo(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($"[{item.Index}]Name: {item.File.BaseFileInfo.Name}, Creation Time: {item.File.BaseFileInfo.CreationTime}.");
            }
        }

        public static void DisplayBaseFileInfo(this IEnumerable<MetadataAnalyzer> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($"[{item.Index}]Name: {item.File.BinaryInfo.BaseFileInfo.Name}, Creation Time: {item.File.BinaryInfo.BaseFileInfo.CreationTime}.");
            }
        }

        #endregion

        #region DisplayBaseAndHashFileInfo

        public static void DisplayBaseAndHashFileInfo(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($@"[{item.Index}]Name: {item.File.BaseFileInfo.Name}, Creation Time: {item.File.BaseFileInfo.CreationTime},
                            SHA256HashSum: {item.File.HashInfo.SHA256Sum}.");
            }
        }

        public static void DisplayBaseAndHashFileInfo(this IEnumerable<MetadataAnalyzer> binaryFiles)
        {
            foreach (var item in binaryFiles.Select((File, Index) => (File, Index)))
            {
                NLogger.Info($@"[{item.Index}]Name: {item.File.BinaryInfo.BaseFileInfo.Name}, Creation Time: {item.File.BinaryInfo.BaseFileInfo.CreationTime},
                            SHA256HashSum: {item.File.BinaryInfo.HashInfo.SHA256Sum}.");
            }
        }

        #endregion

        public static void DisplayImportedMethodsInfo(this IEnumerable<ImportedMethodInfo> importedMethods)
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

        public static void DisplayImportedMethodsInfoForEachBinary(this IEnumerable<MetadataAnalyzer> metadataAnalyzers)
        {
            foreach (var item in metadataAnalyzers.Select((File, i) => (File, i)))
            {
                var importedMethods = item.File.GetImportedMethodsInfo().ToList();

                NLogger.Info($"[{item.i}]File {item.File.BinaryInfo.BaseFileInfo.FullName}: ");
                importedMethods.DisplayImportedMethodsInfo();
            }
        }

        //public static void DisplayBinaryExistStatus(string dllLocation, BinarySearcherStatusCodes binarySearcherStatus)
        //{
        //    int enumLength = Enum.GetValues(typeof(BinarySearcherStatusCodes)).Length;
        //    BinarySearcherStatusCodes last = (BinarySearcherStatusCodes)Enum.GetValues(typeof(BinarySearcherStatusCodes)).GetValue(enumLength - 1);

        //    if(binarySearcherStatus.Equals(last))
        //    {

        //    }
        //}
    }
}
