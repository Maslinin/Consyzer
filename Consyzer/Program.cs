using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Consyzer.File;
using Consyzer.Logging;
using Consyzer.Metadata;
using Log = Consyzer.Logging.NLogService;
using static Consyzer.Constants;
using static Consyzer.Constants.File;

[assembly: InternalsVisibleTo("Consyzer.Tests")]

namespace Consyzer
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    static class Program
    {
        private static int Main()
        {
            try
            {
                var(analysisDirectory, fileExtensions) = CmdArgsParser.GetAnalysisParams();
                Log.Debug(
                    AnalysisStatusLogger.GetAnalysisParamsLog(analysisDirectory, fileExtensions));

                var files = FileHelper.GetFilesFrom(analysisDirectory, fileExtensions);
                if (!files.Any())
                {
                    Log.Warn("Files for analysis with the specified extensions were not found.");
                    return (int)ProgramStatusCode.SuccessExit;
                }

                Log.Info("The following files with the specified extensions were found:");
                Log.Info(AnalysisStatusLogger.GetBaseFileInfoLog(files));

                AnalysisStatusLogger.GetNotCorrectFilesLog(files, out bool filesAreIncorrect);
                if (filesAreIncorrect)
                    return (int)ProgramStatusCode.SuccessExit;

                var metadataAnalyzers = MetadataFileFilter.GetMetadataAssemblyFiles(files)
                    .Select(f => new MetadataAnalyzer(f));
                Log.Info("The following assembly files containing metadata were found:");
                Log.Info(
                    AnalysisStatusLogger.GetBaseAndHashFileInfoLog(metadataAnalyzers.Select(f => f.FileInfo)));

                Log.Info("Information about imported methods from other assemblies in the analyzed files:");
                Log.Info(
                    AnalysisStatusLogger.GetImportedMethodsInfoForEachFileLog(metadataAnalyzers));

                var fileLocations = metadataAnalyzers
                    .SelectMany(m => m.GetImportedMethodsInfo())
                    .Select(m => FileHelper.AddExtensionToFile(m.DllLocation, DefaultFileExtension))
                    .Distinct();
                if (!fileLocations.Any())
                {
                    Log.Warn("All files are missing imported methods from other assemblies.");
                    return (int)ProgramStatusCode.SuccessExit;
                }

                var searcher = new FileExistenceChecker(analysisDirectory);

                Log.Info("The presence of files in the received locations:");
                Log.Info(
                    AnalysisStatusLogger.GetFilesExistStatusLog(searcher, fileLocations));

                return (int)searcher.GetMaxFileExistanceStatus(fileLocations);
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                return (int)ProgramStatusCode.UnexpectedBehavior;
            }
        }
    }
}
