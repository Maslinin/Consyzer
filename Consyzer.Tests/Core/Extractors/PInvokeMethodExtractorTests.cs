using Xunit;
using Consyzer.Core.Resources;
using Consyzer.Core.Extractors;
using static Consyzer.Tests.TestInfrastructure.Constants;

namespace Consyzer.Tests.Core.Extractors;

public sealed class PInvokeMethodExtractorTests
{
    [Fact]
    public void Extract_ShouldReturnMethodsGroup_WhenPInvokeMethodsPresent()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var peAccessor = new PEReaderAccessor(streamAccessor);
        var extractor = new PInvokeMethodExtractor(peAccessor);

        var group = extractor.Extract(EcmaAssemblyWithPInvoke).First();

        Assert.Equal(EcmaAssemblyWithPInvoke.FullName, group.File.FullName);
        Assert.NotEmpty(group.Methods);

        var method = group.Methods.First();
        Assert.False(string.IsNullOrWhiteSpace(method.Signature.GetMethodLocation()));
        Assert.False(string.IsNullOrWhiteSpace(method.DllLocation));
    }

    [Fact]
    public void Extract_ShouldReturnEmpty_WhenNoPInvokeMethods()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var peAccessor = new PEReaderAccessor(streamAccessor);
        var extractor = new PInvokeMethodExtractor(peAccessor);

        var result = extractor.Extract(EcmaAssemblyWithoutPInvoke);

        Assert.Empty(result);
    }
}