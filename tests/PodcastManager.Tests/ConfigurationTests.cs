using Moq;
using System.Collections.Generic;
using Xunit;

namespace PodcastManager.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void ConfigurationSaveAndLoadKeepsAllData()
        {
            var fileSystem = new DummyFileSystem();
            var webClient = new Mock<IWebClient>();
            var conf = new Configuration(fileSystem)
            {
                DataDirectory = "DummyDataDirectory",
                DefaultMaximumItems = 30,
                Feeds = new List<Feed>()
                {
                    new Feed(webClient.Object)
                    {
                        Title = "Dummy Title",
                        Url = "https://test.com/dummy",
                        DownloadedItems = new List<string>() { "item 1", "item 2", "item 3" }
                    },
                    new Feed(webClient.Object)
                    {
                        Title = "Dummy Title 2",
                        Url = "https://test2.com/dummy"
                    }
                }
            };

            var fileName = "config.json";
            conf.Save(fileName);
            var newConf = Configuration.LoadFromFile(fileSystem, fileName);
            Assert.True(conf.Equals(newConf));
        }
    }
}