using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PodcastManager
{
    class Program
    {
        /*private const string DataFolder = @"D:\Podcasts";
        private const int MaxItemsPerFeed = 1;

        private static string[] FeedUrls = new string[] {
            @"http://feeds.tsf.pt/semmoderacao",
            @"http://feeds.tsf.pt/Tsf-GovernoSombra-Podcast",
            @"http://feeds.tsf.pt/Tsf-HistoriasJustica",
            @"http://feeds.wnyc.org/radiolab"
        };*/

        const string ConfigurationFileName = "configuration.json";

        static void Main(string[] args)
        {
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigurationFileName));

            WebClient webClient = new WebClient();

            foreach (var feed in config.Feeds)
            {
                Uri uri = new Uri(feed.Url);
                string directory = Path.Combine(config.DataFolder, uri.Host);
                if (!Directory.Exists(directory))
                {
                    var dirInfo = Directory.CreateDirectory(directory);
                }

                try
                {
                    var rssData = webClient.DownloadString(feed.Url).Trim(new char[] { '\0' });
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(rssData);

                    Console.WriteLine(xmlDoc.SelectSingleNode("/rss/channel/title").InnerText);
                    int itemCount = 0;
                    foreach (XmlNode item in xmlDoc.SelectNodes("/rss/channel/item"))
                    {
                        var urlItem = item["media:content"] ?? item["enclosure"];
                        if (urlItem != null)
                        {
                            var link = urlItem.Attributes["url"].Value;
                            var itemUri = new Uri(link);
                            var fileName = GetFileName(itemUri);

                            if (!feed.Downloaded.Contains(fileName))
                            {
                                var itemPath = Path.Combine(directory, fileName);


                                if (File.Exists(itemPath))
                                {
                                    Console.WriteLine("Deleting file from ");
                                    File.Delete(itemPath);
                                }

                                if (itemCount < (feed.MaxItems ?? config.MaxItemsPerFeed))
                                {
                                    Console.WriteLine(item["title"].InnerText);
                                    webClient.DownloadFile(link, itemPath);
                                    feed.Downloaded.Add(fileName);
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
                catch(Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }

            File.WriteAllText(
                ConfigurationFileName,
                Newtonsoft.Json.JsonConvert.SerializeObject(
                    config,
                    Newtonsoft.Json.Formatting.Indented,
                    new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
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
