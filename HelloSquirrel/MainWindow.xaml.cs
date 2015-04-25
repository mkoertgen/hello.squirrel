using System;
using System.Windows;
using Squirrel;

namespace HelloSquirrel
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, a) =>
            {

                if (Program.ShowTheWelcomeWizard)
                    MessageBox.Show("First Run. Welcome to HelloSquirrel. TODO: some intro");
            };
        }

        private async void UpdateOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                BusyIndicator.Visibility = Visibility.Visible;

                var updateInfo = await Program.Updater.CheckForUpdate();
                if (updateInfo.FutureReleaseEntry.Version > updateInfo.CurrentlyInstalledVersion.Version)
                {
                    if (MessageBox.Show("Update available. Dou you want to update now?", "Update",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                        await Program.Updater.UpdateApp();
                }
                else
                {
                    MessageBox.Show("You are up to date", "Information", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during checking for updates: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                BusyIndicator.Visibility = Visibility.Hidden;
            }
        }
    }
}
