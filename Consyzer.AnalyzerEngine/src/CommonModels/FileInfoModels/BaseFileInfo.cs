using System;
using System.IO;

namespace Consyzer.AnalyzerEngine.CommonModels.FileInfoModels
{
    public class BaseFileInfo
    {
        public string Name { get; }
        public string FullName { get; }
        public string DirectoryName { get; }
        public string FullDirectoryName { get; }
        public string FileExtension { get; }
        public FileAttributes FileAttributes { get; }
        public DateTime CreationTime { get; }
        public DateTime LastAccessTime { get; }
        public DateTime LastChangedTime { get; }

        public BaseFileInfo(FileInfo fileInfo)
        {
            if(fileInfo is null)
            {
                throw new NullReferenceException($"{nameof(fileInfo)} has null reference");
            }
            if(!fileInfo.Exists)
            {
                throw new FileNotFoundException($"File {fileInfo.FullName} does not exist");
            }

            this.Name = fileInfo.Name;
            this.FullName = fileInfo.FullName;
            this.DirectoryName = fileInfo.DirectoryName;
            this.FullDirectoryName = fileInfo.Directory.FullName;
            this.FileExtension = fileInfo.Extension;
            this.FileAttributes = fileInfo.Attributes;
            this.CreationTime = fileInfo.CreationTime;
            this.LastAccessTime = fileInfo.LastAccessTime;
            this.LastChangedTime = fileInfo.LastWriteTime;
        }

        public BaseFileInfo(string pathToBinary)
        {
            var info = new FileInfo(pathToBinary);
            if (!info.Exists)
            {
                throw new FileNotFoundException($"File {pathToBinary} does not exist");
            }

            this.Name = info.Name;
            this.FullName = info.FullName;
            this.DirectoryName = info.DirectoryName;
            this.FullDirectoryName = info.Directory.FullName;
            this.FileExtension = info.Extension;
            this.FileAttributes = info.Attributes;
            this.CreationTime = info.CreationTime;
            this.LastAccessTime = info.LastAccessTime;
            this.LastChangedTime = info.LastWriteTime;
        }

    }
}
