using Xunit;
using System.Reflection;
using Consyzer.AnalyzerEngine.Analyzer.Searchers;

namespace Consyzer.AnalyzerEngine.Tests.Analyzer.Searchers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class BinarySearcherTest
    {
        [Fact(DisplayName = "Checking the existence of a binary file")]
        public void CheckBinaryExists()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var statusCode = BinarySearcher.CheckBinaryExist(location, System.Environment.CurrentDirectory, ".dll");
            Assert.Equal(BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath, statusCode);
        }
    }
}
