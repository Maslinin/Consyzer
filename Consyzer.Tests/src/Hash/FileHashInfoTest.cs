using Xunit;
using Consyzer.Cryptography;

namespace Consyzer.Tests.Cryptography
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FileHashInfoTest
    {
        [Fact]
        public void InstanceCreation_ShouldCreateInstanceWithNotEmptyProperties()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;

            var hashInfo = FileHashInfo.CalculateHash(fileInfo);

            Assert.NotEmpty(hashInfo.MD5Sum);
            Assert.NotEmpty(hashInfo.SHA256Sum);
        }

    }
}
