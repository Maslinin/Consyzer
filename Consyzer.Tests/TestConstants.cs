using System.IO;
using System.Reflection;

namespace Consyzer.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class TestConstants
    {
        public static string MetadataAssemblyLocation => Assembly.GetExecutingAssembly().Location;
        public static FileInfo MetadataAssemblyFileInfo => new FileInfo(MetadataAssemblyLocation);
    }
}
