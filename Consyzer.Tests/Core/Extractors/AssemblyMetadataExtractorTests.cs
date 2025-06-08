using Xunit;
using Consyzer.Core.Resources;
using Consyzer.Core.Extractors;
using Consyzer.Core.Cryptography;
using static Consyzer.Tests.TestInfrastructure.Constants;
using static Consyzer.Tests.TestInfrastructure.Helpers.MatchesHelper;

namespace Consyzer.Tests.Core.Extractors;


public sealed class AssemblyMetadataExtractorTests
{
    private const string SemVerRegex = @"^\d+\.\d+\.\d+(\.\d+)?$";

    [Fact]
    public void Extract_ShouldReturnCorrectMetadata_WhenCalled()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var peAccessor = new PEReaderAccessor(streamAccessor);
        using var hasher = new Sha256FileHasher(streamAccessor);

        var extractor = new AssemblyMetadataExtractor(hasher, peAccessor);

        var metadata = extractor.Extract(EcmaAssemblyWithPInvoke);

        Assert.True(Matches(EcmaAssemblyWithPInvoke, metadata.File));
        Assert.True(Matches(EcmaAssemblyWithPInvoke, metadata.CreationDateUtc));
        Assert.NotEmpty(metadata.Sha256);
        Assert.Matches(SemVerRegex, metadata.Version);
    }
}