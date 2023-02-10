using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Consyzer.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class TestConstants
    {
        public static class FileLocation
        {
            public static string MetadataAssemblyLocation => Assembly.GetExecutingAssembly().Location;
            public static string NotMetadataAssemblyLocation => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testhost.exe");
            public static FileInfo MetadataAssemblyFileInfo => new FileInfo(MetadataAssemblyLocation);
            public static FileInfo NotMetadataAssemblyFileInfo => new FileInfo(NotMetadataAssemblyLocation);
        }

        public class TestMethods
        {
            [DllImport("test")]
            public extern static int ImportedMethod(int testArg);
            public static int MethodWithArguments(int testArg) => testArg;
            public int NotStaticMethod(int testArg) => testArg;
        }
    }
}
