using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    /// <summary>
    /// An exception that is thrown when trying to interact with a file that is not a PE file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public class PEFileNotSupportedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <b>PEFileNotSupportedException</b> class with its message string set to a system-supplied message.
        /// </summary>
        public PEFileNotSupportedException() { }
        /// <summary>
        /// Initializes a new instance of the <b>PEFileNotSupportedException</b> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public PEFileNotSupportedException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <b>PEFileNotSupportedException</b> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public PEFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
        /// <summary>
        /// Initializes a new instance of the <b>PEFileNotSupportedException</b> class with the specified serialization and context information.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PEFileNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}