using Xunit;
using Consyzer.AnalyzerEngine.Analyzers;

namespace Consyzer.AnalyzerEngine.Tests.Analyzers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class MetadataAnalyzerTest
    {
        [Fact]
        public void InstanceCreation_ShouldNotThrowException()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;

            var exception = Record.Exception(() => new MetadataAnalyzer(fileInfo));

            Assert.Null(exception);
        }

        [Fact]
        public void GetMethodsDefinitions_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;
            var mdAnalyzer = new MetadataAnalyzer(fileInfo);

            var methodDefs = mdAnalyzer.GetMethodsDefinitions();

            Assert.NotEmpty(methodDefs);
        }

        [Fact]
        public void GetTypesDefinitions_ShouldReturnNotEmptyCollection()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;
            var mdAnalyzer = new MetadataAnalyzer(fileInfo);

            var typesDefs = mdAnalyzer.GetTypesDefinitions();

            Assert.NotEmpty(typesDefs);
        }

        [Fact]
        public void GetImportedMethodsInfo_ShouldReturnNotNullCollection()
        {
            var importedMethodsAnalyzer = new MetadataAnalyzer(TestConstants.MetadataAssemblyFileInfo);

            var importedMethodsInfo = importedMethodsAnalyzer.GetImportedMethodsInfo();

            Assert.NotNull(importedMethodsInfo);
        }

        [Fact]
        public void GetImportedMethodsDefinitions_ShouldReturnNotEmptyCollectionIfThereAreImportedMethods()
        {
            var importedMethodsAnalyzer = new MetadataAnalyzer(TestConstants.MetadataAssemblyFileInfo);

            var importedMethodsDefs = importedMethodsAnalyzer.GetImportedMethodsDefinitions();

            Assert.NotEmpty(importedMethodsDefs);
        }

    }
}
