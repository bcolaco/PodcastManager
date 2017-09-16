using System;
using Xunit;
using Moq;

namespace PodcastManager.Tests
{
    public class FeedTests
    {
        [Fact]
        public void DummyTest1()
        {
            var webClient = new Mock<IWebClient>();
            var url = "http://test.com";
            var downloadedContent = "test content";
            webClient.Setup(o => o.DownloadString(url)).Returns(downloadedContent);
            var feed = new Feed(webClient.Object)
            {
                Url = url
            };
            Assert.True(url == feed.Url);
        }
    }
}
