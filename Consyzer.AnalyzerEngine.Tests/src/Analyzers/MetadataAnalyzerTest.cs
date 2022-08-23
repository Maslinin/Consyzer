using Xunit;
using System.Reflection;
using Consyzer.AnalyzerEngine.Analyzers;
using Consyzer.AnalyzerEngine.IO;

namespace Consyzer.AnalyzerEngine.Tests.Analyzers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class MetadataAnalyzerTest
    {
        [Fact(DisplayName = "Instance Creation")]
        public void InstanceCreation()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            var exceptionOverloadOne = Record.Exception(() => new MetadataAnalyzer(location));
            var exceptionOverloadTwo = Record.Exception(() => new MetadataAnalyzer(new BinaryFileInfo(location)));

            Assert.Null(exceptionOverloadOne);
            Assert.Null(exceptionOverloadTwo);
        }

        [Fact(DisplayName = "Getting Imported Methods Definitions Handles")]
        public void GetImportedMethodsDefinitionsHandles ()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);

            var importMethodsDefs = mdAnalyzer.GetImportedMethodsDefinitionsHandles();

            Assert.NotNull(importMethodsDefs);
        }

        [Fact(DisplayName = "Getting Imported Methods Definitions")]
        public void GetImportedMethodsDefinitions()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);

            var importMethodsDefs = mdAnalyzer.GetImportedMethodsDefinitions();

            Assert.NotNull(importMethodsDefs);
        }

        [Fact(DisplayName = "Getting Imported Methods Info")]
        public void GetImportedMethodsInfo()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);

            var importMethods = mdAnalyzer.GetImportedMethodsInfo();

            Assert.NotNull(importMethods);
        }

        [Fact(DisplayName = "Getting Types Definitions Handles")]
        public void GetTypesDefinitionsHandles()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);

            var typesDefsHandles = mdAnalyzer.GetTypesDefinitionsHandles();

            Assert.NotNull(typesDefsHandles);
        }

        [Fact(DisplayName = "Getting Types Definitions")]
        public void GetTypesDefinitions()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);

            var typesDefs = mdAnalyzer.GetTypesDefinitions();

            Assert.NotNull(typesDefs);
        }

        [Fact(DisplayName = "Getting Methods Definitions Handles")]
        public void GetMethodsDefinitionsHandles()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);

            var methodsDefsHandles = mdAnalyzer.GetMethodsDefinitionsHandles();

            Assert.NotNull(methodsDefsHandles);
        }

        [Fact(DisplayName = "Getting Methods Definition")]
        public void GetMethodsDefinitions()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzer = new MetadataAnalyzer(location);

            var methodsDefs = mdAnalyzer.GetMethodsDefinitions();

            Assert.NotNull(methodsDefs);
        }
    }
}
