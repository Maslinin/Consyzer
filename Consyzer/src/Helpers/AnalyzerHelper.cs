using System.Linq;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.Analyzer;
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

        public static IEnumerable<string> GetBinaryLocations(this IEnumerable<MetadataAnalyzer> metadataAnalyzers)
        {
            var importedMethods = new List<string>();

            foreach (var method in metadataAnalyzers.Select(File => File ))
            {
                importedMethods.AddRange(method.GetImportedMethodsInfo().Select(m => m.DllLocation));
            }

            return importedMethods.Distinct();
        }

        public static IEnumerable<string> GetExistsBinaries(this IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            return binaryLocations.Where(b => !(BinarySearcher.CheckBinaryExist(b, analysisFolder, defaultBinaryExtension) is BinarySearcherStatusCodes.BinaryNotExists));
        }

        public static IEnumerable<string> GetNotExistsBinaries(this IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            return binaryLocations.Where(b => (BinarySearcher.CheckBinaryExist(b, analysisFolder, defaultBinaryExtension) is BinarySearcherStatusCodes.BinaryNotExists));
        }

        public static BinarySearcherStatusCodes GetTopBinarySearcherStatusAmongBinaries(IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            return binaryLocations.Max(b => BinarySearcher.CheckBinaryExist(b, analysisFolder, defaultBinaryExtension));
        }

    }
}
