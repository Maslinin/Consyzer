using System;
using System.IO;
using System.Linq;

using Consyzer.Logger;
using Consyzer.Config;
using Consyzer.AnalyzerEngine.Analyzer;
using Consyzer.AnalyzerEngine.Support;

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

                if (!File.Exists(ConsyzerConfig.DefaultConfigPath))
                {
                    NLogger.Error($"Configuration file not found at path {ConsyzerConfig.DefaultConfigPath}.");
                    ConsyzerConfig.CreateConfigFile(ConsyzerConfig.DefaultConfigPath);
                    NLogger.Error($"The configuration file was recreated on path {ConsyzerConfig.DefaultConfigPath}. Please complete it before the next utility launch.");

                    throw new FileNotFoundException("Configuration file not found.");
                }
                NLogger.Info("Loading Configuration...");
                var config = ConsyzerConfig.LoadConfigFromFile(ConsyzerConfig.DefaultConfigPath);
                NLogger.Info("Configuration file was successfully loaded.");

                NLogger.Info($"Path for analyze: {pathForAnalyze}");
                NLogger.Info("Getting binaries at the specified path with the specified extensions...");
                var binaryFiles = IOSupport.GetBinaryFilesInfo(pathForAnalyze, config.BinaryFilesExtensions).ToList();

                if(binaryFiles.FirstOrDefault() is null)
                {
                    NLogger.Warn("No binary files found for analysis.");

                    return (int)PostCodes.UndefinedBehavior;
                }
                foreach (var file in binaryFiles)
                {
                    NLogger.Info($"Name: {file.Name}, Creation Time: {file.CreationTime}");
                }

                NLogger.Info("Getting binary files containing metadata...");
                var managedFiles = AnalyzerSupport.GetManagedFilesFromList(binaryFiles);
                var unmanagedFiles = binaryFiles.Count() > managedFiles.Count() ? binaryFiles.Except(managedFiles) : managedFiles.Except(binaryFiles);
                if (managedFiles.Count() == 0)
                {
                    NLogger.Warn("No analysis files containing metadata were found. All files found contain unmanaged code.");
                    return (int)PostCodes.UndefinedBehavior;
                }

                foreach(var file in managedFiles)
                {
                    NLogger.Info($"Name: {file.Name}, Creation Time: {file.CreationTime}");
                }
                NLogger.Info("The following files that are not managed were excluded from the analysis:");
                foreach(var file in unmanagedFiles)
                {
                    NLogger.Info($"Name: {file.Name}, Creation Time: {file.CreationTime}");
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
