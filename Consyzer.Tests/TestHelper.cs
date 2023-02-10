using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests
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

        public static MetadataReader GetMetadataReader()
        {
            var fileStream = new FileStream(MetadataAssemblyFileInfo.FullName, FileMode.Open, FileAccess.Read);
            var peReader = new PEReader(fileStream, PEStreamOptions.Default);

            return peReader.GetMetadataReader();
        }
    }
}
