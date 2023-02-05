using Xunit;
using System.IO;
using Consyzer.Analyzers;

namespace Consyzer.Tests.Analyzers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class MetadataFilterTest
    {
        [Fact]
        public void GetMetadataFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataFilter.GetMetadataFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void GetNotMetadataFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.NotMetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataFilter.GetNotMetadataFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void IsMetadataFile_ShouldReturnTrueIfFileDoesContainMetadata()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;

            bool isMetadataFile = MetadataFilter.IsMetadataFile(fileInfo);

            Assert.True(isMetadataFile);
        }

        [Fact]
        public void IsMetadataFile_ShouldReturnFalseIfFileDoesNotContainMetadata()
        {
            var fileInfo = TestConstants.NotMetadataAssemblyFileInfo;

            bool isNotMetadataFile = MetadataFilter.IsMetadataFile(fileInfo);

            Assert.False(isNotMetadataFile);
        }

        [Fact]
        public void GetMetadataAssemblyFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataFilter.GetMetadataAssemblyFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void GetNotMetadataAssemblyFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.NotMetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataFilter.GetNotMetadataAssemblyFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void IsMetadataAssemblyFile_ShouldReturnTrueIfFileIsMetadataAssembly()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;

            bool isMetadataFile = MetadataFilter.IsMetadataAssemblyFile(fileInfo);

            Assert.True(isMetadataFile);
        }

        [Fact]
        public void IsMetadataAssemblyFile_ShouldReturnFalseIfFileIsMetadataAssembly()
        {
            var fileInfo = TestConstants.NotMetadataAssemblyFileInfo;

            bool isNotMetadataFile = MetadataFilter.IsMetadataAssemblyFile(fileInfo);

            Assert.False(isNotMetadataFile);
        }
    }
}
