using System.IO;
using System.Reflection;

namespace Consyzer.Cryptography.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class TestConstants
    {
        public static string MetadataAssemblyLocation => Assembly.GetExecutingAssembly().Location;
        public static FileInfo MetadataAssemblyFileInfo => new FileInfo(MetadataAssemblyLocation);
    }
}
