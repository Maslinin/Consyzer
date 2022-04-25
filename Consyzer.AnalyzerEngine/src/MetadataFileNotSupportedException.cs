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
        /// <summary>
        /// Initializes a new instance of the <b>MetadataFileNotSupportedException</b> class with its message string set to a system-supplied message.
        /// </summary>
        public MetadataFileNotSupportedException() { }
        /// <summary>
        /// Initializes a new instance of the <b>MetadataFileNotSupportedException</b> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public MetadataFileNotSupportedException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <b>MetadataFileNotSupportedException</b> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public MetadataFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
        /// <summary>
        /// Initializes a new instance of the <b>MetadataFileNotSupportedException</b> class with the specified serialization and context information.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected MetadataFileNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
