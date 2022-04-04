using Xunit;
using System.Linq;
using System.Reflection;
using Consyzer.AnalyzerEngine.Analyzer;
using Consyzer.AnalyzerEngine.Decoder;
using Consyzer.AnalyzerEngine.Decoder.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Tests.Decoder.SyntaxModels
{
    public class SignatureInfoTest
    {
        [Fact(DisplayName = "Instance Creation")]
        public void InstanceCreation()
        {
            var exception = Record.Exception(() => new SignatureInfo());
            Assert.Null(exception);
        }

        [Fact(DisplayName = "Getting Method Location")]
        public void GetMethodLocation()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);
            var decoder = new SignatureDecoder(mdAnalyzer.MdReader);
            string methodLocation = decoder.GetDecodedSignature(mdAnalyzer.GetMethodsDefinitions().First()).GetMethodLocation();
            Assert.NotEmpty(methodLocation);
        }

        [Fact(DisplayName = "Getting Method Arguments")]
        public void GetMethodArgsAsString()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);
            var decoder = new SignatureDecoder(mdAnalyzer.MdReader);
            string methodArgs = decoder.GetDecodedSignature(mdAnalyzer.GetMethodsDefinitions().First()).GetMethodArgsAsString();
            Assert.NotEmpty(methodArgs);
        }
        
        [Fact(DisplayName = "Getting Base Method Signature")]
        public void GetBaseMethodSignature()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);
            var decoder = new SignatureDecoder(mdAnalyzer.MdReader);
            string baseMethodSignature = decoder.GetDecodedSignature(mdAnalyzer.GetMethodsDefinitions().First()).GetBaseMethodSignature();
            Assert.NotEmpty(baseMethodSignature);
        }

        [Fact(DisplayName = "Getting Full Method Signature")]
        public void GetFullMethodSignature()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);
            var decoder = new SignatureDecoder(mdAnalyzer.MdReader);
            string baseMethodSignature = decoder.GetDecodedSignature(mdAnalyzer.GetMethodsDefinitions().First()).GetFullMethodSignature();
            Assert.NotEmpty(baseMethodSignature);
        }
    }
}
