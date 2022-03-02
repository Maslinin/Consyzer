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
        private static void Main()
        {
            try
            {
                if(!File.Exists(LinkerConfiguration.ConfigPath))
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
                var binaryFiles = StaticAnalyzer.GetBinaryFilesName(config.PathToAnalyze, config.BinaryFilesExtensions).ToList();

                if(binaryFiles.Count() == 0)
                {
                    NLogger.Warn("No binary files found for analysis.");

                    return;
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
        }
    }
}
