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

    [Fact]
    public void GetNamespace_ShouldReturnNonEmptyString()
    {
        var rootNamespace = this._signatureExtractor.GetNamespace(TestMethodDefinition);

        Assert.NotEmpty(rootNamespace);
    }

    [Fact]
    public void GetClassName_ShouldReturnNonEmptyString()
    {
        var className = this._signatureExtractor.GetClassName(TestMethodDefinition);

        Assert.NotEmpty(className);
    }

    [Fact]
    public void GetMethodName_ShouldReturnNonEmptyString()
    {
        var methodName = this._signatureExtractor.GetMethodName(TestMethodDefinition);

        Assert.NotEmpty(methodName);
    }

    [Fact]
    public void GetArguments_ShouldReturnNonNullCollection()
    {
        var methodArguments = this._signatureExtractor.GetArguments(TestMethodDefinition);

        Assert.NotNull(methodArguments);
    }

    private static MethodDefinition GetMethodDefinition()
    {
        //we take average element of collection because first definitions are technical
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