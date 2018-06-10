using System;
using System.IO;
using Newtonsoft.Json;

namespace PodcastManager.Models
{
    class ConfigurationPersister : IConfigurationPersister
    {
        private const string ConfigurationFileName = "configuration.json";
        private readonly string filePath;

        public ConfigurationPersister()
        {
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Configuration.ConfigurationDirectory, ConfigurationFileName);
        }

        public Configuration Load()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var configuration = JsonConvert.DeserializeObject<Configuration>(json);
                return configuration;
            }
            else
            {
                return new Configuration();
            }
        }

        public void Save(Configuration configuration)
        {
            var json = JsonConvert.SerializeObject(configuration, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(filePath, json);
        }
    }
}
