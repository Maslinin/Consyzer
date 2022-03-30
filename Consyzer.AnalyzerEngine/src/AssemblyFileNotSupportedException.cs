using System;

namespace Consyzer.AnalyzerEngine
{
    [Serializable]
    public class AssemblyFileNotSupportedException : Exception
    {
        public AssemblyFileNotSupportedException(string message = "File is not an assembly.") : base(message) { }
        public AssemblyFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
    }
}
