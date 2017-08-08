using System.Collections.Generic;

namespace PodcastManager
{
    public class Configuration
    {
        public string DataFolder { get; set; }
        public int MaxItemsPerFeed { get; set; }
        public IList<Feed> Feeds { get; set; }
    }
}
