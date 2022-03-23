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
                if(args.FirstOrDefault() is null)
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
                var config = ConsyzerConfig.LoadFromFile(ConsyzerConfig.DefaultConfigPath);
                NLogger.Info("Configuration file was successfully loaded.");

                NLogger.Info($"Path for analyze: \"{pathForAnalyze}\".");
                NLogger.Info($"Specified binary file extensions for analysis: {string.Join(", ", config.BinaryFilesExtensions)}.");
                NLogger.Info("Getting binaries at the specified path with the specified extensions...");
                var binaryFiles = IOSupport.GetBinaryFilesInfo(pathForAnalyze, config.BinaryFilesExtensions).ToList();
                if(binaryFiles.FirstOrDefault() is null)
                {
                    NLogger.Warn("No binary files found for analysis.");

                    return (int)PostAnalyzeCodes.UndefinedBehavior;
                }

                NLogger.Info("The following binary files were found: ");
                foreach (var item in binaryFiles.Select((File, i) => (File, i)))
                {
                    NLogger.Info($"\t[{item.i}]Name: {item.File.BaseFileInfo.Name}, Creation Time: {item.File.BaseFileInfo.CreationTime}.");
                }

                var metadataFiles = binaryFiles.GetFilesContainsMetadata();
                var correctFiles = metadataFiles.GetAssemblyFiles().ToList();
                if (correctFiles.Count() == 0)
                {
                    NLogger.Warn("No analysis files containing metadata were found. All files do not contain metadata.");

                    return (int)PostAnalyzeCodes.UndefinedBehavior;
                }
                else
                {
                    NLogger.Info("Binary assembly files for analyze containing metadata: ");
                    foreach (var item in correctFiles.Select((File, i) => (File, i)))
                    {
                        NLogger.Info($"\t[{item.i}]Name: {item.File.BaseFileInfo.Name}, Creation Time: {item.File.BaseFileInfo.CreationTime}, " +
                            $"MD5HashSum: {item.File.HashInfo.MD5Sum}, SHA256HashSum: {item.File.HashInfo.SHA256Sum}.");
                    }
                }
                var unsuitableFiles = binaryFiles.GetFilesNotContainsMetadata();
                if(unsuitableFiles.Count() != 0)
                {
                    NLogger.Info("The following files were excluded from analysis because they DO NOT contain metadata: ");
                    foreach (var item in unsuitableFiles.Select((File, i) => (File, i)))
                    {
                        NLogger.Info($"\t[{item.i}]Name: {item.File.BaseFileInfo.Name}, Creation Time: {item.File.BaseFileInfo.CreationTime}.");
                    }
                }
                unsuitableFiles = metadataFiles.GetNotAssemblyFiles();
                if (unsuitableFiles.Count() != 0)
                {
                    NLogger.Info("The following files were excluded from analysis because they are NOT assembly files: ");
                    foreach (var item in unsuitableFiles.Select((File, i) => (File, i)))
                    {
                        NLogger.Info($"\t[{item.i}]Name: {item.File.BaseFileInfo.Name}, Creation Time: {item.File.BaseFileInfo.CreationTime}.");
                    }
                }

                NLogger.Info("Getting information about the content of DLLImport in binary files...");
                foreach (var item in correctFiles.Select((File, i) => (File, i)))
                {
                    var importedMethods = DynamicAnalyzer.GetImportedMethodsInfo(item.File.BaseFileInfo.FullName).ToList();

                    NLogger.Info($"\t[{item.i}]From File {item.File.BaseFileInfo.FullName}: ");
                    foreach (var import in importedMethods.Select((Signature, i) => (Signature, i)))
                    {
                        NLogger.Info($"\t[File: {item.i}, Method: {import.i}]Method Location: {import.Signature.SignatureInfo.GetMethodLocation()}");
                        NLogger.Info($"\t[File: {item.i}, Method: {import.i}]Method Signature: {import.Signature.SignatureInfo.GetBaseMethodSignature()}");
                        NLogger.Info($"\t[File: {item.i}, Method: {import.i}]DLL Location: {import.Signature.DLLPosition}");
                        NLogger.Info($"\t[File: {item.i}, Method: {import.i}]DLL Import Arguments: {import.Signature.DLLImportArguments}");
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
