﻿using Xunit;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.Metadata;

namespace Consyzer.Tests.Metadata
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class SignatureExtractorTest
    {
        private MethodDefinition TestMethodDefinition => TestHelper.GetFirstMethodDefinition();
        private IEnumerable<MethodDefinition> AllTestMethodsDefinitions => TestHelper.GetAllMethodsDefinitions();
        private MetadataReader TestMetadataReader => TestHelper.GetMetadataReader();

        [Fact]
        public void InstanceCreation_ShouldNotThrowException()
        {
            var exception = Record.Exception(() => new SignatureExtractor(TestMetadataReader));

            Assert.Null(exception);
        }

        [Fact]
        public void GetDecodedSignatures_ReceiveMethodDefinitionInstancesCollection_ShouldReturnNotNullCollection()
        {
            var decoder = new SignatureExtractor(TestMetadataReader);

            var decodedSignatures = decoder.GetDecodedSignatures(this.AllTestMethodsDefinitions);

            Assert.NotNull(decodedSignatures);
        }

        [Fact]
        public void GetDecodedSignature_ReceiveMethodDefinitionInstance_ShouldReturnInstanceWithNotNullAndNotEmptyProperties()
        {
            var decoder = new SignatureExtractor(TestMetadataReader);

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