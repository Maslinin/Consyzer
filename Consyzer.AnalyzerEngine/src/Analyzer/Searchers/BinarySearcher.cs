using System;
using System.IO;
using Consyzer.AnalyzerEngine.Helpers;
using Consyzer.AnalyzerEngine.Analyzer.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Analyzer.Searchers
{
    public static class BinarySearcher
    {
        public static BinarySearcherStatusCodes CheckBinaryExist(string binaryPath, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            if (string.IsNullOrEmpty(binaryPath))
            {
                throw new ArgumentNullException($"{nameof(binaryPath)} is null or empty.");
            }
            if (string.IsNullOrEmpty(analysisFolder))
            {
                throw new ArgumentNullException($"{nameof(analysisFolder)} is null or empty.");
            }

            bool pathIsAbsolute = IOHelper.IsAbsolutePath(binaryPath);
            if (!pathIsAbsolute)
            {
                binaryPath = Path.Combine(analysisFolder, binaryPath);
            }
            if (!Path.HasExtension(binaryPath))
            {
                binaryPath = $"{binaryPath}{defaultBinaryExtension}";
            }

            bool pathIsExists = File.Exists(binaryPath);
            if (pathIsExists)
            {
                return pathIsAbsolute ? BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath : BinarySearcherStatusCodes.BinaryExistsOnSourcePath;
            }
            else
            {
                return BinarySearcherStatusCodes.BinaryNotExists;
            }
        }

        public static BinarySearcherStatusCodes CheckBinaryExist(ImportedMethodInfo importedMethod, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            if (importedMethod is null)
            {
                throw new ArgumentNullException($"{nameof(importedMethod)} is null");
            }

            return BinarySearcher.CheckBinaryExist(importedMethod.DllLocation, analysisFolder, defaultBinaryExtension);
        }
    }
}
