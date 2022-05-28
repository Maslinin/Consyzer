using Consyzer.Helpers;
using Consyzer.AnalyzerEngine.Helpers;
using Log = Consyzer.Logger.NLogger;

namespace Consyzer
{
    static class Program
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private static int Main()
        {
            try
            {
                string analysisFolder = OtherHelper.GetDirectoryWithBinariesFromCommandLineArgs();
                Log.Info($"Path for analysis: '{analysisFolder}'.");

                var filesExtensions = OtherHelper.GetBinaryFilesExtensionsFromCommandLineArgs();
                LoggerHelper.LoggingFileExtensionsForAnalysis(filesExtensions);

                var binaryFiles = IOHelper.GetBinaryFilesInfoFrom(analysisFolder, filesExtensions);
                if (LoggerCheckerHelper.CheckAndLoggingBinaryFilesExist(binaryFiles) is false)
                    return (int)WorkStatusCodes.SuccessExit;

                Log.Info("The following binary files with the specified extensions were found:");
                LoggerHelper.LoggingBaseFileInfo(binaryFiles);
                if (LoggerCheckerHelper.CheckAndLoggingFilesCorrect(binaryFiles) is false)
                    return (int)WorkStatusCodes.SuccessExit;

                var sortedFilesAnalyzers = AnalyzerHelper.GetMetadataAnalyzersFromMetadataAssemblyFiles(binaryFiles);
                Log.Info("The following assembly binaries containing metadata were found:");
                LoggerHelper.LoggingBaseAndHashFileInfo(sortedFilesAnalyzers);

                Log.Info("Information about imported methods from other assemblies in the analyzed files:");
                LoggerHelper.LoggingImportedMethodsInfoForEachBinary(sortedFilesAnalyzers);

                var binaryLocations = AnalyzerHelper.GetImportedBinariesLocations(sortedFilesAnalyzers);
                if (LoggerCheckerHelper.CheckAndLoggingDllLocationsExist(binaryLocations) is false)
                    return (int)WorkStatusCodes.SuccessExit;

                Log.Info("The presence of binary files in the received locations:");
                LoggerHelper.LoggingBinariesExistStatus(binaryLocations, analysisFolder);
                LoggerHelper.LoggingBinaryExistAndNonExistCount(binaryLocations, analysisFolder);

                return (int)AnalyzerHelper.GetTopBinarySearcherStatusAmongBinaries(binaryLocations, analysisFolder);
            }
            catch(System.Exception e)
            {
                Log.Error(e.ToString());
                return (int)WorkStatusCodes.UnexpectedBehavior;
            }
        }
    }
}
