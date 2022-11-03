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

        public FileExistStatusCodes GetFileLocation(string filePath, string fileExtension = _defaultFileExtension)
        {
            var codes = new int[]
            {
                (int)CheckFileExistAtAnalysisPath(filePath, fileExtension),
                (int)CheckFileExistAtAbsolutePath(filePath, fileExtension),
                (int)CheckFileExistAtRelativePath(filePath, fileExtension),
                (int)CheckFileExistAtSystemFolder(filePath, fileExtension),
            };

            return (FileExistStatusCodes)codes.Max();
        }

        public FileExistStatusCodes CheckFileExistAtAnalysisPath(string filePath, string fileExtension = _defaultFileExtension)
        {
            string path = PathHelper.ToAbsolutePath(this._analysisFolder, filePath);
            path = PathHelper.AddExtensionToFile(path, fileExtension);

            return File.Exists(path) ? FileExistStatusCodes.FileExistsAtAnalysisPath : FileExistStatusCodes.FileNotExists;
        }

        public FileExistStatusCodes CheckFileExistAtAbsolutePath(string filePath, string fileExtension = _defaultFileExtension)
        {
            string path = PathHelper.AddExtensionToFile(filePath, fileExtension);

            return PathHelper.IsAbsolutePath(path) ? FileExistStatusCodes.FileExistsAtAbsolutePath : FileExistStatusCodes.FileNotExists;
        }

        public FileExistStatusCodes CheckFileExistAtRelativePath(string filePath, string fileExtension = _defaultFileExtension)
        {
            string path = PathHelper.AddExtensionToFile(filePath, fileExtension);

            return PathHelper.IsRelativePath(path) ? FileExistStatusCodes.FileExistsAtRelativePath : FileExistStatusCodes.FileNotExists;
        }

        public FileExistStatusCodes CheckFileExistAtSystemFolder(string filePath, string defaultFileExtension = _defaultFileExtension) 
        {
            string path = PathHelper.ToAbsolutePath(Environment.SystemDirectory, filePath);
            path = PathHelper.AddExtensionToFile(path, defaultFileExtension);

            return File.Exists(path) ? FileExistStatusCodes.FileExistsAtSystemFolder : FileExistStatusCodes.FileNotExists;
        }

    }
}
