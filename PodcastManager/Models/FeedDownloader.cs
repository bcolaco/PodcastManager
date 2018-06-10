using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Xml;

namespace PodcastManager.Models
{
    class FeedDownloader : IFeedDownloader
    {
        private const int MaxItemsToGet = 20;

        public Feed Download(string url)
        {
            using (var webClient = new WebClient())
            {
                var xml = webClient.DownloadString(url).Trim(new char[] { '\0' });
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                var channel = xmlDoc.SelectSingleNode("/rss/channel");
                var feed = new Feed(url)
                {
                    Title = channel["title"]?.InnerText,
                    ImageUrl = channel["itunes:image"]?.Attributes["href"]?.Value
                };

                var itemIndex = 0;
                foreach (XmlNode item in xmlDoc.SelectNodes("/rss/channel/item"))
                {
                    var feedItem = LoadFeedItem(item);
                    feed.Items.Add(feedItem);

                    if (++itemIndex >= MaxItemsToGet)
                    {
                        break;
                    }
                }

                return feed;
            }
        }

        public static FeedItem LoadFeedItem(XmlNode item)
        {
            var feedItem = new FeedItem
            {
                Url = (item["media:content"] ?? item["enclosure"])?.Attributes["url"]?.Value,
                Title = item["title"]?.InnerText,
                Description = item["description"]?.InnerText
            };

            return feedItem;
        }

        public void Download(string url, string directory, AsyncCompletedEventHandler completedEventHandler, DownloadProgressChangedEventHandler downloadProgressChangedEventHandler)
        {
            var uri = new Uri(url);
            using (var webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += completedEventHandler;
                webClient.DownloadProgressChanged += downloadProgressChangedEventHandler;
                webClient.DownloadFileAsync(uri, Path.Combine(directory, uri.Segments[uri.Segments.Length - 1]));
            }
        }
    }
}
