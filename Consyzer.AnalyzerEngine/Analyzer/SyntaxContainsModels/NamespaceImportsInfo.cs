using System.Collections.Generic;

namespace Consyzer.AnalyzerEngine.Analyzer.SyntaxContainsInfo
{
    public class NamespaceDLLImportsInfo
    {
        public string Namespace { get; set; }
        public IEnumerable<ImportedMethodInfo> ImportedMethodsInfo { get; set; }
    }
}
