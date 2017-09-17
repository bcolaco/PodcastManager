namespace PodcastManager
{
    public interface IFileSystem
    {
        void FileWriteAllText(string fileName, string text);
        string FileReadAllText(string fileName);
    }
}
