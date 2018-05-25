using NLog;
using System;
using System.IO;
using System.Net;
using System.Xml;

namespace PodcastManager
{
    class Program
    {
        const string ConfigurationFileName = "configuration.json";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static WebClient WebClient = new WebClient();

        static void Main(string[] args)
        {
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigurationFileName));

            using (var webClient = new WebClient())
            {
                foreach (var feed in config.Feeds)
                {
                    Uri uri = new Uri(feed.Url);
                    string directory = Path.Combine(config.DataFolder, uri.Host);
                    if (!Directory.Exists(directory))
                    {
                        logger.Trace($"Creating directory {directory}");
                        Directory.CreateDirectory(directory);
                    }

                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(feed.DownloadFeed());

                        feed.Title = xmlDoc.SelectSingleNode("/rss/channel/title").InnerText;
                        logger.Info($"Updating \"{feed.Title}\"");
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
                                        logger.Warn($"Deleting file {itemPath}");
                                        File.Delete(itemPath);
                                    }

                                    if (itemCount < (feed.MaxItems ?? config.MaxItemsPerFeed))
                                    {
                                        logger.Info($"Downloading episode \"{item["title"].InnerText}\"");
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
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                }
            }

            try
            {
                File.WriteAllText(
                    ConfigurationFileName,
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        config,
                        Newtonsoft.Json.Formatting.Indented,
                        new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            }
            catch(Exception ex)
            {
                logger.Error(ex);
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
