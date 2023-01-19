using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.Analyzers;

namespace Consyzer.Contracts
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class FileInfoEntityConversionContract
    {
        public static IEnumerable<MetadataAnalyzer> ToMetadataAnalyzersFromMetadataAssemblyFiles(this IEnumerable<FileInfo> fileInfos)
        {
            return MetadataFilter.GetMetadataAssemblyFiles(fileInfos).Select(f => new MetadataAnalyzer(f));
        }

        public static IEnumerable<ImportedMethodsAnalyzer> ToImportedMethodsAnalyzersFromMetadataAssemblyFiles(this IEnumerable<FileInfo> fileInfos)
        {
            return MetadataFilter.GetMetadataAssemblyFiles(fileInfos).Select(f => new ImportedMethodsAnalyzer(f));
        }
    }
}
