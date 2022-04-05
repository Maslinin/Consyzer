using System;

namespace Consyzer.AnalyzerEngine
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PEFileNotSupportedException : Exception
    {
        public PEFileNotSupportedException() { }
        public PEFileNotSupportedException(string message) : base(message) { }
        public PEFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
    }
}