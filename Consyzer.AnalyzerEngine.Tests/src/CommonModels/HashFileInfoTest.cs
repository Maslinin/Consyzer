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

            var hashInfoOverloadOne = HashFileInfo.Calculate(new FileInfo(location));
            var overloadOneExceptionOne = Record.Exception(() => HashFileInfo.Calculate(fileInfo: null));
            var overloadOneExceptionTwo = Record.Exception(() => HashFileInfo.Calculate(new FileInfo($"{location}:Test")));
            var hashInfoOverloadTwo = HashFileInfo.Calculate(new BinaryFileInfo(location));
            var overloadTwoExceptionOne = Record.Exception(() => HashFileInfo.Calculate(binary: null));
            var hashInfoOverloadThree = HashFileInfo.Calculate(location);
            var overloadThreeExceptionOne = Record.Exception(() => HashFileInfo.Calculate(pathToBinary: null));

            Assert.NotEmpty(hashInfoOverloadOne.MD5Sum);
            Assert.NotEmpty(hashInfoOverloadOne.SHA256Sum);
            Assert.NotEmpty(hashInfoOverloadTwo.MD5Sum);
            Assert.NotEmpty(hashInfoOverloadTwo.SHA256Sum);
            Assert.NotEmpty(hashInfoOverloadThree.MD5Sum);
            Assert.NotEmpty(hashInfoOverloadThree.SHA256Sum);

            Assert.NotNull(overloadOneExceptionOne);
            Assert.NotNull(overloadOneExceptionTwo);
            Assert.NotNull(overloadTwoExceptionOne);
            Assert.NotNull(overloadThreeExceptionOne);
        }
    }
}
