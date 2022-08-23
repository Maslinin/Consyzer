using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    /// <summary>
    /// An exception that is thrown when attempting to interact with a non-assembly file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public class AssemblyFileNotSupportedException : Exception
    {
        public AssemblyFileNotSupportedException() { }
        public AssemblyFileNotSupportedException(string message) : base(message) { }
        public AssemblyFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
        protected AssemblyFileNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
