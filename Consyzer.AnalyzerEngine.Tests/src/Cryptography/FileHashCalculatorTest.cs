using Xunit;
using System.IO;
using System.Reflection;
using Consyzer.AnalyzerEngine.Cryptography;

namespace Consyzer.AnalyzerEngine.Tests.Cryptography
{
    public sealed class FileHashCalculatorTest
    {
        [Fact(DisplayName = "Instance Creation")]
        public void InstanceCreation()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location);

            var exception = Record.Exception(() => new FileHashCalculator(location));

            Assert.Null(exception);
        }

        [Fact(DisplayName = "MD5 Calculation - Should Generated Not Empty Hash String")]
        public void CalculateMD5_ShouldGenerateNotEmptyHashString()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location);

            var hashString = new FileHashCalculator(location).CalculateMD5();

            Assert.NotEmpty(hashString);
        }

        [Fact(DisplayName = "SHA256 Calculation - Should Generated Not Empty Hash String")]
        public void CalculateSHA256_ShouldGenerateNotEmptyHashString()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location);

            var hashString = new FileHashCalculator(location).CalculateSHA256();

            Assert.NotEmpty(hashString);
        }
    }
}
