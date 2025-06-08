using System.Security.Cryptography;
using Consyzer.Core.Resources;

namespace Consyzer.Core.Cryptography;

internal sealed class Sha256FileHasher(
    IResourceAccessor<FileInfo, Stream> fileStreamAccessor
) : IFileHasher, IDisposable
{
    private readonly SHA256 _sha256 = SHA256.Create();

    public string CalculateHash(FileInfo fileInfo)
    {
        var stream = fileStreamAccessor.Get(fileInfo);
        byte[] hash = this._sha256.ComputeHash(stream);

        return Convert.ToHexString(hash);
    }

    public void Dispose()
    {
        this._sha256.Dispose();
    }
}