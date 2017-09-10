namespace PodcastManager
{
    public interface IWebClient
    {
        string DownloadString(string address);
        void DownloadFile(string addres, string fileName);
    }
}