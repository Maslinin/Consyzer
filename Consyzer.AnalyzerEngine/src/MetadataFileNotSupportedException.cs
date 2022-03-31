using System;

namespace Consyzer.AnalyzerEngine
{
    [Serializable]
    public class MetadataFileNotSupportedException : Exception
    {
        public MetadataFileNotSupportedException(string message = "File does not contain metadata.") : base(message) { }
        public MetadataFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
    }
}
