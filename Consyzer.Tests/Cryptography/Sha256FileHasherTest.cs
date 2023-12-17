using Xunit;
using Consyzer.Cryptography;
using static Consyzer.Tests.TestConstants;

namespace Consyzer.Tests.Cryptography;

public sealed class Sha256FileHasherTest
{
    private const int Sha256HashLength = 64;

    [Fact]
    public void CalculateHash_ShouldReturnHashWithCorrectLength()
    {
        var fileHashCalculator = new Sha256FileHasher();

        var hash = fileHashCalculator.CalculateHash(MetadataAssemblyFileInfo);

        Assert.Equal(Sha256HashLength, hash.Length);
    }
}