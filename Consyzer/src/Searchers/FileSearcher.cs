using System;
using System.IO;
using System.Linq;

namespace Consyzer.Searchers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class FileSearcher
    {
        private const string _defaultFileExtension = ".dll";

        private readonly string _analysisFolder;

        public FileSearcher(string analysisFolder)
        {
            this._analysisFolder = analysisFolder;
        }

        public FileExistsStatusCodes GetFileLocation(string filePath, string fileExtension = _defaultFileExtension)
        {
            var codes = new int[]
            {
                (int)CheckFileExistAtAnalysisPath(filePath, fileExtension),
                (int)CheckFileExistAtAbsolutePath(filePath, fileExtension),
                (int)CheckFileExistAtRelativePath(filePath, fileExtension),
                (int)CheckFileExistAtSystemFolder(filePath, fileExtension),
            };

            return (FileExistsStatusCodes)codes.Min();
        }

        public FileExistsStatusCodes CheckFileExistAtAnalysisPath(string filePath, string fileExtension = _defaultFileExtension)
        {
            string path = PathHelper.ToAbsolutePath(this._analysisFolder, filePath);
            path = PathHelper.AddExtensionToFile(path, fileExtension);

            return File.Exists(path) ? FileExistsStatusCodes.FileExistsAtAnalysisPath : FileExistsStatusCodes.FileNotExists;
        }

        public FileExistsStatusCodes CheckFileExistAtAbsolutePath(string filePath, string fileExtension = _defaultFileExtension)
        {
            string path = PathHelper.AddExtensionToFile(filePath, fileExtension);

            return PathHelper.IsAbsolutePath(path) ? FileExistsStatusCodes.FileExistsAtAbsolutePath : FileExistsStatusCodes.FileNotExists;
        }

        public FileExistsStatusCodes CheckFileExistAtRelativePath(string filePath, string fileExtension = _defaultFileExtension)
        {
            string path = PathHelper.AddExtensionToFile(filePath, fileExtension);

            return PathHelper.IsRelativePath(path) ? FileExistsStatusCodes.FileExistsAtRelativePath : FileExistsStatusCodes.FileNotExists;
        }

        public FileExistsStatusCodes CheckFileExistAtSystemFolder(string filePath, string defaultFileExtension = _defaultFileExtension) 
        {
            string path = PathHelper.ToAbsolutePath(Environment.SystemDirectory, filePath);
            path = PathHelper.AddExtensionToFile(path, defaultFileExtension);

            return File.Exists(path) ? FileExistsStatusCodes.FileExistsAtSystemFolder : FileExistsStatusCodes.FileNotExists;
        }

    }
}
