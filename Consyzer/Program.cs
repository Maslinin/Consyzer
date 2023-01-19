using System;
using Consyzer.Logging;
using Consyzer.Helpers;
using Consyzer.Contracts;
using Log = Consyzer.Logging.NLogger;

namespace Consyzer
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    static class Program
    {
        private static int Main()
        {
            try
            {
                string analysisFolder = CmdArgsReceiver.GetDirectoryWithFilesForAnalysisFromCommandLineArgs();
                LoggerHelper.LoggingPathToBinariesForAnalysis(analysisFolder);
                var filesExtensions = CmdArgsReceiver.GetFilesExtensionsFromCommandLineArgs();
                LoggerHelper.LoggingFilesExtensionsForAnalysis(filesExtensions);

                var binaryFiles = IOHelper.GetBinaryFilesInfoFrom(analysisFolder, filesExtensions);
                if (!LoggerCheckerHelper.CheckAndLoggingBinaryFilesExist(binaryFiles))
                    return (int)ProgramStatusCode.SuccessExit;

                Log.Info("The following binary files with the specified extensions were found:");
                LoggerHelper.LoggingBaseFileInfo(binaryFiles);
                if (!LoggerCheckerHelper.CheckAndLoggingFilesCorrect(binaryFiles))
                    return (int)ProgramStatusCode.SuccessExit;

                var metadataAnalyzers = binaryFiles.ToImportedMethodsAnalyzersFromMetadataAssemblyFiles();
                Log.Info("The following assembly binaries containing metadata were found:");
                LoggerHelper.LoggingBaseAndHashFileInfo(metadataAnalyzers);

                Log.Info("Information about imported methods from other assemblies in the analyzed files:");
                LoggerHelper.LoggingImportedMethodsInfoForEachBinary(metadataAnalyzers);

                var binaryLocations = AnalyzerHelper.GetImportedMethodsLocations(metadataAnalyzers);
                if (!LoggerCheckerHelper.CheckAndLoggingAnyBinariesExist(binaryLocations))
                    return (int)ProgramStatusCode.SuccessExit;

                Log.Info("The presence of binary files in the received locations:");
                LoggerHelper.LoggingBinariesExistStatus(binaryLocations, analysisFolder);
                LoggerHelper.LoggingExistAndNonExistBinariesCount(binaryLocations, analysisFolder);

                return (int)AnalyzerHelper.GetTopBinarySearcherStatusAmongBinaries(binaryLocations, analysisFolder);
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                return (int)ProgramStatusCode.UnexpectedBehavior;
            }
        }
    }
}
