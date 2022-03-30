using System.Linq;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.Analyzer;
using Consyzer.AnalyzerEngine.Analyzer.SyntaxModels;
using Consyzer.AnalyzerEngine.Analyzer.Searchers;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.Helpers
{
    public static class AnalyzerHelper
    {
        public static IEnumerable<MetadataAnalyzer> GetMetadataAnalyzersFromMetadataAssemblyFiles(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.Select(f => new MetadataAnalyzer(f.BaseFileInfo.FullName));
        }

        public static IEnumerable<string> GetDllLocationsList(this IEnumerable<ImportedMethodInfo> importedMethods)
        {
            return importedMethods.Select(m => m.DllLocation);
        }

    }
}
