using Consyzer.AnalyzerEngine.Decoder.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Analyzer.SyntaxModels
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class ImportedMethodInfo
    {
        public SignatureInfo SignatureInfo { get; }
        public string DllLocation { get; }
        public string DllImportArguments { get; }

        public ImportedMethodInfo() { }

        public ImportedMethodInfo(SignatureInfo SignatureInfo, string DllLocation, string DllImportArguments)
        {
            this.SignatureInfo = SignatureInfo;
            this.DllLocation = DllLocation;
            this.DllImportArguments = DllImportArguments;
        }
    }
}
