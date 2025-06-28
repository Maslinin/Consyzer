using Xunit;
using Consyzer.Core.Resources;
using Consyzer.Core.Classifiers;
using static Consyzer.Tests.TestInfrastructure.Constants;
using static Consyzer.Tests.TestInfrastructure.Helpers.MatchesHelper;

namespace Consyzer.Tests.Core.Classifiers;

public sealed class AnalysisFileClassifierTests
{
    [Fact]
    public void Resolve_ShouldClassifyFilesCorrectly_WhenGivenMixedInput()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var peAccessor = new PEReaderAccessor(streamAccessor);
        var resolver = new AnalysisFileClassifier(peAccessor);

        var files = new[]
        {
            EcmaAssemblyWithPInvoke,
            NonEcmaAssembly
        };
        
        var result = resolver.Check(files);

        Assert.Single(result.EcmaAssemblies);
        Assert.Single(result.NonEcmaModules);

        Assert.Empty(result.NonEcmaAssemblies);

        Assert.True(Matches(EcmaAssemblyWithPInvoke, result.EcmaAssemblies));
        Assert.True(Matches(NonEcmaAssembly, result.NonEcmaModules));
    }
}