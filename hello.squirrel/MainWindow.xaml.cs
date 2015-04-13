using System.Windows;
using Squirrel;

namespace hello.squirrel
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void UpdateOnClick(object sender, RoutedEventArgs e)
        {
            using (var mgr = new UpdateManager("https://path/to/my/update/folder", "hello.squirrel", FrameworkVersion.Net45))
            {
                await mgr.UpdateApp();
            }
        }
    }
}
