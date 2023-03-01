using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Consyzer
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class CmdArgsParser
    {
        private const char _argsDelimiter = ',';
        private static readonly string[] _args = Environment.GetCommandLineArgs();

        public static (string analysisDirectory, IEnumerable<string> fileExtensions) GetAnalysisParams()
        {
            var analysisDirectory = GetArgument(1, "No command line parameter was passed that contains the location of the catalog to analyze.");
            var fileExtensions = GetArgument(2, "No command line parameter containing binary file extensions for analysis was passed.")
                .Split(_argsDelimiter)
                .Select(e => e.Trim());

            ValidateDirectory(analysisDirectory);
            ValidateExtensions(fileExtensions);

            return (analysisDirectory, fileExtensions);
        }

        private static string GetArgument(int index, string errorMessage)
        {
            var argument = _args.ElementAtOrDefault(index);
            return argument ?? throw new ArgumentException(errorMessage);
        }

        private static void ValidateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("The directory path for the analysis does not exist or is incorrect.");
            }
        }

        private static void ValidateExtensions(IEnumerable<string> extensions)
        {
            if (extensions.Any(e => !Path.HasExtension(e)))
            {
                throw new ArgumentException("One or more names are not extensions.");
            }
        }
    }
}
