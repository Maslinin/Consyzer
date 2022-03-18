using Consyzer.AnalyzerEngine.Decoder.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Analyzer.SyntaxModels
{
    public sealed class ImportedMethodInfo
    {
        public SignatureInfo SignatureInfo { get; }
        public string DLLPosition { get; }
        public string DLLImportArguments { get; }

        public ImportedMethodInfo() { }

        public ImportedMethodInfo(SignatureInfo SignatureInfo, string DLLPosition, string DLLImportArguments)
        {
            this.SignatureInfo = SignatureInfo;
            this.DLLPosition = DLLPosition;
            this.DLLImportArguments = DLLImportArguments;
        }
    }
}
