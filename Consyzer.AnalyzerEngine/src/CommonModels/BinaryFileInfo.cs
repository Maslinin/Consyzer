using System;
using System.IO;
using Consyzer.AnalyzerEngine.Helpers;

namespace Consyzer.AnalyzerEngine.CommonModels
{
    public sealed class BinaryFileInfo
    {
        public FileInfo BaseFileInfo { get; }
        public bool HasMetadata { get; }
        public bool IsAssembly { get; }
        public HashFileInfo HashInfo { get; }

        public BinaryFileInfo(FileInfo fileInfo)
        {
            if (fileInfo is null)
            {
                throw new ArgumentException($"{nameof(fileInfo)} is null.");
            }
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"{fileInfo.FullName} does not exist.");
            }

            this.BaseFileInfo = fileInfo;
            this.HasMetadata = CommonAnalyzerHelper.HasMetadata(fileInfo.FullName);
            this.IsAssembly = CommonAnalyzerHelper.MetadataFileIsAssembly(fileInfo.FullName);
            this.HashInfo = HashFileInfo.Calculate(fileInfo);
        }

        public BinaryFileInfo(string pathToBinary)
        {
            if (pathToBinary is null)
            {
                throw new ArgumentException($"{nameof(pathToBinary)} is null.");
            }
            if (!File.Exists(pathToBinary))
            {
                throw new FileNotFoundException($"File {pathToBinary} does not exist.");
            }

            var info = new FileInfo(pathToBinary);

            this.BaseFileInfo = info;
            this.HasMetadata = CommonAnalyzerHelper.HasMetadata(pathToBinary);
            this.IsAssembly = CommonAnalyzerHelper.MetadataFileIsAssembly(pathToBinary);
            this.HashInfo = HashFileInfo.Calculate(info);
        }

    }
}
