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
            var files = IOHelper.GetBinaryFilesInfoFrom(location, new string[] { ".exe", ".dll" });
            Assert.NotEmpty(files);
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
