using Xunit;
using System.Linq;
using System.Reflection;
using Consyzer.Extractors;

namespace Consyzer.Tests.Extractors.Models
{
    public sealed class SignatureInfoTest
    {
        private SignatureInfoExtractor Decoder => new SignatureInfoExtractor(TestHelper.GetMetadataReader());
        private string StaticModifier => nameof(MethodAttributes.Static);

        [Fact]
        public void GetMethodLocation_ShouldReturnNotEmptyString()
        {
            var signatureInfo = this.Decoder.GetSignatureInfo(TestHelper.GetFirstMethodDefinition());

            var methodLocation = signatureInfo.MethodLocation;

            Assert.NotEmpty(methodLocation);
        }

        [Fact]
        public void GetFullMethodSignature_ShouldReturnNotEmptyStringWithStaticModifier()
        {
            var extractor = new SignatureInfoExtractor(TestHelper.GetMetadataReader());
            var methodDef = TestHelper.GetAllMethodsDefinitions()
                .Where(m => extractor.IsStaticMethod(m)).First();
            var signatureInfo = this.Decoder.GetSignatureInfo(methodDef);

            var methodSignature = signatureInfo.FullMethodSignature;

            Assert.NotEmpty(methodSignature);
            Assert.Contains(StaticModifier, methodSignature);
        }

        [Fact]
        public void GetFullMethodSignature_ShouldReturnNotEmptyStringWithoutStaticModifier()
        {
            var extractor = new SignatureInfoExtractor(TestHelper.GetMetadataReader());
            var methodDef = TestHelper.GetAllMethodsDefinitions()
                .Where(m => !extractor.IsStaticMethod(m)).First();
            var signatureInfo = this.Decoder.GetSignatureInfo(methodDef);

            var methodSignature = signatureInfo.FullMethodSignature;

            Assert.NotEmpty(methodSignature);
            Assert.DoesNotContain(StaticModifier, methodSignature);
        }

        [Fact]
        public void GetBaseMethodSignature_ShouldReturnNotEmptyString()
        {
            var signatureInfo = this.Decoder.GetSignatureInfo(TestHelper.GetFirstMethodDefinition());

            var methodLocation = signatureInfo.BaseMethodSignature;

            Assert.NotEmpty(methodLocation);
        }

    }
}
