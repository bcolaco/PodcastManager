using PodcastManager.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PodcastManager.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected ObservableCollection<Feed> feeds;
        public ReadOnlyObservableCollection<Feed> Feeds { get; }
        private Configuration Configuration { get; }

        private Feed _SelectedFeed;
        public Feed SelectedFeed {
            get { return _SelectedFeed; }
            set
            {
                _SelectedFeed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFeed)));
            }
        }

        private readonly IFeedDownloader feedDownloader;
        private readonly IConfigurationPersister configurationPersister;
        
        public ICommand DownloadCommand { get; set; }

        public MainWindowViewModel() : this(new FeedDownloader(), new ConfigurationPersister())
        {
            if(DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                if(Feeds.Count > 0)
                {
                    SelectedFeed = Feeds[0];
                }
            }
        }

        public MainWindowViewModel(IFeedDownloader feedDownloader, IConfigurationPersister configurationPersister)
        {
            this.feedDownloader = feedDownloader;
            this.configurationPersister = configurationPersister;

            feeds = new ObservableCollection<Feed>();
            Feeds = new ReadOnlyObservableCollection<Feed>(feeds);
            DownloadCommand = new RelayCommand((param) => Download(param as FeedItem), (param) => !(param as FeedItem)?.Downloading ?? false);

            Configuration = configurationPersister.Load();
            foreach(var feed in Configuration.Feeds)
            {
                feeds.Add(feed);
            }
        }

        public void Download(FeedItem item)
        {
            item.Downloading = true;
            feedDownloader.Download(
                item.Url,
                Configuration.DownloadDirectory,
                (sender, e) => {
                    item.Downloading = false;
                },
                (sender, e) => {
                    item.DownloadPercentage = e.ProgressPercentage;
                });
        }

        public void AddFeed(string url)
        {
            var feed = new Feed(url);
            feeds.Add(feed);
            SelectedFeed = feed;
            UpdateSelectedFeed();
        }

        public void RemoveFeed(Feed feed)
        {
            feeds.Remove(feed);
            SaveConfiguration();
        }

        public void SaveConfiguration()
        {
            configurationPersister.Save(Configuration);
        }

        public void UpdateSelectedFeed()
        {
            var update = feedDownloader.Download(_SelectedFeed.Url);
            _SelectedFeed.Title = update.Title;
            _SelectedFeed.ImageUrl = update.ImageUrl;
            _SelectedFeed.Items = update.Items;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFeed)));
            SaveConfiguration();
        }

        public void SetDownloadDirectory(string path)
        {
            Configuration.DownloadDirectory = path;
            configurationPersister.Save(Configuration);
        }
    }
}
