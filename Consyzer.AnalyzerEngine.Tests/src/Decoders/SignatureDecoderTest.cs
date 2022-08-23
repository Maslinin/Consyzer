using Xunit;
using System.Linq;
using System.Reflection;
using Consyzer.AnalyzerEngine.Decoders;
using Consyzer.AnalyzerEngine.Analyzers;
using Consyzer.AnalyzerEngine.IO;

namespace Consyzer.AnalyzerEngine.Tests.Decoders
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class SignatureDecoderTest
    {

        [Fact(DisplayName = "Instance Creation")]
        public void InstanceCreation()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var exceptionOverloadOne = Record.Exception(() => new SignatureDecoder(location));
            var exceptionOverloadTwo = Record.Exception(() => new SignatureDecoder(new BinaryFileInfo(location)));
            Assert.Null(exceptionOverloadOne);
            Assert.Null(exceptionOverloadTwo);
        }

        [Fact(DisplayName = "Signature Decoding Check")]
        public void GetDecodedSignature()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);
            var decoder = new SignatureDecoder(mdAnalyzer.MdReader);
            var methodHandle = mdAnalyzer.GetMethodsDefinitionsHandles().First();

            var signatureOverloadOne = decoder.GetDecodedSignature(methodHandle);
            var signatureOverloadTwo = decoder.GetDecodedSignature(mdAnalyzer.MdReader.GetMethodDefinition(methodHandle));

            Assert.NotNull(signatureOverloadOne);
            Assert.NotNull(signatureOverloadTwo);
        }

        [Fact(DisplayName = "Signatures Decoding Check")]
        public void GetDecodedSignatures()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);
            var decoder = new SignatureDecoder(mdAnalyzer.MdReader);
            var typesHandles = mdAnalyzer.GetTypesDefinitionsHandles();
            var methodHandles = typesHandles.Select(h => mdAnalyzer.MdReader.GetTypeDefinition(h)).First().GetMethods();

            var signaturesOverloadOne = decoder.GetDecodedSignatures(typesHandles);
            var signaturesOverloadTwo = decoder.GetDecodedSignatures(typesHandles.First());
            var signaturesOverloadThree = decoder.GetDecodedSignatures(mdAnalyzer.MdReader.GetTypeDefinition(typesHandles.First()));
            var signaturesOverloadFour = decoder.GetDecodedSignatures(typesHandles.Select(h => mdAnalyzer.MdReader.GetTypeDefinition(h)));
            var signaturesOverloadFive = decoder.GetDecodedSignatures(methodHandles);
            var signaturesOverloadSix = decoder.GetDecodedSignatures(methodHandles.Select(h => mdAnalyzer.MdReader.GetMethodDefinition(h)));

            Assert.NotNull(signaturesOverloadOne);
            Assert.NotNull(signaturesOverloadTwo);
            Assert.NotNull(signaturesOverloadThree);
            Assert.NotNull(signaturesOverloadFour);
            Assert.NotNull(signaturesOverloadFive);
            Assert.NotNull(signaturesOverloadSix);
        }

    }
}
