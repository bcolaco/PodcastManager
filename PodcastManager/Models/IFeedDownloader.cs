using System.ComponentModel;
using System.Net;

namespace PodcastManager.Models
{
    public interface IFeedDownloader
    {
        Feed Download(string url);
        void Download(string url, string directory, AsyncCompletedEventHandler completedEventHandler, DownloadProgressChangedEventHandler downloadProgressChangedEventHandler);
    }
}
