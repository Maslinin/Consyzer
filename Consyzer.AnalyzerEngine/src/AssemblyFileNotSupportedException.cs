using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public class AssemblyFileNotSupportedException : Exception, ISerializable
    {
        public AssemblyFileNotSupportedException() { }
        public AssemblyFileNotSupportedException(string message) : base(message) { }
        public AssemblyFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
        protected AssemblyFileNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
