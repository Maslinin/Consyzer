using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Consyzer
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
        private const string _defaultExtension = ".dll";

        private readonly string _analysisFolder;
        private readonly string _defaultFileExtension;

        public FileExistenceChecker(string analysisFolder, string defaultFileExtension = _defaultExtension)
        {
            _analysisFolder = analysisFolder;
            _defaultFileExtension = defaultFileExtension;
        }

        public FileExistenceStatus GetMaxFileExistanceStatus(IEnumerable<string> fileLocations)
        {
            return fileLocations.Max(f => GetMinFileExistanceStatus(f));
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
            string correctPath = GetCorrectFilePath(filePath, _analysisFolder);
            return File.Exists(correctPath) ? FileExistenceStatus.FileExistsAtAnalysisPath : FileExistenceStatus.FileDoesNotExist;
        }

        public FileExistenceStatus CheckFileExistenceAtAbsolutePath(string filePath)
        {
            string correctPath = GetCorrectFilePath(filePath);
            return Path.IsPathFullyQualified(correctPath) ? FileExistenceStatus.FileExistsAtAbsolutePath : FileExistenceStatus.FileDoesNotExist;
        }

        public FileExistenceStatus CheckFileExistenceAtRelativePath(string filePath)
        {
            string correctPath = GetCorrectFilePath(filePath);
            return Path.IsPathRooted(correctPath) ? FileExistenceStatus.FileExistsAtRelativePath : FileExistenceStatus.FileDoesNotExist;
        }

        public FileExistenceStatus CheckFileExistenceAtSystemFolder(string filePath)
        {
            string correctPath = GetCorrectFilePath(filePath, Environment.SystemDirectory);
            return File.Exists(correctPath) ? FileExistenceStatus.FileExistsAtSystemFolder : FileExistenceStatus.FileDoesNotExist;
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string GetCorrectFilePath(string filePath, string folderPath)
        {
            string correctPath = GetAbsolutePath(folderPath, filePath);
            return GetCorrectFilePath(correctPath);
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string GetCorrectFilePath(string filePath)
        {
            string correctPath = AddExtensionToFile(filePath, _defaultFileExtension);
            return correctPath;
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private static string AddExtensionToFile(string filePath, string fileExtension)
        {
            return Path.HasExtension(filePath) ? filePath : Path.ChangeExtension(filePath, fileExtension);
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private static string GetAbsolutePath(string folder, string filePath)
        {
            return Path.IsPathFullyQualified(filePath) ? filePath : Path.Combine(folder, filePath);
        }
    }
}
