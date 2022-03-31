using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Consyzer.Helpers
{
    public static class OtherHelper
    {
        public static string GetDirectoryWithBinariesFromCommandLineArgs()
        {
            string pathToAnalyze = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
            if(pathToAnalyze is null) // 0 is path to current executable, 1 is path to binary
            {
                throw new ArgumentException("No command line parameter was passed that contains the location of the catalog to analyze.");
            }

            if (!Directory.Exists(pathToAnalyze))
            {
                throw new DirectoryNotFoundException("The directory path for the analysis does not exist or is incorrect.");
            }

            return pathToAnalyze;
        }

        public static IEnumerable<string> GetBinaryFilesExtensions()
        {
            string pathToAnalyze = Environment.GetCommandLineArgs().ElementAtOrDefault(2);
            if (pathToAnalyze is null) // 0 is path to current executable, 1 is path to binary, 2 are files extensions
            {
                throw new ArgumentException("No command line parameter containing binary file extensions for analysis was passed.");
            }

            return pathToAnalyze.Split(',');
        }
    }
}
