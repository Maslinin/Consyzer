using Xunit;
using Consyzer.Extractors;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests.Extractors
{
    public sealed class ImportedMethodExtractorTest
    {

        [Fact]
        public void InstanceCreation_ShouldSetFileInfoProperty()
        {
            var fileInfo = MetadataAssemblyFileInfo;

            var extractor = new ImportedMethodExtractor(fileInfo);

            Assert.Equal(fileInfo, extractor.FileInfo);
        }

        [Fact]
        public void GetImportedMethodsInfo_ShouldReturnNotNullCollection()
        {
            var importedMethodsAnalyzer = new ImportedMethodExtractor(MetadataAssemblyFileInfo);

            var importedMethodsInfo = importedMethodsAnalyzer.GetImportedMethodsInfo();

            Assert.NotEmpty(importedMethodsInfo);
        }

        [Fact]
        public void GetImportedMethodDefinitions_ShouldReturnNotEmptyCollectionIfThereAreImportedMethods()
        {
            var importedMethodsAnalyzer = new ImportedMethodExtractor(MetadataAssemblyFileInfo);

            var importedMethodsDefs = importedMethodsAnalyzer.GetImportedMethodDefinitions();

            Assert.NotEmpty(importedMethodsDefs);
        }
    }
}
