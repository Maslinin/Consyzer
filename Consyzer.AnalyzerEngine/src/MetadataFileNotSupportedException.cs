using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    /// <summary>
    /// An exception that is thrown when attempting to interact with a file that does not contain metadata.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public class MetadataFileNotSupportedException : Exception
    {
        public MetadataFileNotSupportedException() { }
        public MetadataFileNotSupportedException(string message) : base(message) { }
        public MetadataFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
        protected MetadataFileNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
