using System;
using Consyzer.Logger;
using Consyzer.Helpers;
using Consyzer.AnalyzerEngine.Helpers;

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
                NLogger.Info($"Path for analysis: '{analysisFolder}'.");

                var filesExtensions = OtherHelper.GetBinaryFilesExtensionsFromCommandLineArgs();
                NLogger.Info($"Specified binary file extensions for analysis: {string.Join(", ", filesExtensions)}.");

                var binaryFiles = IOHelper.GetBinaryFilesInfoFrom(analysisFolder, filesExtensions);
                if(LoggerCheckerHelper.CheckAndLoggingBinaryFilesExist(binaryFiles) is false)
                    return (int)WorkStatusCodes.SuccessExit;

                NLogger.Info("The following binary files with the specified extensions were found:");
                LoggerHelper.LoggingBaseFileInfo(binaryFiles);
                if (LoggerCheckerHelper.CheckAndLoggingFilesCorrect(binaryFiles) is false)
                    return (int)WorkStatusCodes.SuccessExit;

                var sortedFilesAnalyzers = binaryFiles.GetMetadataAssemblyFiles().GetMetadataAnalyzersFromMetadataAssemblyFiles();
                NLogger.Info("The following assembly binaries containing metadata were found:");
                LoggerHelper.LoggingBaseAndHashFileInfo(sortedFilesAnalyzers);

                NLogger.Info("Information about imported methods from other assemblies in the analyzed files:");
                LoggerHelper.LoggingImportedMethodsInfoForEachBinary(sortedFilesAnalyzers);

                var binaryLocations = sortedFilesAnalyzers.GetImportedBinariesLocations();
                if (LoggerCheckerHelper.CheckAndLoggingDllLocationsExist(binaryLocations) is false)
                    return (int)WorkStatusCodes.SuccessExit;

                NLogger.Info("The presence of binary files in the received locations:");
                LoggerHelper.LoggingBinariesExistStatus(binaryLocations, analysisFolder);
                LoggerHelper.LoggingBinaryExistAndNonExistCount(binaryLocations, analysisFolder);

                return (int)AnalyzerHelper.GetTopBinarySearcherStatusAmongBinaries(binaryLocations, analysisFolder);
            }
            catch(Exception e)
            {
                NLogger.Error(e.ToString());
                return (int)WorkStatusCodes.UnexpectedBehavior;
            }
        }
    }
}
