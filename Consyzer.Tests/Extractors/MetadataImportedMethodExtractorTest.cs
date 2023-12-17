using Xunit;
using Consyzer.Extractors;
using static Consyzer.Tests.TestConstants;

namespace Consyzer.Tests.Extractors;

public sealed class MetadataImportedMethodExtractorTest
{
    private readonly IEcmaImportedMethodExtractor _importedMethodExtractor;

    public MetadataImportedMethodExtractorTest()
    {
        this._importedMethodExtractor = new MetadataImportedMethodExtractor(MetadataAssemblyFileInfo);
    }

    [Fact]
    public void GetImportedMethodInfos_ShouldReturnNonEmptyCollection()
    {
        var importedMethodsInfo = this._importedMethodExtractor.GetImportedMethodInfos();

        Assert.NotEmpty(importedMethodsInfo);
    }

    [System.Runtime.InteropServices.DllImport("test")]
    private extern static int ImportedMethodForTest(int testArg);
}