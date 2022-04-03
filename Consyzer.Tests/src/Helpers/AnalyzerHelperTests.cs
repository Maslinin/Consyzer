using Xunit;
using System;
using System.Reflection;
using Consyzer.Helpers;
using Consyzer.AnalyzerEngine.Analyzer;
using Consyzer.AnalyzerEngine.Analyzer.Searchers;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.Tests.Helpers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AnalyzerHelperTests
    {
        [Fact(DisplayName = "Getting MetadataAnalyzer's Collection")]
        public void GetMetadataAnalyzersFromMetadataAssemblyFiles()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var binariesInfo = AnalyzerHelper.GetMetadataAnalyzersFromMetadataAssemblyFiles(new BinaryFileInfo[] { new BinaryFileInfo(location) });
            Assert.NotEmpty(binariesInfo);
        }

        [Fact(DisplayName = "Getting Binaries Locations")]
        public void GetImportedBinariesLocations()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var mdAnalyzers = AnalyzerHelper.GetImportedBinariesLocations(new MetadataAnalyzer[] { new MetadataAnalyzer(location) });
            Assert.NotNull(mdAnalyzers);
        }

        [Fact(DisplayName = "Getting Exists Binaries")]
        public void GetExistsBinaries()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string assemblyFolder = Environment.CurrentDirectory;
            var binaries = AnalyzerHelper.GetExistsBinaries(new string[] { location }, assemblyFolder, ".dll");
            Assert.NotEmpty(binaries);
        }

        [Fact(DisplayName = "Getting Not Exists Binaries")]
        public void GetNotExistsBinaries()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string assemblyFolder = Environment.CurrentDirectory;
            var binaries = AnalyzerHelper.GetNotExistsBinaries(new string[] { location }, assemblyFolder, ".dll");
            Assert.NotNull(binaries);
        }

        [Fact(DisplayName = "Getting Top Binary Searcher Status Among Binaries")]
        public void GetTopBinarySearcherStatusAmongBinaries()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string assemblyFolder = Environment.CurrentDirectory;
            BinarySearcherStatusCodes binaryStatus = AnalyzerHelper.GetTopBinarySearcherStatusAmongBinaries(new string[] { location }, assemblyFolder, ".dll");
            Assert.NotEqual(BinarySearcherStatusCodes.BinaryNotExists, binaryStatus);
        }
    }
}
