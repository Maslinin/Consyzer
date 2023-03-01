using Xunit;
using Consyzer.Cryptography;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests.Cryptography
{
    public sealed class FileHashCalculatorTest
    {
        private int MD5StandardHashLength => 32;
        private int SHA256StandardHashLength => 64;

        [Fact]
        public void CalculateMD5_ShouldGenerateNotEmptyHashString()
        {
            var fileInfo = MetadataAssemblyFileInfo;

            var hashString = FileHashCalculator.CalculateMD5(fileInfo);

            Assert.Equal(this.MD5StandardHashLength, hashString.Length);
        }

        [Fact]
        public void CalculateSHA256_ShouldGenerateHashStringWithLengthOf64Characters()
        {
            var fileInfo = MetadataAssemblyFileInfo;

            var hashString = FileHashCalculator.CalculateSHA256(fileInfo);

            Assert.Equal(this.SHA256StandardHashLength, hashString.Length);
        }
    }
}
