using Xunit;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.AnalyzerEngine.Signature;

namespace Consyzer.AnalyzerEngine.Tests.Decoders
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class SignatureExtractorTest
    {
        public TypeDefinition TestTypeDefinition => TestHelper.GetTypeDefinition();
        public IEnumerable<TypeDefinition> AllTestTypesDefinitions => TestHelper.GetAllTypesDefinitions();
        public MethodDefinition TestMethodDefinition => TestHelper.GetFirstMethodDefinition();
        public IEnumerable<MethodDefinition> AllTestMethodsDefinitions => TestHelper.GetAllMethodsDefinitions();

        [Fact]
        public void InstanceCreation_ShouldNotThrowException()
        {
            var exception = Record.Exception(() => new SignatureExtractor(TestConstants.MetadataAssemblyFileInfo));

            Assert.Null(exception);
        }

        [Fact]
        public void GetDecodedSignatures_ReceiveTypeDefinitionInstancesCollection_ShouldReturnNotNullCollection()
        {
            var decoder = new SignatureExtractor(TestConstants.MetadataAssemblyFileInfo);

            var decodedSignatures = decoder.GetDecodedSignatures(this.AllTestTypesDefinitions);

            Assert.NotNull(decodedSignatures);
        }

        [Fact]
        public void GetDecodedSignatures_ReceiveTypeDefinitionInstance_ShouldReturnNotNullCollection()
        {
            var decoder = new SignatureExtractor(TestConstants.MetadataAssemblyFileInfo);

            var decodedSignatures = decoder.GetDecodedSignatures(this.TestTypeDefinition);

            Assert.NotNull(decodedSignatures);
        }

        [Fact]
        public void GetDecodedSignatures_ReceiveMethodDefinitionInstancesCollection_ShouldReturnNotNullCollection()
        {
            var decoder = new SignatureExtractor(TestConstants.MetadataAssemblyFileInfo);

            var decodedSignatures = decoder.GetDecodedSignatures(this.AllTestMethodsDefinitions);

            Assert.NotNull(decodedSignatures);
        }

        [Fact]
        public void GetDecodedSignature_ReceiveMethodDefinitionInstance_ShouldReturnInstanceWithNotNullAndNotEmptyProperties()
        {
            var decoder = new SignatureExtractor(TestConstants.MetadataAssemblyFileInfo);

            var decodedSignature = decoder.GetDecodedSignature(this.TestMethodDefinition);

            Assert.NotNull(decodedSignature);
            Assert.NotEmpty(decodedSignature.Namespace);
            Assert.NotEmpty(decodedSignature.ClassName);
            Assert.NotEmpty(decodedSignature.MethodName);
            Assert.NotNull(decodedSignature.ReturnType);
            Assert.NotNull(decodedSignature.MethodArguments);
            Assert.NotNull(decodedSignature.MethodAttributes);
            Assert.NotNull(decodedSignature.MethodImplAttributes);
        }
    }
}
