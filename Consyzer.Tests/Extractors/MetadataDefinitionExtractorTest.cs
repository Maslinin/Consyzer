using Xunit;
using Consyzer.Extractors;
using static Consyzer.Tests.TestConstants;

namespace Consyzer.Tests.Extractors;

public sealed class MetadataDefinitionExtractorTest
{
    private readonly IEcmaDefinitionExtractor _definitionExtractor;

    public MetadataDefinitionExtractorTest()
    {
        this._definitionExtractor = new MetadataDefinitionExtractor(MetadataAssemblyFileInfo);
    }

    [Fact]
    public void GetMethodDefinitions_ShouldReturnNonEmptyCollection()
    {
        var methodDefs = this._definitionExtractor.GetMethodDefinitions();

        Assert.NotEmpty(methodDefs);
    }

    [Fact]
    public void GetImportedMethodDefinitions_ShouldReturnNonEmptyCollection()
    {
        var methodDefs = this._definitionExtractor.GetImportedMethodDefinitions();

        Assert.NotEmpty(methodDefs);
    }
}