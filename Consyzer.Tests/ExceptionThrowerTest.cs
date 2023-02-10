using Xunit;
using System.IO;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class ExceptionThrowerTest
    {
        [Fact]
        public void ThrowExceptionIfFileDoesNotExists_ShouldThrowArgumentException()
        {
            FileInfo fileInfo = null;

            var exception = Record.Exception(() => ExceptionChecker.ThrowExceptionIfFileDoesNotExist(fileInfo));

            Assert.NotNull(exception);
        }

        [Fact]
        public void ThrowExceptionIfFileIsNotMetadataAssembly_ShouldThrowAssemblyFileNotSupportedException()
        {
            FileInfo fileInfo = NotMetadataAssemblyFileInfo;

            var exception = Record.Exception(() => ExceptionChecker.ThrowExceptionIfFileIsNotMetadataAssembly(fileInfo));

            Assert.NotNull(exception);
        }

        [Fact]
        public void ThrowExceptionIfFileIsNotMetadataAssembly_ShouldNotThrowAssemblyFileNotSupportedException()
        {
            FileInfo fileInfo = MetadataAssemblyFileInfo;

            var exception = Record.Exception(() => ExceptionChecker.ThrowExceptionIfFileIsNotMetadataAssembly(fileInfo));

            Assert.Null(exception);
        }

        [Fact]
        public void ThrowExceptionIfFileDoesNotContainMetadata_ShouldThrowMetadataFileNotSupportedException()
        {
            FileInfo fileInfo = NotMetadataAssemblyFileInfo;

            var exception = Record.Exception(() => ExceptionChecker.ThrowExceptionIfFileDoesNotContainMetadata(fileInfo));

            Assert.NotNull(exception);
        }
    }
}
