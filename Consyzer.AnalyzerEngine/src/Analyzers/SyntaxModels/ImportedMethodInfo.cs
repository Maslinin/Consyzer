using Consyzer.AnalyzerEngine.Decoders.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Analyzers.SyntaxModels
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
        public SignatureInfo SignatureInfo { get; }
        /// <summary>
        /// Gets the location of the assembly on the disk from which the method on the disk was imported.
        /// </summary>
        public string DllLocation { get; }
        /// <summary>
        /// Gets the arguments of the <b>DLLImport</b> attribute (except the first one) as a string.
        /// </summary>
        public string DllImportArguments { get; }

        /// <summary>
        /// Initializes a new instance of <b>ImportedMethodInfo</b>.
        /// </summary>
        /// <param name="SignatureInfo"></param>
        /// <param name="DllLocation"></param>
        /// <param name="DllImportArguments"></param>
        internal ImportedMethodInfo(SignatureInfo SignatureInfo, string DllLocation, string DllImportArguments)
        {
            this.SignatureInfo = SignatureInfo;
            this.DllLocation = DllLocation;
            this.DllImportArguments = DllImportArguments;
        }
    }
}
