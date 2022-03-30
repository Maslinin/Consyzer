using System;
using System.Linq;
using Consyzer.Logger;
using Consyzer.Config;
using Consyzer.Helpers;
using Consyzer.AnalyzerEngine.Helpers;
using Consyzer.AnalyzerEngine.Analyzer.Searchers;

namespace Consyzer
{
    static class Program
    {
        private static int Main()
        {
            try
            {
                string pathToAnalyze = OtherHelper.GetDirectoryWithBinariesFromCommandLineArgs();
                NLogger.Info($"Path for analyze: \"{pathToAnalyze}\".");

                NLogger.Info("Loading Configuration...");
                var config = ConfigHelper.GetConfig(ConsyzerConfig.DefaultConfigPath);
                NLogger.Info("Configuration file was successfully loaded.");

                NLogger.Info($"Specified binary file extensions for analysis: {config.GetBinaryFilesExtensions()}.");
                NLogger.Info("Getting binaries at the specified path with the specified extensions...");
                var binaryFiles = IOHelper.GetBinaryFilesInfoFrom(pathToAnalyze, config.BinaryFilesExtensions).ToList();
                if(binaryFiles.Any() is false)
                {
                    NLogger.Warn("No binary files found for analysis.");
                    return (int)WorkStatusCodes.UndefinedBehavior;
                }

                NLogger.Info("The following binary files were found: ");
                binaryFiles.DisplayBaseFileInfo();

                var metadataFiles = binaryFiles.GetFilesContainsMetadata();
                var correctFiles = metadataFiles.GetMetadataAssemblyFiles().GetMetadataAnalyzersFromMetadataAssemblyFiles().ToList();
                if (correctFiles.Any() is false)
                {
                    NLogger.Warn("No analysis files containing metadata were found. All files do not contain metadata.");
                    return (int)WorkStatusCodes.UndefinedBehavior;
                }
                else
                {
                    NLogger.Info("Binary assembly files for analyze containing metadata: ");
                    correctFiles.DisplayBaseAndHashFileInfo();
                }

                var unsuitableFiles = binaryFiles.GetFilesNotContainsMetadata();
                if(unsuitableFiles.Any() is true)
                {
                    NLogger.Info("The following files were excluded from analysis because they DO NOT contain metadata: ");
                    unsuitableFiles.DisplayBaseFileInfo();
                }
                unsuitableFiles = metadataFiles.GetNotMetadataAssemblyFiles();
                if (unsuitableFiles.Any() is true)
                {
                    NLogger.Info("The following files were excluded from analysis because they are NOT assembly files: ");
                    unsuitableFiles.DisplayBaseFileInfo();
                }

                NLogger.Info("Getting information about the content of DLLImport in binary files...");
                correctFiles.DisplayImportedMethodsInfoForEachBinary();


            }
            catch(Exception e)
            {
                NLogger.Error(e.ToString());
                return (int)WorkStatusCodes.UndefinedBehavior;
            }

            return (int)WorkStatusCodes.SuccessExit; //replace on BinarySearcherStatusCodes
        }
    }
}
