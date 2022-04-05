using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public class PEFileNotSupportedException : Exception, ISerializable
    {
        public PEFileNotSupportedException() { }
        public PEFileNotSupportedException(string message) : base(message) { }
        public PEFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
        protected PEFileNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}