using Xunit;
using Consyzer.Extractors;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests.Extractors
{
    public sealed class MetadataExtractorTest
    {
        [Fact]
        public void InstanceCreation_ShouldNotThrowException()
        {
            var fileInfo = MetadataAssemblyFileInfo;

            var exception = Record.Exception(() => new MetadataExtractor(fileInfo));

            Assert.Null(exception);
        }

        [Fact]
        public void GetMethodDefinitions_ShouldReturnNotEmptyCollection()
        {
            var mdAnalyzer = new MetadataExtractor(MetadataAssemblyFileInfo);

            var methodDefs = mdAnalyzer.GetMethodDefinitions();

            Assert.NotEmpty(methodDefs);
        }

    }
}
