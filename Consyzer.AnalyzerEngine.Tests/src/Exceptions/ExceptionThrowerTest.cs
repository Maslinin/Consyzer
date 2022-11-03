using Xunit;
using System.IO;
using Consyzer.AnalyzerEngine.Exceptions;

namespace Consyzer.AnalyzerEngine.Tests.Exceptions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class ExceptionThrowerTest
    {
        [Fact]
        public void ThrowExceptionIfFileDoesNotExists_ShouldThrowArgumentException()
        {
            FileInfo fileInfo = null;

            var exception = Record.Exception(() => ExceptionThrower.ThrowExceptionIfFileDoesNotExist(fileInfo));

            Assert.NotNull(exception);
        }

        [Fact]
        public void ThrowExceptionIfFileIsNotMetadataAssembly_ShouldThrowAssemblyFileNotSupportedException()
        {
            FileInfo fileInfo = TestConstants.NotMetadataAssemblyFileInfo;

            var exception = Record.Exception(() => ExceptionThrower.ThrowExceptionIfFileIsNotMetadataAssembly(fileInfo));

            Assert.NotNull(exception);
        }

        [Fact]
        public void ThrowExceptionIfFileDoesNotContainMetadata_ShouldThrowMetadataFileNotSupportedException()
        {
            FileInfo fileInfo = TestConstants.NotMetadataAssemblyFileInfo;

            var exception = Record.Exception(() => ExceptionThrower.ThrowExceptionIfFileDoesNotContainMetadata(fileInfo));

            Assert.NotNull(exception);
        }
    }
}
