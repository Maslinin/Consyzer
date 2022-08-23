using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Consyzer.AnalyzerEngine.Tests")]

namespace Consyzer.AnalyzerEngine.Cryptography
{
    internal sealed class FileHashCalculator
    {
        private readonly FileInfo _fileInfo;

        public FileHashCalculator(FileInfo fileInfo)
        {
            this._fileInfo = fileInfo;
        }

        internal string CalculateMD5()
        {
            using (var md5 = MD5.Create())
            {
                using (var fStream = new FileStream(this._fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashBytes = md5.ComputeHash(fStream);

                    return HashBytesToString(hashBytes);
                }
            }
        }

        internal string CalculateSHA256()
        {
            using (var sha256 = SHA256.Create())
            {
                using (var fStream = new FileStream(this._fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashBytes = sha256.ComputeHash(fStream);

                    return HashBytesToString(hashBytes);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string HashBytesToString(byte[] hashBytes)
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
