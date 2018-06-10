using PodcastManager.Models;
using PodcastManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PodcastManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addPodcastDialog = new AddPoscastWindow
            {
                Owner = this
            };
            if(addPodcastDialog.ShowDialog() ?? false)
            {
                viewModel.AddFeed(addPodcastDialog.Url);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveFeed((Feed)PodcastListBox.SelectedItem);
        }

        private void MaximumItems_LostFocus(object sender, RoutedEventArgs e)
        {
            viewModel.SaveConfiguration();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateSelectedFeed();
        }

        private void SetDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    viewModel.SetDownloadDirectory(dlg.SelectedPath);
                }
            }
        }
    }
}
