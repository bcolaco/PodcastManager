using System.Collections.Generic;

namespace PodcastManager.Models
{
    public class Configuration
    {
        public const string ConfigurationDirectory = "PodcastManager";

        public IList<Feed> Feeds { get; }
        public string DownloadDirectory { get; set; }

        public Configuration() : this(new List<Feed>())
        {
        }

        public Configuration(IList<Feed> feeds)
        {
            Feeds = feeds;
        }
    }
}
