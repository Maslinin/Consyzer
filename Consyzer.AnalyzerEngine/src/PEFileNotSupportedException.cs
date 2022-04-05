using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public sealed class PEFileNotSupportedException : Exception
    {
        public PEFileNotSupportedException() { }
        public PEFileNotSupportedException(string message) : base(message) { }
        public PEFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
    }
}