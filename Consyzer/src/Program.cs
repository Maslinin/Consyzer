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
                binaryFiles.LoggingBaseFileInfo();

                var metadataFiles = binaryFiles.GetFilesContainsMetadata();

                var unsuitableFiles = binaryFiles.GetFilesNotContainsMetadata();
                if (unsuitableFiles.Any() is true)
                {
                    NLogger.Info("The following files were excluded from analysis because they DO NOT contain metadata:");
                    unsuitableFiles.LoggingBaseFileInfo();
                }
                if (metadataFiles.Count() == unsuitableFiles.Count())
                {
                    NLogger.Warn("All found files do NOT contain metadata.");
                    return (int)WorkStatusCodes.SuccessExit;
                }
                unsuitableFiles = metadataFiles.GetNotMetadataAssemblyFiles();
                if (unsuitableFiles.Any() is true)
                {
                    NLogger.Info("The following files were excluded from analysis because they are NOT assembly files:");
                    unsuitableFiles.LoggingBaseFileInfo();
                }
                if (metadataFiles.Count() == unsuitableFiles.Count())
                {
                    NLogger.Warn("All found files contain metadata, but are NOT assembly files.");
                    return (int)WorkStatusCodes.SuccessExit;
                }

                var sortedFilesAnalyzers = metadataFiles.GetMetadataAssemblyFiles().GetMetadataAnalyzersFromMetadataAssemblyFiles().ToList();
                NLogger.Info("The following assembly binaries containing metadata were found:");
                sortedFilesAnalyzers.LoggingBaseAndHashFileInfo();

                NLogger.Info("Information about imported methods from other assemblies in the analyzed files:");
                sortedFilesAnalyzers.LoggingImportedMethodsInfoForEachBinary();

                var binaryLocations = sortedFilesAnalyzers.GetImportedBinariesLocations();
                if (binaryLocations.Any() is false)
                {
                    NLogger.Info("All files are missing imported methods from other assemblies.");
                    return (int)WorkStatusCodes.SuccessExit;
                }
                NLogger.Info("The presence of binary files in the received locations:");
                binaryLocations.LoggingBinariesExistsStatus(analysisFolder);

                NLogger.Info($"Total: {binaryLocations.GetExistsBinaries(analysisFolder).Count()} exists, {binaryLocations.GetNotExistsBinaries(analysisFolder).Count()} not exists.");

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
