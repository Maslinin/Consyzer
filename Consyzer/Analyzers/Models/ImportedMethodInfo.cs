using Consyzer.Signature.Models;

namespace Consyzer.Analyzers.Models
{
    /// <summary>
    /// [Sealed] Represents information about a method imported from another assembly.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class ImportedMethodInfo
    {
        /// <summary>
        /// Gets an <b>SignatureInfo</b> instance that contains detailed information about the method imported from another assembly.
        /// </summary>
        public SignatureInfo SignatureInfo { get; internal set; }
        /// <summary>
        /// Gets the location of the assembly on the disk from which the method on the disk was imported.
        /// </summary>
        public string DllLocation { get; internal set; }
        /// <summary>
        /// Gets the arguments of the <b>DLLImport</b> attribute (except the first one) as a string.
        /// </summary>
        public string DllImportArguments { get; internal set; }
    }
}
