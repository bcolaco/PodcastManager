using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PodcastManager
{
    /// <summary>
    /// Interaction logic for AddPoscastWindow.xaml
    /// </summary>
    public partial class AddPoscastWindow : Window
    {
        public string Url { get { return UrlTextBox.Text; } }

        public AddPoscastWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
