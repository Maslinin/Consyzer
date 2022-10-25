using Xunit;
using System.IO;
using Consyzer.AnalyzerEngine.Analyzers;

namespace Consyzer.AnalyzerEngine.Tests.Analyzers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class MetadataCheckerTest
    {
        [Fact]
        public void GetMetadataFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataChecker.GetMetadataFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void GetNotMetadataFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.NotMetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataChecker.GetNotMetadataFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void IsMetadataFile_ShouldReturnTrueIfFileDoesContainMetadata()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;

            bool isMetadataFile = MetadataChecker.IsMetadataFile(fileInfo);

            Assert.True(isMetadataFile);
        }

        [Fact]
        public void IsMetadataFile_ShouldReturnFalseIfFileDoesNotContainMetadata()
        {
            var fileInfo = TestConstants.NotMetadataAssemblyFileInfo;

            bool isNotMetadataFile = MetadataChecker.IsMetadataFile(fileInfo);

            Assert.False(isNotMetadataFile);
        }

        [Fact]
        public void GetMetadataAssemblyFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataChecker.GetMetadataAssemblyFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void GetNotMetadataAssemblyFiles_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.NotMetadataAssemblyFileInfo;
            var fileInfoList = new FileInfo[] { fileInfo };

            var metadataFiles = MetadataChecker.GetNotMetadataAssemblyFiles(fileInfoList);

            Assert.NotEmpty(metadataFiles);
        }

        [Fact]
        public void IsMetadataAssemblyFile_ShouldReturnTrueIfFileIsMetadataAssembly()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;

            bool isMetadataFile = MetadataChecker.IsMetadataAssemblyFile(fileInfo);

            Assert.True(isMetadataFile);
        }

        [Fact]
        public void IsMetadataAssemblyFile_ShouldReturnFalseIfFileIsMetadataAssembly()
        {
            var fileInfo = TestConstants.NotMetadataAssemblyFileInfo;

            bool isNotMetadataFile = MetadataChecker.IsMetadataAssemblyFile(fileInfo);

            Assert.False(isNotMetadataFile);
        }
    }
}
