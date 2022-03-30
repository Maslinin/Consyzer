using System;
using System.IO;
using System.Linq;

namespace Consyzer.Helpers
{
    public static class OtherHelper
    {
        public static string GetDirectoryWithBinariesFromCommandLineArgs()
        {
            string pathToAnalyze = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
            if(pathToAnalyze is null) // 0 is path to current executable, 1 is path to binary
            {
                throw new ArgumentException("The command line parameter responsible for the directory location for the analysis was not passed.");
            }

            if (!Directory.Exists(pathToAnalyze))
            {
                throw new DirectoryNotFoundException("The directory path for the analysis does not exist or is incorrect.");
            }

            return pathToAnalyze;
        }

    }
}
