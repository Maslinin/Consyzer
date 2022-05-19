using System;
using System.IO;
using System.Runtime.InteropServices;
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
        /// <param name="pathToBinary"></param>
        /// <param name="analysisFolder"></param>
        /// <param name="defaultBinaryExtension"></param>
        /// <returns>Binary existence status code.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static BinarySearcherStatusCodes CheckBinaryExist(string pathToBinary, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            if (string.IsNullOrEmpty(pathToBinary))
            {
                throw new ArgumentNullException($"{nameof(pathToBinary)} is null or empty.");
            }
            if (string.IsNullOrEmpty(analysisFolder))
            {
                throw new ArgumentNullException($"{nameof(analysisFolder)} is null or empty.");
            }

            bool pathIsAbsolute = IOHelper.IsAbsolutePath(pathToBinary);
            if (!pathIsAbsolute)
            {
                pathToBinary = Path.Combine(analysisFolder, pathToBinary);
            }
            if (!Path.HasExtension(pathToBinary))
            {
                pathToBinary = $"{pathToBinary}{defaultBinaryExtension}";
            }

            bool pathIsExist = File.Exists(pathToBinary);
            if (pathIsExist)
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
                throw new ArgumentNullException($"{nameof(importedMethod)} is null.");
            }
            if(string.IsNullOrEmpty(analysisFolder))
            {
                throw new ArgumentNullException($"{nameof(analysisFolder)} is null or empty.");
            }

            return BinarySearcher.CheckBinaryExist(importedMethod.DllLocation, analysisFolder, defaultBinaryExtension);
        }

        /// <summary>
        /// Checks if the specified binary exists in the <b>system folder</b> <b>(windows only)</b>.<br/>
        /// 1 if the specified binary <b>exists</b> in the Windows <b>system folder</b>;<br/>
        /// 2 if it does <b>not exist</b>, or if the current OS <b>is not a windows</b>.
        /// </summary>
        /// <param name="binaryName"></param>
        /// <param name="defaultBinaryExtension"></param>
        /// <returns>Binary existence status code.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static BinarySearcherStatusCodes CheckBinaryExistInSystemFolder(string binaryName, string defaultBinaryExtension = ".dll")
        {
            if (string.IsNullOrEmpty(binaryName))
            {
                throw new ArgumentNullException($"{nameof(binaryName)} is null or empty.");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string pathToSystemBinary = Path.Combine(Environment.SystemDirectory, binaryName);
                pathToSystemBinary = Path.HasExtension(pathToSystemBinary) ? pathToSystemBinary : $"{pathToSystemBinary}{defaultBinaryExtension}";

                return File.Exists(pathToSystemBinary) ? BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath : BinarySearcherStatusCodes.BinaryNotExists;
            }

            return BinarySearcherStatusCodes.BinaryNotExists;
        }

        /// <summary>
        /// Checks if the binary file from which the specified method was imported exists in the <b>system folder</b> <b>(windows only)</b>.<br/>
        /// 1 if the specified binary <b>exists</b> in the Windows <b>system folder</b>;<br/>
        /// 2 if it does <b>not exist</b>, or if the current OS <b>is not a windows</b>.
        /// </summary>
        /// <param name="importedMethod"></param>
        /// <param name="defaultBinaryExtension"></param>
        /// <returns>Binary existence status code.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static BinarySearcherStatusCodes CheckBinaryExistInSystemFolder(ImportedMethodInfo importedMethod, string defaultBinaryExtension = ".dll")
        {
            if (importedMethod is null)
            {
                throw new ArgumentNullException($"{nameof(importedMethod)} is null.");
            }

            return BinarySearcher.CheckBinaryExistInSystemFolder(importedMethod.DllLocation, defaultBinaryExtension);
        }

        /// <summary>
        /// 
        /// 0 if the binary file <b>exists</b> in the specified folder;<br/>
        /// 1 if the binary file is <b>on an absolute path</b> or <b>on an system folder</b> <b>(windows only)</b>;<br/>
        /// 2 if the binary file <b>does not exist</b>.
        /// </summary>
        /// <param name="pathToBinary"></param>
        /// <param name="analysisFolder"></param>
        /// <param name="defaultBinaryExtension"></param>
        /// <returns>Binary existence status code.</returns>
        public static BinarySearcherStatusCodes CheckBinaryExistInSourceAndSystemFolder(string pathToBinary, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return BinarySearcher.CheckBinaryExist(pathToBinary, analysisFolder, defaultBinaryExtension);
            }

            return (BinarySearcherStatusCodes)Math.Min(
                (int)BinarySearcher.CheckBinaryExist(pathToBinary, analysisFolder, defaultBinaryExtension), 
                (int)BinarySearcher.CheckBinaryExistInSystemFolder(pathToBinary, defaultBinaryExtension));
        }

        /// <summary>
        /// 
        /// 0 if the binary file <b>exists</b> in the specified folder;<br/>
        /// 1 if the binary file is <b>on an absolute path</b> or <b>on an system folder</b> <b>(windows only)</b>;<br/>
        /// 2 if the binary file <b>does not exist</b>.
        /// </summary>
        /// <param name="importedMethod"></param>
        /// <param name="analysisFolder"></param>
        /// <param name="defaultBinaryExtension"></param>
        /// <returns>Binary existence status code.</returns>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static BinarySearcherStatusCodes CheckBinaryExistInSourceAndSystemFolder(ImportedMethodInfo importedMethod, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            return BinarySearcher.CheckBinaryExistInSourceAndSystemFolder(importedMethod.DllLocation, analysisFolder, defaultBinaryExtension);
        }
    }
}
