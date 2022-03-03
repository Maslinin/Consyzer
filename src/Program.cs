using System;
using System.IO;
using System.Linq;
using Consyzer.Logger;
using Consyzer.Analyzer;
using Consyzer.Configuration;

namespace Consyzer
{
    static class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                if(args.Length < 1)
                {
                    throw new ArgumentException("The command line parameter responsible for the directory location for the analysis was not passed.");
                }

                string pathToAnalyze = args[0];
                if(!Directory.Exists(pathToAnalyze))
                {
                    throw new DirectoryNotFoundException("The directory specified for the analysis does not exist");
                }

                if (!File.Exists(LinkerConfiguration.ConfigPath))
                {
                    NLogger.Error($"Configuration file not found at path {LinkerConfiguration.ConfigPath}.");
                    LinkerConfiguration.CreateConfigFile(LinkerConfiguration.ConfigPath);
                    NLogger.Error("The configuration file was recreated. Please complete it before the next utility launch.");

                    throw new FileNotFoundException("Configuration file not found.");
                }
                NLogger.Info("Loading Configuration...");
                var config = new LinkerConfiguration();
                NLogger.Info("Configuration file was successfully loaded.");

                NLogger.Info("Getting binaries at the specified path with the specified extensions...");
                var binaryFiles = StaticAnalyzer.GetBinaryFilesName(pathToAnalyze, config.BinaryFilesExtensions).ToList();

                if(binaryFiles.FirstOrDefault() is null)
                {
                    NLogger.Warn("No binary files found for analysis.");

                    return -1;
                }
                else
                {
                    foreach (var file in binaryFiles)
                    {
                        NLogger.Info($"Name: {file.Name}, Full Name: {file.FullName}, Creation Time: {file.CreationTime}");
                    }
                }
            }
            catch(Exception e)
            {
                NLogger.Error(e.ToString());
            }

            return 0;
        }
    }
}
