using Xunit;
using System;
using System.Reflection;
using Consyzer.AnalyzerEngine.Decoders.SyntaxModels;
using Consyzer.AnalyzerEngine.Analyzers.Searchers;
using Consyzer.AnalyzerEngine.Analyzers.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Tests.Analyzers.Searchers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class BinarySearcherTest
    {
        [Fact(DisplayName = "Checking the existence of a binary file")]
        public void CheckBinaryExists()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var statusCodeOverloadOneOptionOne = BinarySearcher.CheckBinaryExist(location, Environment.CurrentDirectory, ".dll");
            var statusCodeOverloadOneOptionTwo = BinarySearcher.CheckBinaryExist(location.Replace("\\", " ").Replace("/", " "), Environment.CurrentDirectory, ".dll");
            var OverloadOneExceptionOne = Record.Exception(() => BinarySearcher.CheckBinaryExist(pathToBinary: null, Environment.CurrentDirectory, ".dll"));
            var OverloadOneExceptionTwo = Record.Exception(() => BinarySearcher.CheckBinaryExist(location, null, ".dll"));

            Assert.Equal(BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath, statusCodeOverloadOneOptionOne);
            Assert.Equal(BinarySearcherStatusCodes.BinaryNotExists, statusCodeOverloadOneOptionTwo);
            Assert.NotNull(OverloadOneExceptionOne);
            Assert.NotNull(OverloadOneExceptionTwo);
        }
    }
}
