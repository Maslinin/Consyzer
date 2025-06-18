using Xunit;
using Consyzer.Core.Checkers;
using Consyzer.Core.Resources;
using static Consyzer.Tests.TestInfrastructure.Constants;
using static Consyzer.Tests.TestInfrastructure.Helpers.MatchesHelper;

namespace Consyzer.Tests.Core.Checkers;

public sealed class FileClassificationCheckerTests
{
    [Fact]
    public void Check_ShouldClassifyFilesCorrectly_WhenGivenMixedInput()
    {
        using var streamAccessor = new FileStreamAccessor();
        using var peAccessor = new PEReaderAccessor(streamAccessor);
        var checker = new FileClassificationChecker(peAccessor);

        var files = new[]
        {
            EcmaAssemblyWithPInvoke,
            NonEcmaAssembly
        };
        
        var result = checker.Check(files);

        Assert.Single(result.EcmaAssemblies);
        Assert.Single(result.NonEcmaModules);

        Assert.Empty(result.NonEcmaAssemblies);

        Assert.True(Matches(EcmaAssemblyWithPInvoke, result.EcmaAssemblies));
        Assert.True(Matches(NonEcmaAssembly, result.NonEcmaModules));
    }
}