using System.Linq;
using System.Collections.Generic;

namespace PodcastManager
{
    public class Configuration
    {
        private IFileSystem fileSystem;
        public string DataDirectory { get; set; }
        public int DefaultMaximumItems { get; set; }
        public IList<Feed> Feeds { get; set; }

        public Configuration(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public override bool Equals(object obj)
        {
            //Adapted from http://www.loganfranken.com/blog/687/overriding-equals-in-c-part-1/
            var objConfiguration = obj as Configuration;
            if(objConfiguration != null)
            {
                return
                    string.Equals(DataDirectory, objConfiguration.DataDirectory) &&
                    string.Equals(DefaultMaximumItems, objConfiguration.DefaultMaximumItems) &&
                    Enumerable.SequenceEqual(Feeds, objConfiguration.Feeds);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            // Adapted from http://www.loganfranken.com/blog/692/overriding-equals-in-c-part-2/
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HashingBase = (int) 2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, DataDirectory) ? DataDirectory.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, DefaultMaximumItems) ? DefaultMaximumItems.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Feeds) ? Feeds.GetHashCode() : 0);
                return hash;
            }
        }

        public void Save(string fileName)
        {
            fileSystem.FileWriteAllText(fileName,
                Newtonsoft.Json.JsonConvert.SerializeObject(
                    this,
                    Newtonsoft.Json.Formatting.Indented,
                    new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore })
            );
        }

        public static Configuration LoadFromFile(IFileSystem fileSystem, string fileName)
        {
            var configurationData = fileSystem.FileReadAllText(fileName);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(configurationData);
        }
    }
}