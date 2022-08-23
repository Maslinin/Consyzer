using Xunit;
using System.IO;
using System.Reflection;
using Consyzer.AnalyzerEngine.Cryptography;

namespace Consyzer.AnalyzerEngine.Tests.Cryptography
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FileHashInfoTest
    {
        [Fact(DisplayName = "Instance Creation")]
        public void InstanceCreation()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var hashInfo = new FileHashInfo(new FileInfo(location));

            Assert.NotEmpty(hashInfo.MD5Sum);
            Assert.NotEmpty(hashInfo.SHA256Sum);
        }
    }
}
