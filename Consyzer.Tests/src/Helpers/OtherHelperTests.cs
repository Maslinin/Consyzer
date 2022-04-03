using Xunit;
using Consyzer.Helpers;

namespace Consyzer.Tests.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class OtherHelperTests
    {
        [Fact(DisplayName = "Get the directory to analyze from argv")]
        public void GetDirectoryWithBinariesFromCommandLineArgs()
        {
            var exception = Record.Exception(() => OtherHelper.GetDirectoryWithBinariesFromCommandLineArgs());
            Assert.NotNull(exception);
        }

        [Fact(DisplayName = "Gets the binary file extensions for analysis from argv")]
        public void GetBinaryFilesExtensionsFromCommandLineArgs()
        {
            var exception = Record.Exception(() => OtherHelper.GetBinaryFilesExtensionsFromCommandLineArgs());
            Assert.NotNull(exception);
        }
    }
}
