using System;
using System.IO;
using System.Reflection;

namespace Consyzer.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class TestConstants
    {
        public static string MetadataAssemblyLocation => Assembly.GetExecutingAssembly().Location;
        public static FileInfo MetadataAssemblyFileInfo => new FileInfo(MetadataAssemblyLocation);
        public static string NotMetadataAssemblyLocation => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testhost.exe");
        public static FileInfo NotMetadataAssemblyFileInfo => new FileInfo(NotMetadataAssemblyLocation);
    }
}
