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
                LoggerHelper.LoggingPathToBinariesForAnalysis(analysisFolder);

                var filesExtensions = OtherHelper.GetBinaryFilesExtensionsFromCommandLineArgs();
                LoggerHelper.LoggingFilesExtensionsForAnalysis(filesExtensions);

                var binaryFiles = IOHelper.GetBinaryFilesInfoFrom(analysisFolder, filesExtensions);
                if (!LoggerCheckerHelper.CheckAndLoggingBinaryFilesExist(binaryFiles))
                    return (int)WorkStatusCodes.SuccessExit;

                Log.Info("The following binary files with the specified extensions were found:");
                LoggerHelper.LoggingBaseFileInfo(binaryFiles);
                if (!LoggerCheckerHelper.CheckAndLoggingFilesCorrect(binaryFiles))
                    return (int)WorkStatusCodes.SuccessExit;

                var metadataAnalyzers = AnalyzerHelper.GetMetadataAnalyzersFromMetadataAssemblyFiles(binaryFiles);
                Log.Info("The following assembly binaries containing metadata were found:");
                LoggerHelper.LoggingBaseAndHashFileInfo(metadataAnalyzers);

                Log.Info("Information about imported methods from other assemblies in the analyzed files:");
                LoggerHelper.LoggingImportedMethodsInfoForEachBinary(metadataAnalyzers);

                var binaryLocations = AnalyzerHelper.GetImportedMethodsLocations(metadataAnalyzers);
                if (!LoggerCheckerHelper.CheckAndLoggingAnyBinariesExist(binaryLocations))
                    return (int)WorkStatusCodes.SuccessExit;

                Log.Info("The presence of binary files in the received locations:");
                LoggerHelper.LoggingBinariesExistStatus(binaryLocations, analysisFolder);
                LoggerHelper.LoggingExistAndNonExistBinariesCount(binaryLocations, analysisFolder);

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
