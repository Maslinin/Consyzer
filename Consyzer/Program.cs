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
                LogWriter.LoggingPathToBinariesForAnalysis(analysisFolder);
                var filesExtensions = CmdArgsReceiver.GetFilesExtensionsFromCommandLineArgs();
                LogWriter.LoggingFilesExtensionsForAnalysis(filesExtensions);

                var binaryFiles = IOHelper.GetBinaryFilesInfoFrom(analysisFolder, filesExtensions);
                if (!LogWriter.CheckAndLoggingBinaryFilesExist(binaryFiles))
                    return (int)ProgramStatusCode.SuccessExit;

                Log.Info("The following binary files with the specified extensions were found:");
                LogWriter.LoggingBaseFileInfo(binaryFiles);
                if (!LogWriter.CheckAndLoggingFilesCorrect(binaryFiles))
                    return (int)ProgramStatusCode.SuccessExit;

                var metadataAnalyzers = binaryFiles.ToMetadataAnalyzersFromMetadataAssemblyFiles();
                Log.Info("The following assembly binaries containing metadata were found:");
                LogWriter.LoggingBaseAndHashFileInfo(metadataAnalyzers);

                Log.Info("Information about imported methods from other assemblies in the analyzed files:");
                LogWriter.LoggingImportedMethodsInfoForEachBinary(metadataAnalyzers);

                var binaryLocations = AnalyzerHelper.GetImportedMethodsLocations(metadataAnalyzers);
                if (!LogWriter.CheckAndLoggingAnyBinariesExist(binaryLocations))
                    return (int)ProgramStatusCode.SuccessExit;

                Log.Info("The presence of binary files in the received locations:");
                LogWriter.LoggingBinariesExistStatus(binaryLocations, analysisFolder);
                LogWriter.LoggingExistAndNonExistBinariesCount(binaryLocations, analysisFolder);

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
