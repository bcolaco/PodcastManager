using System.IO;

namespace PodcastManager
{
    public class FileSystem : IFileSystem
    {
        public void FileWriteAllText(string fileName, string text)
        {
            File.WriteAllText(fileName, text);
        }

        public string FileReadAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
