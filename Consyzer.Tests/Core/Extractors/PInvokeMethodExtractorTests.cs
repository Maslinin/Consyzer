using Xunit;
using Consyzer.Core.Resources;
using Consyzer.Core.Extractors;
using static Consyzer.Tests.TestInfrastructure.Constants;

namespace Consyzer.Tests.Core.Extractors;

public sealed class PInvokeMethodExtractorTests
{
    [Fact]
    public void Extract_ShouldReturnMethods_WhenPInvokeMethodsPresent()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var peAccessor = new PEReaderAccessor(streamAccessor);
        var extractor = new PInvokeMethodExtractor(peAccessor);

        var methods = extractor.Extract(EcmaAssemblyWithPInvoke);

        Assert.NotEmpty(methods);

        var method = methods.First();
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