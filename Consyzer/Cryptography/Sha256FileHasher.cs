using System.Security.Cryptography;

namespace Consyzer.Cryptography;

internal sealed class Sha256FileHasher : IFileHasher
{
    public string CalculateHash(FileInfo fileInfo)
    {
        using var sha256 = SHA256.Create();
        using var fileStream = fileInfo.OpenRead();
        byte[] hashBytes = sha256.ComputeHash(fileStream);

        return BytesToHexString(hashBytes);
    }

    private static string BytesToHexString(byte[] hashBytes)
    {
        return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
    }
}