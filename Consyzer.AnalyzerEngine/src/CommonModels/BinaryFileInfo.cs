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

            BaseFileInfo = fileInfo;
            HasMetadata = CommonAnalyzerHelper.HasMetadata(fileInfo.FullName);
            IsAssembly = CommonAnalyzerHelper.MetadataFileIsAssembly(fileInfo.FullName);
            HashInfo = HashFileInfo.Calculate(fileInfo);
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

            BaseFileInfo = info;
            HasMetadata = CommonAnalyzerHelper.HasMetadata(pathToBinary);
            IsAssembly = CommonAnalyzerHelper.MetadataFileIsAssembly(pathToBinary);
            HashInfo = HashFileInfo.Calculate(info);
        }

    }
}
