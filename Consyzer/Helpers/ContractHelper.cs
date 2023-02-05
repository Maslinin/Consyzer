using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.Analyzers;

namespace Consyzer.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class ContractHelper
    {
        public static IEnumerable<MetadataAnalyzer> ToMetadataAnalyzersFromMetadataAssemblyFiles(this IEnumerable<FileInfo> fileInfos)
        {
            return MetadataFilter.GetMetadataAssemblyFiles(fileInfos).Select(f => new MetadataAnalyzer(f));
        }

    }
}
