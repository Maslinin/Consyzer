using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.Helpers;
using static Consyzer.Constants;

namespace Consyzer
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class FileSearcher
    {
        public enum FileExistanceStatusCode
        {
            FileExistsAtAnalysisPath,
            FileExistsAtAbsolutePath,
            FileExistsAtRelativePath,
            FileExistsAtSystemFolder,
            FileDoesNotExists
        }

        private readonly string _analysisFolder;

        public FileSearcher(string analysisFolder)
        {
            this._analysisFolder = analysisFolder;
        }

        public FileExistanceStatusCode GetMaxFileExistanceStatusCode(IEnumerable<string> fileLocations, string defaultFileExtension = DefaultFileExtension)
        {
            return fileLocations.Max(f => this.GetMinFileExistanceStatusCode(f, defaultFileExtension));
        }

        public FileExistanceStatusCode GetMinFileExistanceStatusCode(string filePath, string fileExtension = DefaultFileExtension)
        {
            var codes = new int[]
            {
                (int)CheckFileExistsAtAnalysisPath(filePath, fileExtension),
                (int)CheckFileExistsAtAbsolutePath(filePath, fileExtension),
                (int)CheckFileExistsAtRelativePath(filePath, fileExtension),
                (int)CheckFileExistsAtSystemFolder(filePath, fileExtension),
            };

            return (FileExistanceStatusCode)codes.Min();
        }

        public FileExistanceStatusCode CheckFileExistsAtAnalysisPath(string filePath, string fileExtension = DefaultFileExtension)
        {
            string correctPath = GetAbsolutePath(this._analysisFolder, filePath);
            correctPath = IOHelper.AddExtensionToFile(correctPath, fileExtension);
            return File.Exists(correctPath) ? FileExistanceStatusCode.FileExistsAtAnalysisPath : FileExistanceStatusCode.FileDoesNotExists;
        }

        public FileExistanceStatusCode CheckFileExistsAtAbsolutePath(string filePath, string fileExtension = DefaultFileExtension)
        {
            string correctPath = IOHelper.AddExtensionToFile(filePath, fileExtension);
            return Path.IsPathFullyQualified(correctPath) ? FileExistanceStatusCode.FileExistsAtAbsolutePath : FileExistanceStatusCode.FileDoesNotExists;
        }

        public FileExistanceStatusCode CheckFileExistsAtRelativePath(string filePath, string fileExtension = DefaultFileExtension)
        {
            string correctPath = IOHelper.AddExtensionToFile(filePath, fileExtension);
            return Path.IsPathRooted(correctPath) ? FileExistanceStatusCode.FileExistsAtRelativePath : FileExistanceStatusCode.FileDoesNotExists;
        }

        public FileExistanceStatusCode CheckFileExistsAtSystemFolder(string filePath, string defaultFileExtension = DefaultFileExtension)
        {
            string correctPath = GetAbsolutePath(Environment.SystemDirectory, filePath);
            correctPath = IOHelper.AddExtensionToFile(correctPath, defaultFileExtension);
            return File.Exists(correctPath) ? FileExistanceStatusCode.FileExistsAtSystemFolder : FileExistanceStatusCode.FileDoesNotExists;
        }

        private static string GetAbsolutePath(string folder, string filePath)
        {
            return Path.IsPathFullyQualified(filePath) ? filePath : Path.Combine(folder, filePath);
        }
    }
}
