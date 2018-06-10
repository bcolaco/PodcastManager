namespace PodcastManager.Models
{
    public class FeedItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DownloadPercentage { get; set; }
        public bool Downloading { get; set; }

        public override string ToString()
        {
            return Title ?? Url ?? base.ToString();
        }
    }
}