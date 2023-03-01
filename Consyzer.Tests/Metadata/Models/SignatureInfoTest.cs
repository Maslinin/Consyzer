using Xunit;
using System.Linq;
using Consyzer.Metadata;

namespace Consyzer.Tests.Metadata.Models
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class SignatureInfoTest
    {
        private SignatureExtractor Decoder => new SignatureExtractor(TestHelper.GetMetadataReader());
        private string StaticModifier => "static";

        [Fact]
        public void GetMethodLocation_ShouldReturnNotEmptyString()
        {
            var signatureInfo = this.Decoder.GetDecodedSignature(TestHelper.GetFirstMethodDefinition());

            var methodLocation = signatureInfo.MethodLocation;

            Assert.NotEmpty(methodLocation);
        }

        [Fact]
        public void GetFullMethodSignature_ShouldReturnNotEmptyStringWithStaticModifier()
        {
            var extractor = new MethodInfoExtractor(TestHelper.GetMetadataReader());
            var methodDef = TestHelper.GetAllMethodsDefinitions()
                .Where(m => extractor.IsStaticMethod(m)).First();
            var signatureInfo = this.Decoder.GetDecodedSignature(methodDef);

            var methodSignature = signatureInfo.FullMethodSignature;

            Assert.NotEmpty(methodSignature);
            Assert.Contains(StaticModifier, methodSignature);
        }

        [Fact]
        public void GetFullMethodSignature_ShouldReturnNotEmptyStringWithoutStaticModifier()
        {
            var extractor = new MethodInfoExtractor(TestHelper.GetMetadataReader());
            var methodDef = TestHelper.GetAllMethodsDefinitions()
                .Where(m => !extractor.IsStaticMethod(m)).First();
            var signatureInfo = this.Decoder.GetDecodedSignature(methodDef);

            var methodSignature = signatureInfo.FullMethodSignature;

            Assert.NotEmpty(methodSignature);
            Assert.DoesNotContain(StaticModifier, methodSignature);
        }

        [Fact]
        public void GetBaseMethodSignature_ShouldReturnNotEmptyString()
        {
            var signatureInfo = this.Decoder.GetDecodedSignature(TestHelper.GetFirstMethodDefinition());

            var methodLocation = signatureInfo.BaseMethodSignature;

            Assert.NotEmpty(methodLocation);
        }

    }
}
