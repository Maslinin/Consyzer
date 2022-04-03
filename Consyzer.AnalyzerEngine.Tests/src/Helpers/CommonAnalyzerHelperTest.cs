using Xunit;
using System.IO;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.Helpers;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.AnalyzerEngine.Tests.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class CommonAnalyzerHelperTest
    {
        [Fact(DisplayName = "Checking Definition for Metadata Content")]
        public void HasMetadata()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            bool hasMetadata = CommonAnalyzerHelper.HasMetadata(location);
            Assert.True(hasMetadata);
        }

        [Fact(DisplayName = "Checking for files containing metadata")]
        public void GetFilesContainsMetadata()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var hasMetadataFilesOptionOne = new List<FileInfo> { new FileInfo(location) }.GetFilesContainsMetadata();
            var hasMetadataFilesOptionTwo = new List<BinaryFileInfo> { new BinaryFileInfo(location) }.GetFilesContainsMetadata();
            var hasMetadataFilesOptionThree = new List<PEReader> { new PEReader(new FileStream(location, FileMode.Open, FileAccess.Read)) }.GetFilesContainsMetadata();

            Assert.NotEmpty(hasMetadataFilesOptionOne);
            Assert.NotEmpty(hasMetadataFilesOptionTwo);
            Assert.NotEmpty(hasMetadataFilesOptionThree);
        }

        [Fact(DisplayName = "Checking for files not containing metadata")]
        public void GetFilesNotContainsMetadata()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var notHasMetadataFilesOptionOne = new List<FileInfo> { new FileInfo(location) }.GetFilesNotContainsMetadata();
            var notHasMetadataFilesOptionTwo = new List<BinaryFileInfo> { new BinaryFileInfo(location) }.GetFilesNotContainsMetadata();
            var notHasMetadataFilesOptionThree = new List<PEReader> { new PEReader(new FileStream(location, FileMode.Open, FileAccess.Read)) }.GetFilesNotContainsMetadata();

            Assert.Empty(notHasMetadataFilesOptionOne);
            Assert.Empty(notHasMetadataFilesOptionTwo);
            Assert.Empty(notHasMetadataFilesOptionThree);
        }

        [Fact(DisplayName = "Checking that the metadata file is an assembly")]
        public void MetadataFileIsAssembly()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            bool isMetadata = CommonAnalyzerHelper.MetadataFileIsAssembly(location);
            Assert.True(isMetadata);
        }

        [Fact(DisplayName = "Checking that metadata files are assemblies")]
        public void GetMetadataAssemblyFiles()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var areAssembliesFilesOptionOne = new List<FileInfo> { new FileInfo(location) }.GetMetadataAssemblyFiles();
            var areAssembliesFilesOptionTwo = new List<BinaryFileInfo> { new BinaryFileInfo(location) }.GetMetadataAssemblyFiles();
            var areAssembliesFilesOptionThree = new List<PEReader> { new PEReader(new FileStream(location, FileMode.Open, FileAccess.Read)) }.GetMetadataAssemblyFiles();

            Assert.NotEmpty(areAssembliesFilesOptionOne);
            Assert.NotEmpty(areAssembliesFilesOptionTwo);
            Assert.NotEmpty(areAssembliesFilesOptionThree);
        }

        [Fact(DisplayName = "Checking that metadata files are not assemblies")]
        public void GetNotMetadataAssemblyFiles()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var areNotAssembliesFilesOptionOne = new List<FileInfo> { new FileInfo(location) }.GetNotMetadataAssemblyFiles();
            var areNotAssembliesFilesOptionTwo = new List<BinaryFileInfo> { new BinaryFileInfo(location) }.GetNotMetadataAssemblyFiles();
            var areNotAssembliesFilesOptionThree = new List<PEReader> { new PEReader(new FileStream(location, FileMode.Open, FileAccess.Read)) }.GetNotMetadataAssemblyFiles();

            Assert.Empty(areNotAssembliesFilesOptionOne);
            Assert.Empty(areNotAssembliesFilesOptionTwo);
            Assert.Empty(areNotAssembliesFilesOptionThree);
        }
    }
}
