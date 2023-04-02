using Xunit;
using Consyzer.Extractors;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests.Extractors
{
    public sealed class ImportedMethodsExtractorTest
    {
        [Fact]
        public void GetImportedMethodsInfo_ShouldReturnNotNullCollection()
        {
            var importedMethodsAnalyzer = new ImportedMethodsExtractor(MetadataAssemblyFileInfo);

            var importedMethodsInfo = importedMethodsAnalyzer.GetImportedMethodsInfo();

            Assert.NotNull(importedMethodsInfo);
        }

        [Fact]
        public void GetImportedMethodDefinitions_ShouldReturnNotEmptyCollectionIfThereAreImportedMethods()
        {
            var importedMethodsAnalyzer = new ImportedMethodsExtractor(MetadataAssemblyFileInfo);

            var importedMethodsDefs = importedMethodsAnalyzer.GetImportedMethodDefinitions();

            Assert.NotEmpty(importedMethodsDefs);
        }
    }
}
