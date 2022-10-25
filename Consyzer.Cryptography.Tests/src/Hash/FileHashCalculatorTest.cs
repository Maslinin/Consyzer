using Xunit;
using Consyzer.Cryptography.Tests;
using System.IO;

namespace Consyzer.Cryptography.Hash.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FileHashCalculatorTest
    {
        [Fact]
        public void CalculateMD5_ShouldGenerateNotEmptyHashString()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;

            var hashString = FileHashCalculator.CalculateMD5(fileInfo);

            Assert.Equal(32, hashString.Length);
        }

        [Fact]
        public void CalculateSHA256_ShouldGenerateHashStringWithLengthOf64Characters()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;

            var hashString = FileHashCalculator.CalculateSHA256(fileInfo);

            Assert.Equal(64, hashString.Length);
        }
    }
}
