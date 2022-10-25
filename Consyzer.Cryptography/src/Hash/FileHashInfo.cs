using System.IO;

namespace Consyzer.Cryptography.Hash
{
    /// <summary>
    /// Provides information about the hash amounts of a file.
    /// </summary>
    public class FileHashInfo : IHashInfo
    {
        public string MD5Sum { get; }
        public string SHA256Sum { get; }

        public FileHashInfo(FileInfo fileInfo)
        {
            this.MD5Sum = FileHashCalculator.CalculateMD5(fileInfo);
            this.SHA256Sum = FileHashCalculator.CalculateSHA256(fileInfo);
        }
    }
}
