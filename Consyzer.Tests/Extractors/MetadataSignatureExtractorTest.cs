using Xunit;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Extractors;
using static Consyzer.Tests.TestConstants;

namespace Consyzer.Tests.Extractors;

public sealed class MetadataSignatureExtractorTest
{
    private static MethodDefinition TestMethodDefinition => GetMethodDefinition();

    private readonly IEcmaSignatureExtractor _signatureExtractor;

    public MetadataSignatureExtractorTest()
    {
        this._signatureExtractor = new MetadataSignatureExtractor(GetMetadataReader());
    }

    [Fact]
    public void GetSignatureInfo_ShouldReturnInstanceWithNonEmptyProperties()
    {
        var decodedSignature = this._signatureExtractor.GetSignature(TestMethodDefinition);

        Assert.NotNull(decodedSignature);
        Assert.NotEmpty(decodedSignature.Namespace);
        Assert.NotEmpty(decodedSignature.Class);
        Assert.NotEmpty(decodedSignature.Method);
        Assert.NotNull(decodedSignature.MethodArguments);
    }

    private static MethodDefinition GetMethodDefinition()
    {
        //take an average element of the collection because first definitions are always technical
        var methodsDefs = GetAllMethodsDefinitions();
        return methodsDefs.ElementAt(methodsDefs.Count() / 2);
    }

    private static IEnumerable<MethodDefinition> GetAllMethodsDefinitions()
    {
        var mdReader = GetMetadataReader();
        return mdReader.MethodDefinitions.Select(mdReader.GetMethodDefinition);
    }

    private static MetadataReader GetMetadataReader()
    {
        var fileStream = File.OpenRead(MetadataAssemblyLocation);
        var peReader = new PEReader(fileStream, PEStreamOptions.Default);
        return peReader.GetMetadataReader();
    }
}