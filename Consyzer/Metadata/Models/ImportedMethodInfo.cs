namespace Consyzer.Metadata.Models
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class ImportedMethodInfo
    {
        public SignatureInfo Signature { get; internal set; }
        public string DllLocation { get; internal set; }
        public string DllImportArgs { get; internal set; }
    }
}
