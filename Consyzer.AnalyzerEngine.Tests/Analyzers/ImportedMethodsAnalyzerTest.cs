using Xunit;
using Consyzer.AnalyzerEngine.Analyzers;

namespace Consyzer.AnalyzerEngine.Tests.Analyzers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class ImportedMethodsAnalyzerTest
    {
        [Fact]
        public void InstanceCreation_ShouldNotThrowException()
        {
            var fileInfo = TestConstants.MetadataAssemblyFileInfo;

            var exception = Record.Exception(() => new ImportedMethodsAnalyzer(fileInfo));

            Assert.Null(exception);
        }

        [Fact]
        public void GetImportedMethodsInfo_ShouldReturnNotNullCollection()
        {
            var importedMethodsAnalyzer = new ImportedMethodsAnalyzer(TestConstants.MetadataAssemblyFileInfo);

            var importedMethodsInfo = importedMethodsAnalyzer.GetImportedMethodsInfo();

            Assert.NotNull(importedMethodsInfo);
        }

        [Fact]
        public void GetImportedMethodsDefinitions_ShouldReturnNotEmptyCollectionIfThereAreImportedMethods()
        {
            var importedMethodsAnalyzer = new ImportedMethodsAnalyzer(TestConstants.MetadataAssemblyFileInfo);

            var importedMethodsDefs = importedMethodsAnalyzer.GetImportedMethodsDefinitions();

            Assert.NotEmpty(importedMethodsDefs);
        }
    }
}
