using System;
using System.IO;

namespace PodcastManager
{
    class Program
    {
        const string ConfigurationFileName = "";

        static void Main(string[] args)
        {
            Configuration configuration;
            try
            {
                configuration = Configuration.LoadFromFile(ConfigurationFileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Could not load configuration: {ex}");
                return;
            }
                
            foreach(var feed in configuration.Feeds)
            {
                try
                {
                    feed.Update(configuration);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Could not update feed {feed.Title}: {ex}");
                }
            }
            try
            {
                configuration.Save(ConfigurationFileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Could not update configuration file: {ex}");
            }
        }
    }
}
