using Xunit;
using Consyzer.Core.Resources;
using static Consyzer.Tests.TestInfrastructure.Constants;

namespace Consyzer.Tests.Core.Resources;

public sealed class PEReaderAccessorTests
{
    [Fact]
    public void Get_ShouldReturnPEReader_WhenCalledFirstTime()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var accessor = new PEReaderAccessor(streamAccessor);

        var reader = accessor.Get(EcmaAssemblyWithPInvoke);

        Assert.NotNull(reader);
        Assert.True(reader.HasMetadata);
    }

    [Fact]
    public void Get_ShouldReturnCachedPEReader_WhenCalledMultipleTimesWithSameFile()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var accessor = new PEReaderAccessor(streamAccessor);

        var reader1 = accessor.Get(EcmaAssemblyWithPInvoke);
        var reader2 = accessor.Get(EcmaAssemblyWithPInvoke);

        Assert.Same(reader1, reader2);
    }

    [Fact]
    public void Dispose_ShouldReleasePEReaders()
    {
        var streamAccessor = new FileStreamAccessor();
        var accessor = new PEReaderAccessor(streamAccessor);

        var reader = accessor.Get(EcmaAssemblyWithPInvoke);

        accessor.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = reader.HasMetadata);
    }
}