using Xunit;
using System.IO;
using System.Collections.Generic;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests
{
    public sealed class MetadataFileFilterTest
    {
        [Fact]
        public void GetMetadataFiles_ShouldReturnOnlyMetadataFiles()
        {
            var fileInfos = new List<FileInfo>
            {
                MetadataAssemblyFileInfo,
                NotMetadataAssemblyFileInfo
            };

            var metadataFiles = MetadataFileFilter.GetMetadataFiles(fileInfos);

            Assert.Collection(metadataFiles,
                f => Assert.Equal(MetadataAssemblyFileInfo.FullName, f.FullName));
        }

        [Fact]
        public void GetNonMetadataFiles_ShouldReturnOnlyNonMetadataFiles()
        {
            var fileInfos = new List<FileInfo>
            {
                    MetadataAssemblyFileInfo,
                    NotMetadataAssemblyFileInfo
            };

            var nonMetadataFiles = MetadataFileFilter.GetNonMetadataFiles(fileInfos);

            Assert.Collection(nonMetadataFiles,
                f => Assert.Equal(NotMetadataAssemblyFileInfo.FullName, f.FullName));
        }

        [Fact]
        public void GetMetadataAssemblyFiles_ShouldReturnOnlyMetadataAssemblyFiles()
        {
            var fileInfos = new List<FileInfo>
            {
                    MetadataAssemblyFileInfo,
                    NotMetadataAssemblyFileInfo
            };

            var metadataAssemblyFiles = MetadataFileFilter.GetMetadataAssemblyFiles(fileInfos);

            Assert.Collection(metadataAssemblyFiles,
                f => Assert.Equal(MetadataAssemblyFileInfo.FullName, f.FullName));
        }

        [Fact]
        public void GetNonMetadataAssemblyFiles_ShouldReturnOnlyNonMetadataAssemblyFiles()
        {
            var fileInfos = new List<FileInfo>
            {
                    MetadataAssemblyFileInfo,
                    NotMetadataAssemblyFileInfo
            };

            var nonMetadataAssemblyFiles = MetadataFileFilter.GetNonMetadataAssemblyFiles(fileInfos);

            Assert.Collection(nonMetadataAssemblyFiles,
                f => Assert.Equal(NotMetadataAssemblyFileInfo.FullName, f.FullName));
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void IsMetadataFile_ShouldMeetTheConditions(bool expected, bool checkAssembly)
        {
            var fileInfo = expected ? MetadataAssemblyFileInfo : NotMetadataAssemblyFileInfo;

            var result = MetadataFileFilter.IsMetadataFile(fileInfo, checkAssembly);

            Assert.Equal(expected, result);
        }

    }
}
