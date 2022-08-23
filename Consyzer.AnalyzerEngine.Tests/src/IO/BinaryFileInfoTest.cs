using Xunit;
using System.IO;
using System.Reflection;
using Consyzer.AnalyzerEngine.IO;

namespace Consyzer.AnalyzerEngine.Tests.IO
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class BinaryFileInfoTest
    {
        [Fact(DisplayName = "Instance Creation")]
        public void InstanceCreation()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var exceptionOverloadOne = Record.Exception(() => new BinaryFileInfo(location));
            var exceptionOverloadTwo = Record.Exception(() => new FileInfo(location));

            Assert.Null(exceptionOverloadOne);
            Assert.Null(exceptionOverloadTwo);
        }
    }
}
