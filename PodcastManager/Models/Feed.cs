using System.Collections.Generic;

namespace PodcastManager.Models
{
    public class Feed
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int? MaxItems { get; set; }
        public IList<string> Downloaded { get; set; } = new List<string>();
        public IList<FeedItem> Items { get; set;  }

        public Feed(string url)
        {
            Items = new List<FeedItem>();
            Title = url;
            Url = url;
        }

        public override string ToString()
        {
            return Title ?? Url ?? base.ToString();
        }
    }
}
