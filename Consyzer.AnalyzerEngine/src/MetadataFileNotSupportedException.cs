using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public sealed class MetadataFileNotSupportedException : Exception
    {
        public MetadataFileNotSupportedException() { }
        public MetadataFileNotSupportedException(string message) : base(message) { }
        public MetadataFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
    }
}
