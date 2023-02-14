using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.Metadata;
using Consyzer.Metadata.Models;

namespace Consyzer.Tests.Metadata
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class MethodInfoExtractorTest
    {
        public MetadataReader MetadataReader => TestHelper.GetMetadataReader();
        public MethodDefinition TestMethodDefinition => TestHelper.GetFirstMethodDefinition();
        public IEnumerable<MethodDefinition> TestMethodsDefinitions => TestHelper.GetAllMethodsDefinitions();

        [Fact]
        public void InstanceCreation_ShouldNotThrowException()
        {
            var exception = Record.Exception(() => new MethodInfoExtractor(this.MetadataReader));

            Assert.Null(exception);
        }

        [Fact]
        public void GetNamespace_ShouldReturnNotEmptyString()
        {
            var extractor = new MethodInfoExtractor(this.MetadataReader);

            var rootNamespace = extractor.GetNamespace(this.TestMethodDefinition);

            Assert.NotEmpty(rootNamespace);
        }

        [Fact]
        public void GetClassName_ShouldReturnNotEmptyString()
        {
            var extractor = new MethodInfoExtractor(this.MetadataReader);

            var className = extractor.GetClassName(this.TestMethodDefinition);

            Assert.NotEmpty(className);
        }

        [Fact]
        public void GetMethodName_ShouldReturnNotEmptyString()
        {
            var extractor = new MethodInfoExtractor(this.MetadataReader);

            var methodName = extractor.GetMethodName(this.TestMethodDefinition);

            Assert.NotEmpty(methodName);
        }

        [Fact]
        public void GetMethodAccessibilityModifier_ShouldReturnPublicModifier()
        {
            var extractor = new MethodInfoExtractor(this.MetadataReader);

            var methodAccessibilityModifier = this.TestMethodsDefinitions.Any(m => extractor.GetMethodAccessibilityModifier(m) == AccessModifier.Public);

            Assert.True(methodAccessibilityModifier);
        }

        [Fact]
        public void IsStaticMethod_ShouldReturnTrue()
        {
            var extractor = new MethodInfoExtractor(this.MetadataReader);

            var isStatic = this.TestMethodsDefinitions.Any(m => extractor.IsStaticMethod(m));

            Assert.True(isStatic);
        }

        [Fact]
        public void GetMethodAttributes_ShouldReturnNotEmptyString()
        {
            var extractor = new MethodInfoExtractor(this.MetadataReader);

            var methodAttributes = extractor.GetMethodAttributes(this.TestMethodDefinition);

            Assert.NotEmpty(methodAttributes);
        }

        [Fact]
        public void GetMethodImplAttributes_ShouldReturnNotEmptyString()
        {
            var extractor = new MethodInfoExtractor(this.MetadataReader);

            var methodImplAttributes = extractor.GetMethodImplAttributes(this.TestMethodDefinition);

            Assert.NotEmpty(methodImplAttributes);
        }

        [Fact]
        public void GetMethodReturnType_ShouldReturnSignatureParameterInstanceWithNotEmptyProperties()
        {
            var extractor = new MethodInfoExtractor(this.MetadataReader);

            var methodReturnType = extractor.GetMethodReturnType(this.TestMethodDefinition);

            Assert.NotEmpty(methodReturnType.Type);
            Assert.NotEmpty(methodReturnType.Attributes);
            Assert.NotEmpty(methodReturnType.Name);
        }

        [Fact]
        public void GetMethodArguments_ShouldReturnNotNullCollection()
        {
            var extractor = new MethodInfoExtractor(this.MetadataReader);

            var methodArguments = extractor.GetMethodArguments(this.TestMethodDefinition);

            Assert.NotNull(methodArguments);
        }
    }
}
