using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;

namespace PodcastManager
{
    public class Feed
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int? MaximumItems { get; set; }
        public IList<string> DownloadedItems { get; set; } = new List<string>();
        private IWebClient webClient;

        public Feed(IWebClient webClient)
        {
            this.webClient = webClient;
        }

        public override bool Equals(object obj)
        {
            var objFeed = obj as Feed;
            if(objFeed != null)
            {
                return
                    string.Equals(Title, objFeed.Title) &&
                    string.Equals(Url, objFeed.Url) &&
                    string.Equals(MaximumItems, objFeed.MaximumItems) &&
                    Enumerable.SequenceEqual(DownloadedItems, objFeed.DownloadedItems);
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
                hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Title) ? Title.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Url) ? Url.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, MaximumItems) ? MaximumItems.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, DownloadedItems) ? DownloadedItems.GetHashCode() : 0);
                return hash;
            }
        }

        public void Update(Configuration configuration)
        {
            var uri = new Uri(Url);
            var directory = Path.Combine(configuration.DataDirectory, uri.Host);
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(webClient.DownloadString(Url));
            
            Title = xmlDoc.SelectSingleNode("/rss/channel/title").InnerText;
            int itemCount = 0;
            foreach (XmlNode item in xmlDoc.SelectNodes("/rss/channel/item"))
            {
                var urlItem = item["media:content"] ?? item["enclosure"];
                if (urlItem != null)
                {
                    var link = urlItem.Attributes["url"].Value;
                    var itemUri = new Uri(link);
                    var fileName = GetFileName(itemUri);
                    if (!DownloadedItems.Contains(fileName))
                    {
                        var itemPath = Path.Combine(directory, fileName);
                        if (File.Exists(itemPath))
                        {
                            File.Delete(itemPath);
                        }
                        if (itemCount < (MaximumItems ?? configuration.DefaultMaximumItems))
                        {
                            webClient.DownloadFile(link, itemPath);
                            DownloadedItems.Add(fileName);
                            itemCount += 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static string GetFileName(Uri uri)
        {
            var index = uri.AbsolutePath.LastIndexOf('/');
            if (index >= 0)
            {
                return uri.AbsolutePath.Substring(index + 1);
            }
            else
            {
                return uri.AbsolutePath;
            }
        }
    }
}
