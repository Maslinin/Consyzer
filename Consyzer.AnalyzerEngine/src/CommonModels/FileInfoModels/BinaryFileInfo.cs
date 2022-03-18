using System.IO;

namespace Consyzer.AnalyzerEngine.CommonModels.FileInfoModels
{
    public sealed class BinaryFileInfo
    {
        public FileInfo BaseFileInfo { get; }
        public bool HasMetadata { get; }
        public bool IsAssembly { get; }
        public HashFileInfo HashInfo { get; }

        public BinaryFileInfo(string pathToBinary)
        {
            var info = new FileInfo(pathToBinary);
            if (!info.Exists)
            {
                throw new FileNotFoundException($"File {pathToBinary} does not exist");
            }
            
            this.BaseFileInfo = info;
            this.HasMetadata = Support.AnalyzerSupport.HasMetadata(pathToBinary);
            this.IsAssembly = Support.AnalyzerSupport.IsAssembly(pathToBinary);
            this.HashInfo = HashFileInfo.Calculate(info);
        }
    }
}
