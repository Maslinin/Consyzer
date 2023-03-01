using Xunit;
using Consyzer.Metadata;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests.Metadata
{
    public sealed class MetadataAnalyzerTest
    {
        [Fact]
        public void InstanceCreation_ShouldNotThrowException()
        {
            var fileInfo = MetadataAssemblyFileInfo;

            var exception = Record.Exception(() => new MetadataAnalyzer(fileInfo));

            Assert.Null(exception);
        }

        [Fact]
        public void GetMethodDefinitions_ShouldReturnNotEmptyCollection()
        {
            var mdAnalyzer = new MetadataAnalyzer(MetadataAssemblyFileInfo);

            var methodDefs = mdAnalyzer.GetMethodDefinitions();

            Assert.NotEmpty(methodDefs);
        }

        [Fact]
        public void GetImportedMethodsInfo_ShouldReturnNotNullCollection()
        {
            var importedMethodsAnalyzer = new MetadataAnalyzer(MetadataAssemblyFileInfo);

            var importedMethodsInfo = importedMethodsAnalyzer.GetImportedMethodsInfo();

            Assert.NotNull(importedMethodsInfo);
        }

        [Fact]
        public void GetImportedMethodDefinitions_ShouldReturnNotEmptyCollectionIfThereAreImportedMethods()
        {
            var importedMethodsAnalyzer = new MetadataAnalyzer(MetadataAssemblyFileInfo);

            var importedMethodsDefs = importedMethodsAnalyzer.GetImportedMethodDefinitions();

            Assert.NotEmpty(importedMethodsDefs);
        }

    }
}
