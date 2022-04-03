using Xunit;
using System.Linq;
using System.Reflection;
using Consyzer.AnalyzerEngine.Decoder;
using Consyzer.AnalyzerEngine.Analyzer;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.AnalyzerEngine.Tests.Decoder
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class SignatureDecoderTest
    {

        [Fact(DisplayName = "Instance Creation")]
        public void InstanceCreation()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var exceptionOptionOne = Record.Exception(() => new SignatureDecoder(location));
            var exceptionOptionTwo = Record.Exception(() => new SignatureDecoder(new BinaryFileInfo(location)));
            Assert.Null(exceptionOptionOne);
            Assert.Null(exceptionOptionTwo);
        }

        [Fact(DisplayName = "Signature Decoding Check")]
        public void GetDecodedSignature()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);
            var decoder = new SignatureDecoder(mdAnalyzer.MdReader);
            var methodHandle = mdAnalyzer.GetMethodsDefinitionsHandles().First();

            var signatureOptionOne = decoder.GetDecodedSignature(methodHandle);
            var signatureOptionTwo = decoder.GetDecodedSignature(mdAnalyzer.MdReader.GetMethodDefinition(methodHandle));

            Assert.NotNull(signatureOptionOne);
            Assert.NotNull(signatureOptionTwo);
        }

        [Fact(DisplayName = "Signatures Decoding Check")]
        public void GetDecodedSignatures()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);
            var decoder = new SignatureDecoder(mdAnalyzer.MdReader);
            var typesHandles = mdAnalyzer.GetTypesDefinitionsHandles();
            var methodHandles = typesHandles.Select(h => mdAnalyzer.MdReader.GetTypeDefinition(h)).First().GetMethods();

            var signaturesOptionOne = decoder.GetDecodedSignatures(typesHandles);
            var signaturesOptionTwo = decoder.GetDecodedSignatures(typesHandles.First());
            var signaturesOptionThree = decoder.GetDecodedSignatures(mdAnalyzer.MdReader.GetTypeDefinition(typesHandles.First()));
            var signaturesOptionFour = decoder.GetDecodedSignatures(typesHandles.Select(h => mdAnalyzer.MdReader.GetTypeDefinition(h)));
            var signaturesOptionFive = decoder.GetDecodedSignatures(methodHandles);
            var signaturesOptionSix = decoder.GetDecodedSignatures(methodHandles.Select(h => mdAnalyzer.MdReader.GetMethodDefinition(h)));

            Assert.NotNull(signaturesOptionOne);
            Assert.NotNull(signaturesOptionTwo);
            Assert.NotNull(signaturesOptionThree);
            Assert.NotNull(signaturesOptionFour);
            Assert.NotNull(signaturesOptionFive);
            Assert.NotNull(signaturesOptionSix);
        }

    }
}
