using Xunit;
using Consyzer.Core.Resources;
using Consyzer.Core.Cryptography;
using static Consyzer.Tests.TestInfrastructure.Constants;
using System.Security.Cryptography;

namespace Consyzer.Tests.Core.Cryptography;

public sealed class Sha256FileHasherTests
{
    [Fact]
    public void CalculateHash_ShouldReturnCorrectByteLength_WhenConverted()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var hasher = new Sha256FileHasher(streamAccessor);

        var hexHash = hasher.CalculateHash(EcmaAssemblyWithPInvoke);
        var byteHash = Convert.FromHexString(hexHash);

        Assert.Equal(SHA256.HashSizeInBytes, byteHash.Length);
    }

    [Fact]
    public void CalculateHash_ShouldReturnDifferentHashes_WhenFilesAreDifferent()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var hasher = new Sha256FileHasher(streamAccessor);

        var hash1 = hasher.CalculateHash(EcmaAssemblyWithPInvoke);
        var hash2 = hasher.CalculateHash(NonEcmaAssembly);

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Dispose_ShouldPreventFurtherHashing()
    {
        using var streamAccessor = new FileStreamAccessor();
        var hasher = new Sha256FileHasher(streamAccessor);

        hasher.Dispose();

        Assert.Throws<ObjectDisposedException>(() => hasher.CalculateHash(EcmaAssemblyWithPInvoke));
    }
}