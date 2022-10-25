using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Consyzer.Cryptography.Tests")]

namespace Consyzer.Cryptography.Hash
{
    internal static class FileHashCalculator
    {
        internal static string CalculateMD5(FileInfo fileInfo)
        {
            using (var md5 = MD5.Create())
            {
                using (var fStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashBytes = md5.ComputeHash(fStream);

                    return HashBytesToString(hashBytes);
                }
            }
        }

        internal static string CalculateSHA256(FileInfo fileInfo)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var fStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashBytes = sha256.ComputeHash(fStream);

                    return HashBytesToString(hashBytes);
                }
            }
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
