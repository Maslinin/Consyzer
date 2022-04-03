using Xunit;
using System.IO;
using System.Reflection;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.AnalyzerEngine.Tests.CommonModels
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class BinaryFileInfoTest
    {
        [Fact(DisplayName = "Instance Creation")]
        public void InstanceCreation()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var exceptionOptionOne = Record.Exception(() => new BinaryFileInfo(location));
            var exceptionOptionTwo = Record.Exception(() => new BinaryFileInfo(new FileInfo(location)));

            Assert.Null(exceptionOptionOne);
            Assert.Null(exceptionOptionTwo);
        }
    }
}
