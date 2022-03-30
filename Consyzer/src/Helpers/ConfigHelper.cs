using System.IO;
using Consyzer.Config;

namespace Consyzer.Helpers
{
    public static class ConfigHelper
    {
        public static ConsyzerConfig GetConfig(string configPath)
        {
            if (!File.Exists(configPath))
            {
                ConsyzerConfig.CreateConfigFile(configPath);
                throw new FileNotFoundException($@"Configuration file not found at path {configPath}.
                    The configuration file was recreated on path {configPath}. Please complete it before the next utility launch.");
            }

            return ConsyzerConfig.LoadFromFile(configPath);
        }

        public static string GetBinaryFilesExtensions(this ConsyzerConfig config)
        {
            return string.Join(", ", config.BinaryFilesExtensions);
        }
    }
}
