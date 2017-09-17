using System.Collections.Generic;

namespace PodcastManager.Tests
{
    public class DummyFileSystem : IFileSystem
    {
        Dictionary<string, string> fs = new Dictionary<string, string>();
        public void FileWriteAllText(string fileName, string text)
        {
            fs[fileName] = text;
        }

        public string FileReadAllText(string fileName)
        {
            return fs[fileName];
        }
    }
}