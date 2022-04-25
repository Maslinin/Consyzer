using System;
using System.Runtime.Serialization;

namespace Consyzer.AnalyzerEngine
{
    /// <summary>
    /// An exception that is thrown when attempting to interact with a non-assembly file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Serializable]
    public class AssemblyFileNotSupportedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <b>AssemblyFileNotSupportedException</b> class with its message string set to a system-supplied message.
        /// </summary>
        public AssemblyFileNotSupportedException() { }
        /// <summary>
        /// Initializes a new instance of the <b>AssemblyFileNotSupportedException</b> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public AssemblyFileNotSupportedException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <b>AssemblyFileNotSupportedException</b> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public AssemblyFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
        /// <summary>
        /// Initializes a new instance of the <b>AssemblyFileNotSupportedException</b> class with the specified serialization and context information.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected AssemblyFileNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
