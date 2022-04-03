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
                NLogger.Info($"Specified binary file extensions for analysis: {string.Join(',', filesExtensions)}.");

                NLogger.Info("Getting binaries at the specified path with the specified extensions...");
                var binaryFiles = IOHelper.GetBinaryFilesInfoFrom(analysisFolder, filesExtensions).ToList();
                if(binaryFiles.Any() is false)
                {
                    NLogger.Warn("No binary files found for analysis.");
                    return exitCode;
                }

                NLogger.Info("The following binary files were found: ");
                binaryFiles.LoggingBaseFileInfo();

                var metadataFiles = binaryFiles.GetFilesContainsMetadata();
                var correctFiles = metadataFiles.GetMetadataAssemblyFiles().GetMetadataAnalyzersFromMetadataAssemblyFiles().ToList();
                if (correctFiles.Any() is false)
                {
                    NLogger.Warn("No analysis files containing metadata were found. All files do not contain metadata.");
                    return exitCode;
                }
                else
                {
                    NLogger.Info("Binary assembly files for analyze containing metadata: ");
                    correctFiles.LoggingBaseAndHashFileInfo();
                }

                var unsuitableFiles = binaryFiles.GetFilesNotContainsMetadata();
                if(unsuitableFiles.Any() is true)
                {
                    NLogger.Info("The following files were excluded from analysis because they DO NOT contain metadata: ");
                    unsuitableFiles.LoggingBaseFileInfo();
                }
                unsuitableFiles = metadataFiles.GetNotMetadataAssemblyFiles();
                if (unsuitableFiles.Any() is true)
                {
                    NLogger.Info("The following files were excluded from analysis because they are NOT assembly files: ");
                    unsuitableFiles.LoggingBaseFileInfo();
                }

                NLogger.Info("Getting information about the content of DLLImport in binary files...");
                correctFiles.LoggingImportedMethodsInfoForEachBinary();

                NLogger.Info("Checking the existence of binary files on the received locations...");
                var binaryLocations = correctFiles.GetImportedBinariesLocations();
                binaryLocations.LoggingBinariesExistsStatus(analysisFolder);

                NLogger.Info($"Total: {binaryLocations.GetExistsBinaries(analysisFolder).Count()} exists, {binaryLocations.GetNotExistsBinaries(analysisFolder).Count()} not exists.");

                exitCode = (int)AnalyzerHelper.GetTopBinarySearcherStatusAmongBinaries(binaryLocations, analysisFolder);
            }
            catch(Exception e)
            {
                NLogger.Error(e.ToString());
                return exitCode;
            }

            return exitCode;
        }
    }
}
