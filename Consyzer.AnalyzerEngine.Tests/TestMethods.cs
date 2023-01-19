using System.Runtime.InteropServices;

namespace Consyzer.AnalyzerEngine.Tests
{
    internal class TestMethods
    {
        public static int TestMethodWithArguments(int testArg) => testArg;

        [DllImport("test")]
        public extern static int TestImportedMethod(int testArg);

        public int TestNotStaticMethod(int testArg) => testArg;
    }
}
