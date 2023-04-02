using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.Extractors;
using Consyzer.Extractors.Models;

namespace Consyzer.Tests.Extractors
{
    public sealed class SignatureInfoExtractorTest
    {
        public MetadataReader MetadataReader => TestHelper.GetMetadataReader();
        public MethodDefinition TestMethodDefinition => TestHelper.GetFirstMethodDefinition();
        public IEnumerable<MethodDefinition> TestMethodsDefinitions => TestHelper.GetAllMethodsDefinitions();

        [Fact]
        public void InstanceCreation_ShouldNotThrowException()
        {
            var exception = Record.Exception(() => new SignatureInfoExtractor(this.MetadataReader));

            Assert.Null(exception);
        }

        [Fact]
        public void GetSignatureInfo_ReceiveMethodDefinitionInstance_ShouldReturnInstanceWithNotNullAndNotEmptyProperties()
        {
            var decoder = new SignatureInfoExtractor(MetadataReader);

            var decodedSignature = decoder.GetSignatureInfo(this.TestMethodDefinition);

            Assert.NotNull(decodedSignature);
            Assert.NotEmpty(decodedSignature.Namespace);
            Assert.NotEmpty(decodedSignature.ClassName);
            Assert.NotEmpty(decodedSignature.MethodName);
            Assert.NotNull(decodedSignature.ReturnType);
            Assert.NotNull(decodedSignature.MethodArguments);
        }

        [Fact]
        public void GetNamespace_ShouldReturnNotEmptyString()
        {
            var extractor = new SignatureInfoExtractor(this.MetadataReader);

            var rootNamespace = extractor.GetNamespace(this.TestMethodDefinition);

            Assert.NotEmpty(rootNamespace);
        }

        [Fact]
        public void GetClassName_ShouldReturnNotEmptyString()
        {
            var extractor = new SignatureInfoExtractor(this.MetadataReader);

            var className = extractor.GetClassName(this.TestMethodDefinition);

            Assert.NotEmpty(className);
        }

        [Fact]
        public void GetMethodName_ShouldReturnNotEmptyString()
        {
            var extractor = new SignatureInfoExtractor(this.MetadataReader);

            var methodName = extractor.GetMethodName(this.TestMethodDefinition);

            Assert.NotEmpty(methodName);
        }

        [Fact]
        public void GetMethodAccessibilityModifier_ShouldReturnPublicModifier()
        {
            var extractor = new SignatureInfoExtractor(this.MetadataReader);

            var methodAccessibilityModifier = this.TestMethodsDefinitions.Any(m => extractor.GetMethodAccessibilityModifier(m) == AccessModifier.Public);

            Assert.True(methodAccessibilityModifier);
        }

        [Fact]
        public void IsStaticMethod_ShouldReturnTrue()
        {
            var extractor = new SignatureInfoExtractor(this.MetadataReader);

            var isStatic = this.TestMethodsDefinitions.Any(m => extractor.IsStaticMethod(m));

            Assert.True(isStatic);
        }

        [Fact]
        public void GetMethodReturnType_ShouldReturnSignatureParameterInstanceWithNotEmptyProperties()
        {
            var extractor = new SignatureInfoExtractor(this.MetadataReader);

            var methodReturnType = extractor.GetMethodReturnType(this.TestMethodDefinition);

            Assert.NotEmpty(methodReturnType.Type);
            Assert.NotEmpty(methodReturnType.Attributes);
            Assert.NotEmpty(methodReturnType.Name);
        }

        [Fact]
        public void GetMethodArguments_ShouldReturnNotNullCollection()
        {
            var extractor = new SignatureInfoExtractor(this.MetadataReader);

            var methodArguments = extractor.GetMethodArguments(this.TestMethodDefinition);

            Assert.NotNull(methodArguments);
        }
    }
}
