using System;
using System.IO;
using Consyzer.AnalyzerEngine.Helpers;
using Consyzer.AnalyzerEngine.Analyzer.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Analyzer.Searchers
{
    public static class BinarySearcher
    {
        public static BinarySearcherStatusCodes CheckBinaryExists(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException($"{nameof(path)} is null or empty");
            }

            bool pathIsExists = File.Exists(path);
            bool pathIsAbsolute = IOHelper.IsAbsolutePath(path);

            if (pathIsExists)
            {
                return pathIsAbsolute ? BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath : BinarySearcherStatusCodes.BinaryExistsOnSourcePath;
            }
            else
            {
                return BinarySearcherStatusCodes.BinaryNotExists;
            }
        }

        public static BinarySearcherStatusCodes CheckBinaryExists(ImportedMethodInfo importedMethod)
        {
            if (importedMethod is null)
            {
                throw new ArgumentNullException($"{nameof(importedMethod)} is null");
            }

            return BinarySearcher.CheckBinaryExists(importedMethod.DllLocation);
        }
    }
}
