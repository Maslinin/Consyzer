using System;
using System.Runtime.Serialization;

namespace Consyzer.File
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public class FileNotSupportedException : Exception
    {
        public FileNotSupportedException() { }
        public FileNotSupportedException(string message) : base(message) { }
        public FileNotSupportedException(string message, Exception ex) : base(message, ex) { }
        protected FileNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
