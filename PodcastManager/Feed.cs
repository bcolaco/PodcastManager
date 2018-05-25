using NLog;
using System.Collections.Generic;

namespace PodcastManager
{
    public class Feed
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public string Title { get; set; }
        public string Url { get; set; }
        public int? MaxItems { get; set; }
        public IList<string> Downloaded { get; set; } = new List<string>();

        public string DownloadFeed()
        {
            logger.Debug($"Downloading {Url}");
            return Program.WebClient.DownloadString(Url).Trim(new char[] { '\0' });
        }
    }
}
