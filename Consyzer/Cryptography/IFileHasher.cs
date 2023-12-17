using System.IO;

namespace Consyzer.Cryptography;

internal interface IFileHasher
{
    string CalculateHash(FileInfo fileInfo);
}