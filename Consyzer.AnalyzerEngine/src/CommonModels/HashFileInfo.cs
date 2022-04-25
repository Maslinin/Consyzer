using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Consyzer.AnalyzerEngine.CommonModels
{
    /// <summary>
    /// [Sealed] Provides information about the hash amounts of the binary file.
    /// </summary>
    public sealed class HashFileInfo
    {
        /// <summary>
        /// Gets the <b>MD5</b> hash sum as a string.
        /// </summary>
        public string MD5Sum { get; }
        /// <summary>
        /// Gets the <b>SHA256</b> hash sum as a string.
        /// </summary>
        public string SHA256Sum { get; }

        private HashFileInfo(string MD5Sum, string SHA256Sum)
        {
            this.MD5Sum = MD5Sum;
            this.SHA256Sum = SHA256Sum;
        }

        #region Overloads of hash sum calculation

        /// <summary>
        /// Calculates the hash of the sum of the binary file.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns><b>HashFileInfo</b> instance containing the calculated hash sums of the binary file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static HashFileInfo Calculate(FileInfo fileInfo)
        {
            if (fileInfo is null)
            {
                throw new ArgumentNullException($"{nameof(fileInfo)} is null.");
            }
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"File {fileInfo.FullName} does not exist.");
            }

            string @MD5Sum, @SHA256Sum;

            //MD5 hash compute:
            using (var md5 = MD5.Create())
            {
                using (var fStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashBytes = md5.ComputeHash(fStream);

                    var hash = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; ++i)
                    {
                        hash.Append(hashBytes[i].ToString("X2"));
                    }

                    @MD5Sum = hash.ToString();
                }
            }

            //SHA256 hash compute:
            using (var sha256 = SHA256.Create())
            {
                using (var fStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashBytes = sha256.ComputeHash(fStream);

                    var hash = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; ++i)
                    {
                        hash.Append(hashBytes[i].ToString("X2"));
                    }

                    @SHA256Sum = hash.ToString();
                }
            }

            return new HashFileInfo(@MD5Sum, @SHA256Sum);
        }

        /// <summary>
        /// Calculates the hash of the sum of the binary file.
        /// </summary>
        /// <param name="binary"></param>
        /// <returns><b>HashFileInfo</b> instance containing the calculated hash sums of the binary file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static HashFileInfo Calculate(BinaryFileInfo binary)
        {
            if (binary is null)
            {
                throw new ArgumentNullException($"{nameof(binary)} is null.");
            }

            return HashFileInfo.Calculate(new FileInfo(binary.BaseFileInfo.FullName));
        }

        /// <summary>
        /// Calculates the hash of the sum of the binary file.
        /// </summary>
        /// <param name="pathToBinary"></param>
        /// <returns><b>HashFileInfo</b> instance containing the calculated hash sums of the binary file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static HashFileInfo Calculate(string pathToBinary)
        {
            if (string.IsNullOrEmpty(pathToBinary))
            {
                throw new ArgumentNullException($"{nameof(pathToBinary)} is null or empty.");
            }

            return HashFileInfo.Calculate(new FileInfo(pathToBinary));
        }

        #endregion
    }
}
