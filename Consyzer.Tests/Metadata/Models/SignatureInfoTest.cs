using Xunit;
using System.Linq;
using Consyzer.Metadata;
using System.Reflection;

namespace Consyzer.Tests.Metadata.Models
{
    public sealed class SignatureInfoTest
    {
        private MethodInfoExtractor Decoder => new MethodInfoExtractor(TestHelper.GetMetadataReader());
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
            var extractor = new MethodInfoExtractor(TestHelper.GetMetadataReader());
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
            var extractor = new MethodInfoExtractor(TestHelper.GetMetadataReader());
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
