using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Consyzer.AnalyzerEngine.CommonModels
{
    public sealed class HashFileInfo
    {
        public string MD5Sum { get; }
        public string SHA256Sum { get; }

        private HashFileInfo(string MD5Sum, string SHA256Sum)
        {
            this.MD5Sum = MD5Sum;
            this.SHA256Sum = SHA256Sum;
        }

        #region Overloads of hash sum calculation

        public static HashFileInfo Calculate(FileInfo fileInfo)
        {
            if (fileInfo is null)
            {
                throw new System.NullReferenceException($"{nameof(fileInfo)} has null reference");
            }
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"File {fileInfo.FullName} does not exist");
            }

            string MD5Sum, SHA256Sum;

            //MD5 hash compute:
            using (var md5 = MD5.Create())
            {
                using (var fStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashBytes = md5.ComputeHash(fStream);

                    var hash = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        hash.Append(hashBytes[i].ToString("X2"));
                    }

                    MD5Sum = hash.ToString();
                }
            }

            //SHA256 hash compute:
            using (var sha256 = SHA256.Create())
            {
                using (var fStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashBytes = sha256.ComputeHash(fStream);

                    var hash = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        hash.Append(hashBytes[i].ToString("X2"));
                    }

                    SHA256Sum = hash.ToString();
                }
            }

            return new HashFileInfo(MD5Sum, SHA256Sum);
        }

        public static HashFileInfo Calculate(BinaryFileInfo binary)
        {
            return Calculate(new FileInfo(binary.BaseFileInfo.FullName));
        }

        public static HashFileInfo Calculate(string pathToBinary)
        {
            return Calculate(new FileInfo(pathToBinary));
        }

        #endregion
    }
}
