using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Consyzer.FileInteraction;
using Consyzer.Logging;
using Consyzer.Extractors;
using Log = Consyzer.Logging.NLogService;
using static Consyzer.Constants;

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

                Log.Info(
                    AnalysisStatusLogger.GetNotMetadataFilesLog(files, out bool filesAreNotMetadata));
                if (filesAreNotMetadata)
                    return (int)ProgramStatusCode.SuccessExit;
                Log.Info(
                    AnalysisStatusLogger.GetNotMetadataAssemblyFilesLog(files, out bool filesAreNotMetadataAssembly));
                if (filesAreNotMetadataAssembly)
                    return (int)ProgramStatusCode.SuccessExit;

                var importedMethodsAnalyzers = MetadataFileFilter.GetMetadataAssemblyFiles(files)
                    .AsParallel()
                    .Select(f => new ImportedMethodExtractor(f));
                Log.Info("The following assembly files containing metadata were found:");
                Log.Info(
                    AnalysisStatusLogger.GetBaseAndHashFileInfoLog(importedMethodsAnalyzers.Select(f => f.FileInfo)));

                Log.Info("Information about imported methods from other assemblies in the analyzed files:");
                Log.Info(
                    AnalysisStatusLogger.GetImportedMethodsInfoForEachFileLog(importedMethodsAnalyzers));

                var fileLocations = importedMethodsAnalyzers
                    .SelectMany(m => m.GetImportedMethodsInfo())
                    .Select(m => FileHelper.AddExtensionToFile(m.DllLocation, FileHelper.DefaultExtension))
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
