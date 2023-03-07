using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Consyzer.Cryptography
{
    internal class FileHashInfo : IHashInfo
    {
        public string SHA256Sum { get; }

        public static IHashInfo CalculateHash(FileInfo fileInfo)
        {
            return new FileHashInfo(fileInfo);
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private FileHashInfo(FileInfo fileInfo)
        {
            using var sha256 = SHA256.Create();
            this.SHA256Sum = CalculateHash(fileInfo, sha256);
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private static string CalculateHash(FileInfo fileInfo, HashAlgorithm hashAlgorithm)
        {
            using var fStream = fileInfo.OpenRead();
            byte[] hashBytes = hashAlgorithm.ComputeHash(fStream);

            return HashBytesToString(hashBytes);
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private static string HashBytesToString(byte[] hashBytes)
        {
            var hash = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; ++i)
            {
                hash.Append(hashBytes[i].ToString("X2"));
            }

            return hash.ToString();
        }
    }
}
