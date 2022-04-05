using Xunit;
using System;
using System.Reflection;
using Consyzer.AnalyzerEngine.Helpers;

namespace Consyzer.AnalyzerEngine.Tests.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class IOHelperTest
    {
        [Fact(DisplayName = "Checking for information about binary files")]
        public void GetBinaryFilesInfoFrom()
        {
            string location = Environment.CurrentDirectory;

            var filesOptionOne = IOHelper.GetBinaryFilesInfoFrom(location, new string[] { ".exe", ".dll" });
            var filesOptionTwo = IOHelper.GetBinaryFilesInfoFrom(location);
            var exceptionOptionOne = Record.Exception(() => IOHelper.GetBinaryFilesInfoFrom($"{location}:Test"));

            Assert.NotEmpty(filesOptionOne);
            Assert.NotEmpty(filesOptionTwo);
            Assert.NotNull(exceptionOptionOne);
        }

        [Fact(DisplayName = "Checking executable location to an absolute path")]
        public void IsAbsolutePath()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            bool isAbsolutePath = IOHelper.IsAbsolutePath(location);
            Assert.True(isAbsolutePath);
        }
    }
}
