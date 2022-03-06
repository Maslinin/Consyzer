namespace Consyzer.AnalyzerEngine.Analyzer.SyntaxContainsInfo
{
    public sealed class ImportedMethodInfo
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string MethodSignature { get; set; }
        public string DLLPosition { get; set; }
        public string DLLImportArguments { get; set; }
    }
}
