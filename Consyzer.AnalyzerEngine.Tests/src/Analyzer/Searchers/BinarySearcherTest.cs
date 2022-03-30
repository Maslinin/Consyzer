using Xunit;
using System.Reflection;
using Consyzer.AnalyzerEngine.Analyzer.Searchers;

namespace Consyzer.AnalyzerEngine.Tests.Analyzer.Searchers
{
    public sealed class BinarySearcherTest
    {
        [Fact(DisplayName = "Checking the existence of a binary file")]
        public void CheckBinaryExists()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var statusCode = BinarySearcher.CheckBinaryExists(location);
            Assert.Equal(BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath, statusCode);
        }
    }
}
