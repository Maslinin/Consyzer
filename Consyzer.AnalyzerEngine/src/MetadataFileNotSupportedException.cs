using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public class MetadataFileNotSupportedException : Exception, ISerializable
    {
        public MetadataFileNotSupportedException() { }
        public MetadataFileNotSupportedException(string message) : base(message) { }
        public MetadataFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
        protected MetadataFileNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
