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

            var hashInfoOptionOne = HashFileInfo.Calculate(location);
            var hashInfoOptionTwo = HashFileInfo.Calculate(new FileInfo(location));
            var hashInfoOptionThree = HashFileInfo.Calculate(new BinaryFileInfo(location));

            Assert.NotEmpty(hashInfoOptionOne.MD5Sum);
            Assert.NotEmpty(hashInfoOptionOne.SHA256Sum);
            Assert.NotEmpty(hashInfoOptionTwo.MD5Sum);
            Assert.NotEmpty(hashInfoOptionTwo.SHA256Sum);
            Assert.NotEmpty(hashInfoOptionThree.MD5Sum);
            Assert.NotEmpty(hashInfoOptionThree.SHA256Sum);
        }
    }
}
