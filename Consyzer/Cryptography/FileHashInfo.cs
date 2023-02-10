using System.IO;

namespace Consyzer.Cryptography
{
    internal class FileHashInfo : IHashInfo
    {
        public string MD5Sum { get; }
        public string SHA256Sum { get; }

        public static IHashInfo CalculateHash(FileInfo fileInfo)
        {
            return new FileHashInfo(fileInfo);
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private FileHashInfo(FileInfo fileInfo)
        {
            this.MD5Sum = FileHashCalculator.CalculateMD5(fileInfo);
            this.SHA256Sum = FileHashCalculator.CalculateSHA256(fileInfo);
        }

    }
}
