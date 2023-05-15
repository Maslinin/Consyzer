using Xunit;
using System.IO;
using Consyzer.Cryptography;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests.Cryptography
{
    public sealed class FileHashInfoTest
    {
        private int SHA256StandardHashLength => 64;

        [Fact]
        public void CalculateHash_ReturnsNonNullHashInfo()
        {
            var fileInfo = new FileInfo(MetadataAssemblyLocation);

            var hashInfo = FileHashInfo.CalculateHash(fileInfo);

            Assert.NotNull(hashInfo);
        }

        [Fact]
        public void CalculateHash_ShouldReturnIHashFileInfoWithRightHashSums()
        {
            var fileInfo = MetadataAssemblyFileInfo;

            var hashInfo = FileHashInfo.CalculateHash(fileInfo);

            Assert.Equal(this.SHA256StandardHashLength, hashInfo.SHA256Sum.Length);
        }

    }
}
