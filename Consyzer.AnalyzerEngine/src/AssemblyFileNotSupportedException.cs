using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public sealed class AssemblyFileNotSupportedException : Exception
    {
        public AssemblyFileNotSupportedException() { }
        public AssemblyFileNotSupportedException(string message) : base(message) { }
        public AssemblyFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
    }
}
