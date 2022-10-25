using Xunit;
using System.Linq;
using Consyzer.AnalyzerEngine.Decoders;

namespace Consyzer.AnalyzerEngine.Tests.Decoders.Models
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class SignatureInfoTest
    {
        public SignatureDecoder Decoder => new SignatureDecoder(TestConstants.MetadataAssemblyFileInfo);

        [Fact]
        public void GetMethodLocation_ShouldReturnNotEmptyString()
        {
            var signatureInfo = this.Decoder.GetDecodedSignature(TestHelper.GetFirstMethodDefinition());

            var methodLocation = signatureInfo.GetMethodLocation();

            Assert.NotEmpty(methodLocation);
        }

        [Fact]
        public void GetFullMethodSignature_ShouldReturnNotEmptyStringWithStaticModifier()
        {
            var extractor = new MethodInfoExtractor(TestHelper.GetMetadataReader());
            var methodDef = TestHelper.GetAllMethodsDefinitions().Where(x => extractor.IsStaticMethod(x)).First();
            var signatureInfo = this.Decoder.GetDecodedSignature(methodDef);

            var methodSignature = signatureInfo.GetFullMethodSignature();

            Assert.NotEmpty(methodSignature);
            Assert.Contains("static", methodSignature);
        }

        [Fact]
        public void GetFullMethodSignature_ShouldReturnNotEmptyStringWithoutStaticModifier()
        {
            var extractor = new MethodInfoExtractor(TestHelper.GetMetadataReader());
            var methodDef = TestHelper.GetAllMethodsDefinitions().Where(x => !extractor.IsStaticMethod(x)).First();
            var signatureInfo = this.Decoder.GetDecodedSignature(methodDef);

            var methodSignature = signatureInfo.GetFullMethodSignature();

            Assert.NotEmpty(methodSignature);
            Assert.DoesNotContain("static", methodSignature);
        }

        [Fact]
        public void GetBaseMethodSignature_ShouldReturnNotEmptyString()
        {
            var signatureInfo = this.Decoder.GetDecodedSignature(TestHelper.GetFirstMethodDefinition());

            var methodLocation = signatureInfo.GetBaseMethodSignature();

            Assert.NotEmpty(methodLocation);
        }

        [Fact]
        public void GetMethodArgsAsString_ShouldReturnEmptyStringIfMethodDoesNotContainArguments()
        {
            var extractor = new MethodInfoExtractor(TestHelper.GetMetadataReader());
            var methodDefWithoutArguments = TestHelper.GetAllMethodsDefinitions().Where(x => !extractor.GetMethodArguments(x).Any()).First();
            var signatureInfo = this.Decoder.GetDecodedSignature(methodDefWithoutArguments);

            var methodArgs = signatureInfo.GetMethodArgsAsString();

            Assert.Empty(methodArgs);
        }

        [Fact]
        public void GetMethodArgsAsString_ShouldReturnNotEmptyStringIfMethodContainsArguments()
        {
            var extractor = new MethodInfoExtractor(TestHelper.GetMetadataReader());
            var methodDefWithArguments = TestHelper.GetAllMethodsDefinitions().Where(x => extractor.GetMethodArguments(x).Any()).First();
            var signatureInfo = this.Decoder.GetDecodedSignature(methodDefWithArguments);

            var methodArgs = signatureInfo.GetMethodArgsAsString();

            Assert.NotEmpty(methodArgs);
        }
    }
}
