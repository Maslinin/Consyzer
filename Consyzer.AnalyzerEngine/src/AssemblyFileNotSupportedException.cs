using System;
using System.Diagnostics.CodeAnalysis;

namespace Consyzer.AnalyzerEngine
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class AssemblyFileNotSupportedException : Exception
    {
        public AssemblyFileNotSupportedException(string message = "File is not an assembly.") : base(message) { }
        public AssemblyFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
    }
}
