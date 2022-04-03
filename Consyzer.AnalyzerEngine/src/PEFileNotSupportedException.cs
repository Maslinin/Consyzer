using System;
using System.Diagnostics.CodeAnalysis;

namespace Consyzer.AnalyzerEngine
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class PEFileNotSupportedException : Exception
    {
        public PEFileNotSupportedException(string message = "File is not a PE file.") : base(message) { }
        public PEFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
    }
}