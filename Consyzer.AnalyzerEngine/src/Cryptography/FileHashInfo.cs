using System.IO;

namespace Consyzer.AnalyzerEngine.Cryptography
{
    /// <summary>
    /// [Sealed] Provides information about the hash amounts of the binary file.
    /// </summary>
    public sealed class FileHashInfo
    {
        /// <summary>
        /// Gets the <b>MD5</b> hash sum as a string.
        /// </summary>
        public string MD5Sum { get; }
        /// <summary>
        /// Gets the <b>SHA256</b> hash sum as a string.
        /// </summary>
        public string SHA256Sum { get; }

        public FileHashInfo(FileInfo fileInfo)
        {
            var hashCalculator = new FileHashCalculator(fileInfo);

            this.MD5Sum = hashCalculator.CalculateMD5();
            this.SHA256Sum = hashCalculator.CalculateSHA256();
        }
    }
}
