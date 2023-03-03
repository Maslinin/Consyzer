using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Consyzer.File;
using Consyzer.Logging;
using Consyzer.Metadata;
using Log = Consyzer.Logging.LogService;
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
                AnalysisStatusLogger.LogAnalysisParams(analysisDirectory, fileExtensions);

                var files = FileHelper.GetFilesFrom(analysisDirectory, fileExtensions);
                if (!files.Any())
                {
                    Log.Warn("Files for analysis with the specified extensions were not found.");
                    return (int)ProgramStatusCode.SuccessExit;
                }

                Log.Info("The following files with the specified extensions were found:");
                AnalysisStatusLogger.LogBaseFileInfo(files);

                if (!AnalysisStatusLogger.CheckAndLogCorrectFiles(files))
                    return (int)ProgramStatusCode.SuccessExit;

                var metadataAnalyzers = MetadataFileFilter.GetMetadataAssemblyFiles(files)
                    .Select(f => new MetadataAnalyzer(f));
                Log.Info("The following assembly files containing metadata were found:");
                AnalysisStatusLogger.LogBaseAndHashFileInfo(metadataAnalyzers);

                Log.Info("Information about imported methods from other assemblies in the analyzed files:");
                AnalysisStatusLogger.LogImportedMethodsInfoForEachFile(metadataAnalyzers);

                var fileLocations = GetImportedMethodsLocations(metadataAnalyzers);
                if (!fileLocations.Any())
                {
                    Log.Warn("All files are missing imported methods from other assemblies.");
                    return (int)ProgramStatusCode.SuccessExit;
                }

                var searcher = new FileExistenceChecker(analysisDirectory);

                Log.Info("The presence of files in the received locations:");
                AnalysisStatusLogger.LogFilesExistStatus(searcher, fileLocations);

                return (int)searcher.GetMaxFileExistanceStatus(fileLocations);
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                return (int)ProgramStatusCode.UnexpectedBehavior;
            }
        }

        public static IEnumerable<string> GetImportedMethodsLocations(IEnumerable<MetadataAnalyzer> metadataAnalyzers, string defaultFileExtension = DefaultFileExtension)
        {
            var importedMethods = new List<string>();

            foreach (var method in metadataAnalyzers)
            {
                var methods = method.GetImportedMethodsInfo()
                    .Select(m => FileHelper.AddExtensionToFile(m.DllLocation, defaultFileExtension));
                importedMethods.AddRange(methods);
            }

            return importedMethods.Distinct();
        }
    }
}
