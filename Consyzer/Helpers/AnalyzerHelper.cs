using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.Searchers;
using Consyzer.AnalyzerEngine.Analyzers;

namespace Consyzer.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class AnalyzerHelper
    {
        public static IEnumerable<string> GetImportedMethodsLocations(IEnumerable<MetadataAnalyzer> metadataAnalyzers, string defaultBinaryExtension = ".dll")
        {
            var importedMethods = new List<string>();

            foreach (var method in metadataAnalyzers)
            {
                importedMethods.AddRange(method.GetImportedMethodsInfo().Select(m => Path.HasExtension(m.DllLocation) ? m.DllLocation : $"{m.DllLocation}{defaultBinaryExtension}" ));
            }

            return importedMethods.Distinct();
        }

        public static IEnumerable<string> GetExistsBinaries(IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            return binaryLocations.Where(b => !(new FileSearcher(analysisFolder).GetFileLocation(b, defaultBinaryExtension) is FileExistanceStatusCode.FileDoesNotExists));
        }

        public static IEnumerable<string> GetNotExistsBinaries(IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            return binaryLocations.Where(b => (new FileSearcher(analysisFolder).GetFileLocation(b, defaultBinaryExtension) is FileExistanceStatusCode.FileDoesNotExists));
        }

        public static FileExistanceStatusCode GetTopBinarySearcherStatusAmongBinaries(IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            return binaryLocations.Max(b => new FileSearcher(analysisFolder).GetFileLocation(b, defaultBinaryExtension));
        }
    }
}
