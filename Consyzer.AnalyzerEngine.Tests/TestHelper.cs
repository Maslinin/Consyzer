using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Consyzer.AnalyzerEngine.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class TestHelper
    {
        public static MethodDefinition GetFirstMethodDefinition()
        {
            //we take average element of collection because first definitions are technical
            var methodsDefs = GetAllMethodsDefinitions();
            int collectionLength = methodsDefs.Count() / 2;
            return methodsDefs.ElementAt(collectionLength);
        }

        public static IEnumerable<MethodDefinition> GetAllMethodsDefinitions()
        {
            var mdReader = GetMetadataReader();
            var methodsDefinitionsHandles = mdReader.MethodDefinitions;
            return methodsDefinitionsHandles.Select(x => mdReader.GetMethodDefinition(x));
        }

        public static TypeDefinition GetTypeDefinition()
        {
            //we take average element of collection because first definitions are technical
            var typesDefs = GetAllTypesDefinitions();
            int collectionLength = typesDefs.Count() / 2;
            return typesDefs.ElementAt(collectionLength);
        }

        public static IEnumerable<TypeDefinition> GetAllTypesDefinitions()
        {
            var mdReader = GetMetadataReader();
            var typesDefinitionsHandles = mdReader.TypeDefinitions;
            return typesDefinitionsHandles.Select(x => mdReader.GetTypeDefinition(x));
        }

        public static MetadataReader GetMetadataReader()
        {
            var fileStream = new FileStream(TestConstants.MetadataAssemblyFileInfo.FullName, FileMode.Open, FileAccess.Read);
            var peReader = new PEReader(fileStream, PEStreamOptions.Default);

            return peReader.GetMetadataReader();
        }
    }
}
