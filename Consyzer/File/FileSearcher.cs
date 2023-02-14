using System;
using SIO = System.IO;
using System.Linq;
using System.Collections.Generic;
using static Consyzer.Constants.File;

namespace Consyzer.File
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
            string correctPath = FileHelper.GetAbsolutePath(this._analysisFolder, filePath);
            correctPath = FileHelper.AddExtensionToFile(correctPath, fileExtension);
            return SIO.File.Exists(correctPath) ? FileExistanceStatusCode.FileExistsAtAnalysisPath : FileExistanceStatusCode.FileDoesNotExists;
        }

        public FileExistanceStatusCode CheckFileExistsAtAbsolutePath(string filePath, string defaultFileExtension = DefaultFileExtension)
        {
            string correctPath = FileHelper.AddExtensionToFile(filePath, defaultFileExtension);
            return FileHelper.IsAbsolutePath(correctPath) ? FileExistanceStatusCode.FileExistsAtAbsolutePath : FileExistanceStatusCode.FileDoesNotExists;
        }

        public FileExistanceStatusCode CheckFileExistsAtRelativePath(string filePath, string defaultFileExtension = DefaultFileExtension)
        {
            string correctPath = FileHelper.AddExtensionToFile(filePath, defaultFileExtension);
            return FileHelper.IsRelativePath(correctPath) ? FileExistanceStatusCode.FileExistsAtRelativePath : FileExistanceStatusCode.FileDoesNotExists;
        }

        public FileExistanceStatusCode CheckFileExistsAtSystemFolder(string filePath, string defaultFileExtension = DefaultFileExtension)
        {
            string correctPath = FileHelper.GetAbsolutePath(Environment.SystemDirectory, filePath);
            correctPath = FileHelper.AddExtensionToFile(correctPath, defaultFileExtension);
            return SIO.File.Exists(correctPath) ? FileExistanceStatusCode.FileExistsAtSystemFolder : FileExistanceStatusCode.FileDoesNotExists;
        }

    }
}
