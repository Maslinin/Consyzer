using System;

namespace Consyzer.AnalyzerEngine
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AssemblyFileNotSupportedException : Exception
    {
        public AssemblyFileNotSupportedException() { }
        public AssemblyFileNotSupportedException(string message) : base(message) { }
        public AssemblyFileNotSupportedException(string message, Exception ex) : base(message, ex) { }

    }
}
