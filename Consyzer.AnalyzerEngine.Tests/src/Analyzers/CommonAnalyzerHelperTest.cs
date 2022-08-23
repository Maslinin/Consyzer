using Xunit;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.PortableExecutable;
using Consyzer.AnalyzerEngine.Analyzers;
using Consyzer.AnalyzerEngine.IO;

namespace Consyzer.AnalyzerEngine.Tests.Analyzers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class CommonAnalyzerHelperTest
    {
        [Fact(DisplayName = "Checking Definition for Metadata Content")]
        public void HasMetadata()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            bool hasMetadata = CommonAnalyzersHelper.HasMetadata(location);
            Assert.True(hasMetadata);
        }

        [Fact(DisplayName = "Checking for files containing metadata")]
        public void GetFilesContainsMetadata()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var hasMetadataFilesOverloadOne = new List<FileInfo> { new FileInfo(location) }.GetFilesContainsMetadata();
            var hasMetadataFilesOverloadTwo = new List<BinaryFileInfo> { new BinaryFileInfo(location) }.GetFilesContainsMetadata();
            var hasMetadataFilesOverloadThree = new List<PEReader> { new PEReader(new FileStream(location, FileMode.Open, FileAccess.Read)) }.GetFilesContainsMetadata();

            Assert.NotEmpty(hasMetadataFilesOverloadOne);
            Assert.NotEmpty(hasMetadataFilesOverloadTwo);
            Assert.NotEmpty(hasMetadataFilesOverloadThree);
        }

        [Fact(DisplayName = "Checking for files not containing metadata")]
        public void GetFilesNotContainsMetadata()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var notHasMetadataFilesOverloadOne = new List<FileInfo> { new FileInfo(location) }.GetFilesNotContainsMetadata();
            var notHasMetadataFilesOverloadTwo = new List<BinaryFileInfo> { new BinaryFileInfo(location) }.GetFilesNotContainsMetadata();
            var notHasMetadataFilesOverloadThree = new List<PEReader> { new PEReader(new FileStream(location, FileMode.Open, FileAccess.Read)) }.GetFilesNotContainsMetadata();

            Assert.Empty(notHasMetadataFilesOverloadOne);
            Assert.Empty(notHasMetadataFilesOverloadTwo);
            Assert.Empty(notHasMetadataFilesOverloadThree);
        }

        [Fact(DisplayName = "Checking that the metadata file is an assembly")]
        public void MetadataFileIsAssembly()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            bool isMetadata = CommonAnalyzersHelper.MetadataFileIsAssembly(location);
            Assert.True(isMetadata);
        }

        [Fact(DisplayName = "Checking that metadata files are assemblies")]
        public void GetMetadataAssemblyFiles()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var areAssembliesFilesOverloadOne = new List<FileInfo> { new FileInfo(location) }.GetMetadataAssemblyFiles();
            var areAssembliesFilesOverloadTwo = new List<BinaryFileInfo> { new BinaryFileInfo(location) }.GetMetadataAssemblyFiles();
            var areAssembliesFilesOverloadThree = new List<PEReader> { new PEReader(new FileStream(location, FileMode.Open, FileAccess.Read)) }.GetMetadataAssemblyFiles();

            Assert.NotEmpty(areAssembliesFilesOverloadOne);
            Assert.NotEmpty(areAssembliesFilesOverloadTwo);
            Assert.NotEmpty(areAssembliesFilesOverloadThree);
        }

        [Fact(DisplayName = "Checking that metadata files are not assemblies")]
        public void GetNotMetadataAssemblyFiles()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var areNotAssembliesFilesOverloadOne = new List<FileInfo> { new FileInfo(location) }.GetNotMetadataAssemblyFiles();
            var areNotAssembliesFilesOverloadTwo = new List<BinaryFileInfo> { new BinaryFileInfo(location) }.GetNotMetadataAssemblyFiles();
            var areNotAssembliesFilesOverloadThree = new List<PEReader> { new PEReader(new FileStream(location, FileMode.Open, FileAccess.Read)) }.GetNotMetadataAssemblyFiles();

            Assert.Empty(areNotAssembliesFilesOverloadOne);
            Assert.Empty(areNotAssembliesFilesOverloadTwo);
            Assert.Empty(areNotAssembliesFilesOverloadThree);
        }
    }
}
