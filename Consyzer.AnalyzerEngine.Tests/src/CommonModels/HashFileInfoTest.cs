using Xunit;
using System.IO;
using System.Reflection;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.AnalyzerEngine.Tests.CommonModels
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class HashFileInfoTest
    {
        [Fact(DisplayName = "Hash calculating check")]
        public void CalculateHash()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var hashInfoOverloadOne = HashFileInfo.Calculate(location);
            var hashInfoOverloadTwo = HashFileInfo.Calculate(new FileInfo(location));
            var hashInfoOverloadThree = HashFileInfo.Calculate(new BinaryFileInfo(location));

            Assert.NotEmpty(hashInfoOverloadOne.MD5Sum);
            Assert.NotEmpty(hashInfoOverloadOne.SHA256Sum);
            Assert.NotEmpty(hashInfoOverloadTwo.MD5Sum);
            Assert.NotEmpty(hashInfoOverloadTwo.SHA256Sum);
            Assert.NotEmpty(hashInfoOverloadThree.MD5Sum);
            Assert.NotEmpty(hashInfoOverloadThree.SHA256Sum);
        }
    }
}
