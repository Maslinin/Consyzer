using Xunit;
using Consyzer.Core.Resources;
using static Consyzer.Tests.TestInfrastructure.Constants;

namespace Consyzer.Tests.Core.Resources;

public sealed class FileStreamAccessorTests
{
    [Fact]
    public void Get_ShouldReturnStream_WhenCalledFirstTime()
    {
        using var accessor = new FileStreamAccessor();

        var stream = accessor.Get(EcmaAssemblyWithPInvoke);

        Assert.NotNull(stream);
        Assert.True(stream.CanRead);
        Assert.True(stream.Length > 0);
    }

    [Fact]
    public void Get_ShouldReturnCachedStream_WhenCalledMultipleTimes()
    {
        using var accessor = new FileStreamAccessor();

        var stream1 = accessor.Get(EcmaAssemblyWithPInvoke);
        var stream2 = accessor.Get(EcmaAssemblyWithPInvoke);

        Assert.Same(stream1, stream2);
    }

    [Fact]
    public void Dispose_ShouldCloseAllStreams()
    {
        var accessor = new FileStreamAccessor();

        var stream = accessor.Get(EcmaAssemblyWithPInvoke);
        accessor.Dispose();

        Assert.False(stream.CanRead);
    }
}