using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Consyzer.FileInteraction
{
    internal enum FileExistenceStatus
    {
        FileExistsAtAnalysisPath,
        FileExistsAtAbsolutePath,
        FileExistsAtRelativePath,
        FileExistsAtSystemFolder,
        FileDoesNotExist
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class FileExistenceChecker
    {
        private readonly string _analysisFolder;
        private readonly string _defaultFileExtension;

        public FileExistenceChecker(string analysisFolder, string defaultFileExtension = FileHelper.DefaultExtension)
        {
            this._analysisFolder = analysisFolder;
            this._defaultFileExtension = defaultFileExtension;
        }

        public FileExistenceStatus GetMaxFileExistanceStatus(IEnumerable<string> fileLocations)
        {
            return fileLocations.Max(f => this.GetMinFileExistanceStatus(f));
        }

        public FileExistenceStatus GetMinFileExistanceStatus(string filePath)
        {
            var codes = new FileExistenceStatus[]
            {
                CheckFileExistenceAtAnalysisPath(filePath),
                CheckFileExistenceAtAbsolutePath(filePath),
                CheckFileExistenceAtRelativePath(filePath),
                CheckFileExistenceAtSystemFolder(filePath),
            };

            return codes.Min();
        }

        public FileExistenceStatus CheckFileExistenceAtAnalysisPath(string filePath)
        {
            string correctPath = this.GetCorrectFilePath(filePath, this._analysisFolder);
            return File.Exists(correctPath) ? FileExistenceStatus.FileExistsAtAnalysisPath : FileExistenceStatus.FileDoesNotExist;
        }

        public FileExistenceStatus CheckFileExistenceAtAbsolutePath(string filePath)
        {
            string correctPath = this.GetCorrectFilePath(filePath);
            return FileHelper.IsAbsolutePath(correctPath) ? FileExistenceStatus.FileExistsAtAbsolutePath : FileExistenceStatus.FileDoesNotExist;
        }

        public FileExistenceStatus CheckFileExistenceAtRelativePath(string filePath)
        {
            string correctPath = this.GetCorrectFilePath(filePath);
            return FileHelper.IsRelativePath(correctPath) ? FileExistenceStatus.FileExistsAtRelativePath : FileExistenceStatus.FileDoesNotExist;
        }

        public FileExistenceStatus CheckFileExistenceAtSystemFolder(string filePath)
        {
            string correctPath = this.GetCorrectFilePath(filePath, Environment.SystemDirectory);
            return File.Exists(correctPath) ? FileExistenceStatus.FileExistsAtSystemFolder : FileExistenceStatus.FileDoesNotExist;
        }

        private string GetCorrectFilePath(string filePath, string folderPath)
        {
            string correctPath = FileHelper.GetAbsolutePath(folderPath, filePath);
            return this.GetCorrectFilePath(correctPath);
        }

        private string GetCorrectFilePath(string filePath)
        {
            string correctPath = FileHelper.AddExtensionToFile(filePath, this._defaultFileExtension);
            return correctPath;
        }

    }
}
