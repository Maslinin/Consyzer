using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.Helpers;
using Consyzer.AnalyzerEngine.Analyzers;
using Consyzer.AnalyzerEngine.Analyzers.Searchers;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.Helpers
{
    public static class AnalyzerHelper
    {
        public static IEnumerable<MetadataAnalyzer> GetMetadataAnalyzersFromMetadataAssemblyFiles(IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.GetMetadataAssemblyFiles().Select(f => new MetadataAnalyzer(f.BaseFileInfo.FullName));
        }

        public static IEnumerable<string> GetImportedBinariesLocations(IEnumerable<MetadataAnalyzer> metadataAnalyzers, string defaultBinaryExtension = ".dll")
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
            return binaryLocations.Where(b => !(BinarySearcher.CheckBinaryExistInSourceAndSystemFolder(b, analysisFolder, defaultBinaryExtension) is BinarySearcherStatusCodes.BinaryNotExists));
        }

        public static IEnumerable<string> GetNotExistsBinaries(IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            return binaryLocations.Where(b => (BinarySearcher.CheckBinaryExistInSourceAndSystemFolder(b, analysisFolder, defaultBinaryExtension) is BinarySearcherStatusCodes.BinaryNotExists));
        }

        public static BinarySearcherStatusCodes GetTopBinarySearcherStatusAmongBinaries(IEnumerable<string> binaryLocations, string analysisFolder, string defaultBinaryExtension = ".dll")
        {
            return binaryLocations.Max(b => BinarySearcher.CheckBinaryExistInSourceAndSystemFolder(b, analysisFolder, defaultBinaryExtension));
        }
    }
}
