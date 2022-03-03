using System;
using System.IO;
using Newtonsoft.Json;

namespace Consyzer.Configuration
{
    public sealed class LinkerConfiguration
    {
        public static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConsyzerConfig.json");

        /// <summary>
        /// Gets or sets a list of binary file extensions
        /// </summary>
        public string[] BinaryFilesExtensions { get; set; }

        /// <summary>
        /// Reads the configuration file at the configPath path and deserializes it to the Configuration object
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns> Deserialized LinkerConfiguration Instance </returns>
        public static LinkerConfiguration LoadConfigFromFile(string configPath)
        {
            string configJson;

            try
            {
                configJson = File.ReadAllText(configPath);
            }
            catch (Exception e)
            {
                throw new IOException("Can't read a configuration file", e);
            }

            try
            {
                return JsonConvert.DeserializeObject<LinkerConfiguration>(configJson);
            }
            catch (Exception e)
            {
                throw new JsonSerializationException("Can't deserialize a configuration file", e);
            }
        }

        /// <summary>
        /// Creates a new configuration file at the specified path
        /// </summary>
        /// <param name="configPath"></param>
        public static void CreateConfigFile(string configPath)
        {
            string configContent;

            try
            {
                configContent = JsonConvert.SerializeObject(new LinkerConfiguration());
            }
            catch (Exception e)
            {
                throw new JsonSerializationException("Can't serialize a configuration file", e);
            }

            try
            {
                File.WriteAllText(configPath, configContent);
            }
            catch (Exception e)
            {
                throw new IOException("Can't read a configuration file", e);
            }
        }
    }
}
