namespace PodcastManager.Models
{
    public interface IConfigurationPersister
    {
        void Save(Configuration configuration);
        Configuration Load();
    }
}
