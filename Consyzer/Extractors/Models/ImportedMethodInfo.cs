namespace Consyzer.Extractors.Models;

internal sealed class ImportedMethodInfo
{
    public SignatureInfo Signature { get; set; }
    public string DllLocation { get; set; }
    public string DllImportArgs { get; set; }
}