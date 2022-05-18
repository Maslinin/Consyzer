using System;
using System.Linq;
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
            int exitCode = (int)WorkStatusCodes.UndefinedBehavior;

            try
            {
                string analysisFolder = OtherHelper.GetDirectoryWithBinariesFromCommandLineArgs();
                NLogger.Info($"Path for analyze: \"{analysisFolder}\".");

                var filesExtensions = OtherHelper.GetBinaryFilesExtensionsFromCommandLineArgs();
                NLogger.Info($"Specified binary file extensions for analysis: {string.Join(", ", filesExtensions)}.");

                var binaryFiles = IOHelper.GetBinaryFilesInfoFrom(analysisFolder, filesExtensions).ToList();
                if(binaryFiles.Any() is false)
                {
                    NLogger.Warn("Binary files for analysis with the specified extensions were not found.");
                    return exitCode;
                }

                NLogger.Info("The following binary files with the specified extensions were found:");
                LoggerHelper.LoggingBaseFileInfo(binaryFiles);
                if (LoggerHelper.LoggingAnalyzedFilesValidityCheck(binaryFiles))
                {
                    return (int)WorkStatusCodes.SuccessExit;
                }

                var sortedFilesAnalyzers = binaryFiles.GetMetadataAssemblyFiles().GetMetadataAnalyzersFromMetadataAssemblyFiles().ToList();
                NLogger.Info("The following assembly binaries containing metadata were found:");
                LoggerHelper.LoggingBaseAndHashFileInfo(sortedFilesAnalyzers);

                NLogger.Info("Information about imported methods from other assemblies in the analyzed files:");
                LoggerHelper.LoggingImportedMethodsInfoForEachBinary(sortedFilesAnalyzers);

                var binaryLocations = sortedFilesAnalyzers.GetImportedBinariesLocations();
                if (binaryLocations.Any() is false)
                {
                    NLogger.Info("All files are missing imported methods from other assemblies.");
                    return (int)WorkStatusCodes.SuccessExit;
                }

                NLogger.Info("The presence of binary files in the received locations:");
                LoggerHelper.LoggingBinariesExistsStatus(binaryLocations, analysisFolder);
                LoggerHelper.LoggingBinaryExistsAndNonExistsCount(binaryLocations, analysisFolder);

                return exitCode = (int)AnalyzerHelper.GetTopBinarySearcherStatusAmongBinaries(binaryLocations, analysisFolder);
            }
            catch(Exception e)
            {
                NLogger.Error(e.ToString());
                return exitCode;
            }
        }
    }
}
