using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Consyzer.Logging;
using Consyzer.Helpers;
using Consyzer.Metadata;
using Log = Consyzer.Logging.NLogger;
using static Consyzer.Constants;
using System.Collections.Generic;

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
                LogWriter.LogAnalysisParams(analysisDirectory, fileExtensions);

                var files = IOHelper.GetFilesFrom(analysisDirectory, fileExtensions);
                if (!files.Any())
                {
                    Log.Warn("Binary files for analysis with the specified extensions were not found.");
                    return (int)ProgramStatusCode.SuccessExit;
                }

                Log.Info("The following binary files with the specified extensions were found:");
                LogWriter.LogBaseFileInfo(files);

                if (!LogWriter.CheckAndLogCorrectFiles(files))
                    return (int)ProgramStatusCode.SuccessExit;

                var metadataAnalyzers = MetadataFileFilter.GetMetadataAssemblyFiles(files)
                    .Select(f => new MetadataAnalyzer(f));
                Log.Info("The following assembly binaries containing metadata were found:");
                LogWriter.LogBaseAndHashFileInfo(metadataAnalyzers);

                Log.Info("Information about imported methods from other assemblies in the analyzed files:");
                LogWriter.LogImportedMethodsInfoForEachFile(metadataAnalyzers);

                var fileLocations = GetImportedMethodsLocations(metadataAnalyzers);
                if (!fileLocations.Any())
                {
                    Log.Warn("All files are missing imported methods from other assemblies.");
                    return (int)ProgramStatusCode.SuccessExit;
                }

                var searcher = new FileSearcher(analysisDirectory);

                Log.Info("The presence of binary files in the received locations:");
                LogWriter.LogFilesExistStatus(searcher, fileLocations);

                return (int)searcher.GetMaxFileExistanceStatusCode(fileLocations);
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                return (int)ProgramStatusCode.UnexpectedBehavior;
            }
        }

        public static IEnumerable<string> GetImportedMethodsLocations(IEnumerable<IMetadataAnalyzer> metadataAnalyzers, string defaultFileExtension = ".dll")
        {
            var importedMethods = new List<string>();

            foreach (var method in metadataAnalyzers)
            {
                var methods = method.GetImportedMethodsInfo()
                    .Select(m => IOHelper.AddExtensionToFile(m.DllLocation, defaultFileExtension));
                importedMethods.AddRange(methods);
            }

            return importedMethods.Distinct();
        }
    }
}
