using System.Collections.Generic;

namespace PodcastManager
{
    public class Feed
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int? MaxItems { get; set; }
        public IList<string> Downloaded { get; set; } = new List<string>();
    }
}
