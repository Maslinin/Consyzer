using Xunit;
using System.IO;
using Consyzer.Metadata;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests.Metadata
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class MetadataFileFilterTest
    {
        [Fact]
        public void GetMetadataFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = MetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataFileFilter.GetMetadataFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void GetNotMetadataFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = NotMetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataFileFilter.GetNotMetadataFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void IsMetadataFile_ShouldReturnTrueIfFileDoesContainMetadata()
        {
            var fileInfo = MetadataAssemblyFileInfo;

            bool isMetadataFile = MetadataFileFilter.IsMetadataFile(fileInfo);

            Assert.True(isMetadataFile);
        }

        [Fact]
        public void IsMetadataFile_ShouldReturnFalseIfFileDoesNotContainMetadata()
        {
            var fileInfo = NotMetadataAssemblyFileInfo;

            bool isNotMetadataFile = MetadataFileFilter.IsMetadataFile(fileInfo);

            Assert.False(isNotMetadataFile);
        }

        [Fact]
        public void GetMetadataAssemblyFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = MetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataFileFilter.GetMetadataAssemblyFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void GetNotMetadataAssemblyFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = NotMetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataFileFilter.GetNotMetadataAssemblyFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void IsMetadataAssemblyFile_ShouldReturnTrueIfFileIsMetadataAssembly()
        {
            var fileInfo = MetadataAssemblyFileInfo;

            bool isMetadataFile = MetadataFileFilter.IsMetadataAssemblyFile(fileInfo);

            Assert.True(isMetadataFile);
        }

        [Fact]
        public void IsMetadataAssemblyFile_ShouldReturnFalseIfFileIsMetadataAssembly()
        {
            var fileInfo = NotMetadataAssemblyFileInfo;

            bool isNotMetadataFile = MetadataFileFilter.IsMetadataAssemblyFile(fileInfo);

            Assert.False(isNotMetadataFile);
        }
    }
}
