using System;
using System.IO;
using Consyzer.AnalyzerEngine.Helpers;
using Consyzer.AnalyzerEngine.Analyzers.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Analyzers.Searchers
{
    /// <summary>
    /// [Static] Provides tools for searching binary files.
    /// </summary>
    public static class BinarySearcher
    {
        /// <summary>
        /// Checks if binary file exists on the specified path relative to the specified directory.<br/>
        /// 0 if the binary file <b>exists</b> in the specified folder;<br/>
        /// 1 if the binary file is <b>on an absolute path</b>;<br/>
        /// 2 if the binary file <b>does not exist</b>.
        /// </summary>
        /// <param name="binaryPath"></param>
        /// <param name="analysisFolder"></param>
        /// <param name="defaultBinaryExtension"></param>
        /// <returns>Binary existence status code.</returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Checks binary file exists on the specified path relative to the specified directory.<br/>
        /// 0 if the binary file <b>exists</b> in the specified folder;<br/>
        /// 1 if the binary file is <b>on an absolute path</b>;<br/>
        /// 2 if the binary file <b>does not exist</b>.
        /// </summary>
        /// <param name="importedMethod"></param>
        /// <param name="analysisFolder"></param>
        /// <param name="defaultBinaryExtension"></param>
        /// <returns>Binary existence status code.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
