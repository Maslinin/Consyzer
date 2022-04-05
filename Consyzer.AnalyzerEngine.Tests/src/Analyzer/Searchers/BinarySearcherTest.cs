using Xunit;
using System;
using System.Reflection;
using Consyzer.AnalyzerEngine.Decoder.SyntaxModels;
using Consyzer.AnalyzerEngine.Analyzer.Searchers;
using Consyzer.AnalyzerEngine.Analyzer.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Tests.Analyzer.Searchers
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
            var OverloadOneExceptionOne = Record.Exception(() => BinarySearcher.CheckBinaryExist(binaryPath: null, Environment.CurrentDirectory, ".dll"));
            var OverloadOneExceptionTwo = Record.Exception(() => BinarySearcher.CheckBinaryExist(location, null, ".dll"));

            var statusCodeOverloadTwo = BinarySearcher.CheckBinaryExist(new ImportedMethodInfo(new SignatureInfo(), location, string.Empty), Environment.CurrentDirectory, ".dll");
            var OverloadTwoExeptionOne = Record.Exception(() => BinarySearcher.CheckBinaryExist(importedMethod: null, Environment.CurrentDirectory, ".dll"));

            Assert.Equal(BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath, statusCodeOverloadOneOptionOne);
            Assert.Equal(BinarySearcherStatusCodes.BinaryNotExists, statusCodeOverloadOneOptionTwo);
            Assert.NotNull(OverloadOneExceptionOne);
            Assert.NotNull(OverloadOneExceptionTwo);

            Assert.Equal(BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath, statusCodeOverloadTwo);
            Assert.NotNull(OverloadTwoExeptionOne);
        }
    }
}
