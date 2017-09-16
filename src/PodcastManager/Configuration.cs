using System.Collections.Generic;
using System.IO;

namespace PodcastManager
{
    public class Configuration
    {
        public string DataDirectory { get; set; }
        public int DefaultMaximumItems { get; set; }
        public IList<Feed> Feeds { get; set; }

        public void Save(string fileName)
        {
            File.WriteAllText(fileName,
                Newtonsoft.Json.JsonConvert.SerializeObject(
                    this,
                    Newtonsoft.Json.Formatting.Indented,
                    new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore })
            );
        }

        public static Configuration LoadFromFile(string fileName)
        {
            var configurationData = File.ReadAllText(fileName);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(configurationData);
        }
    }
}