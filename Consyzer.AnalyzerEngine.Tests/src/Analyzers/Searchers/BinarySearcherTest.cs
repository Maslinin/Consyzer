using Xunit;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Consyzer.AnalyzerEngine.Analyzers.Searchers;

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

        [Fact(DisplayName = "Checking the existence of a binary file in system folder")]
        public void CheckBinaryExistInSystemFolder()
        {
            var statusCodeOptionOne = BinarySearcher.CheckBinaryExistInSystemFolder("checkExample", ".dll");
            var statusCodeOptionTwo = BinarySearcher.CheckBinaryExistInSystemFolder("kernel32", ".dll");
            var statusCodeOptionThree = BinarySearcher.CheckBinaryExistInSystemFolder("kernel32.dll", null);
            var OverloadOneExceptionOne = Record.Exception(() => BinarySearcher.CheckBinaryExistInSystemFolder(binaryName: null, ".dll"));

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Assert.Equal(BinarySearcherStatusCodes.BinaryNotExists, statusCodeOptionOne);
            Assert.Equal(BinarySearcherStatusCodes.BinaryNotExists, statusCodeOptionOne);
            Assert.Equal(BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath, statusCodeOptionTwo);
            Assert.Equal(BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath, statusCodeOptionThree);
            Assert.NotNull(OverloadOneExceptionOne);
        }

        [Fact(DisplayName = "Checking the existence of a binary file in source and system folder")]
        public void CheckBinaryExistInSourceAndSystemFolder()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var statusCodeOptionOne = BinarySearcher.CheckBinaryExistInSourceAndSystemFolder("checkExample", location, ".dll");
            var statusCodeOptionTwo = BinarySearcher.CheckBinaryExistInSourceAndSystemFolder("kernel32", location, ".dll");
            var statusCodeOptionThree = BinarySearcher.CheckBinaryExistInSourceAndSystemFolder("kernel32.dll", location, null);
            var OverloadOneExceptionOne = Record.Exception(() => BinarySearcher.CheckBinaryExist(pathToBinary: null, Environment.CurrentDirectory, ".dll"));
            var OverloadOneExceptionTwo = Record.Exception(() => BinarySearcher.CheckBinaryExist(location, null, ".dll"));

            Assert.Equal(BinarySearcherStatusCodes.BinaryNotExists, statusCodeOptionOne);
            Assert.Equal(BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath, statusCodeOptionTwo);
            Assert.Equal(BinarySearcherStatusCodes.BinaryExistsOnAbsolutePath, statusCodeOptionThree);
            Assert.NotNull(OverloadOneExceptionOne);
            Assert.NotNull(OverloadOneExceptionTwo);
        }
    }
}
