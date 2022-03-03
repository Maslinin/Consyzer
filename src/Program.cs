using System;
using System.IO;
using System.Linq;

using Consyzer.Logger;
using Consyzer.Analyzer;
using Consyzer.Config;

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

                string pathForAnalyze = args[0];
                if(!Directory.Exists(pathForAnalyze))
                {
                    throw new DirectoryNotFoundException("The directory path for the analysis does not exist or is incorrect.");
                }

                if (!File.Exists(ConsyzerConfiguration.DefaultConfigPath))
                {
                    NLogger.Error($"Configuration file not found at path {ConsyzerConfiguration.DefaultConfigPath}.");
                    ConsyzerConfiguration.CreateConfigFile(ConsyzerConfiguration.DefaultConfigPath);
                    NLogger.Error($"The configuration file was recreated on path {ConsyzerConfiguration.DefaultConfigPath}. Please complete it before the next utility launch.");

                    throw new FileNotFoundException("Configuration file not found.");
                }
                NLogger.Info("Loading Configuration...");
                var config = ConsyzerConfiguration.LoadConfigFromFile(ConsyzerConfiguration.DefaultConfigPath);
                NLogger.Info("Configuration file was successfully loaded.");

                NLogger.Info($"Path for analyze: {pathForAnalyze}");
                NLogger.Info("Getting binaries at the specified path with the specified extensions...");
                var binaryFiles = DynamicAnalyzer.GetBinaryFilesName(pathForAnalyze, config.BinaryFilesExtensions).ToList();

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
