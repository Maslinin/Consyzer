using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Consyzer.Helpers;
using static Consyzer.Constants;

namespace Consyzer
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class FileSearcher
    {

        private const string _defaultFileExtension = ".dll";

        private readonly string _analysisFolder;

        public FileSearcher(string analysisFolder)
        {
            _analysisFolder = analysisFolder;
        }

        public FileExistanceStatusCode GetMaxFileExistanceStatusCode(IEnumerable<string> fileLocations, string defaultFileExtension = _defaultFileExtension)
        {
            return fileLocations.Max(f => this.GetMinFileExistanceStatusCode(f, defaultFileExtension));
        }

        public FileExistanceStatusCode GetMinFileExistanceStatusCode(string filePath, string fileExtension = _defaultFileExtension)
        {
            var codes = new int[]
            {
                (int)CheckFileExistAtAnalysisPath(filePath, fileExtension),
                (int)CheckFileExistAtAbsolutePath(filePath, fileExtension),
                (int)CheckFileExistAtRelativePath(filePath, fileExtension),
                (int)CheckFileExistAtSystemFolder(filePath, fileExtension),
            };

            return (FileExistanceStatusCode)codes.Min();
        }

        public FileExistanceStatusCode CheckFileExistAtAnalysisPath(string filePath, string fileExtension = _defaultFileExtension)
        {
            string path = IOHelper.GetAbsolutePath(_analysisFolder, filePath);
            path = IOHelper.AddExtensionToFile(path, fileExtension);
            return File.Exists(path) ? FileExistanceStatusCode.FileExistsAtAnalysisPath : FileExistanceStatusCode.FileDoesNotExists;
        }

        public FileExistanceStatusCode CheckFileExistAtAbsolutePath(string filePath, string fileExtension = _defaultFileExtension)
        {
            string path = IOHelper.AddExtensionToFile(filePath, fileExtension);
            return IOHelper.IsAbsolutePath(path) ? FileExistanceStatusCode.FileExistsAtAbsolutePath : FileExistanceStatusCode.FileDoesNotExists;
        }

        public FileExistanceStatusCode CheckFileExistAtRelativePath(string filePath, string fileExtension = _defaultFileExtension)
        {
            string path = IOHelper.AddExtensionToFile(filePath, fileExtension);
            return IOHelper.IsRelativePath(path) ? FileExistanceStatusCode.FileExistsAtRelativePath : FileExistanceStatusCode.FileDoesNotExists;
        }

        public FileExistanceStatusCode CheckFileExistAtSystemFolder(string filePath, string defaultFileExtension = _defaultFileExtension)
        {
            string path = IOHelper.GetAbsolutePath(Environment.SystemDirectory, filePath);
            path = IOHelper.AddExtensionToFile(path, defaultFileExtension);
            return File.Exists(path) ? FileExistanceStatusCode.FileExistsAtSystemFolder : FileExistanceStatusCode.FileDoesNotExists;
        }

    }
}
