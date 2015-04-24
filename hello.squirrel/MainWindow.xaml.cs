﻿using System.Windows;
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
            // cf.: https://github.com/mkoertgen/hello.nuget.server
            using (var mgr = new UpdateManager("http://localhost:8888/nuget/feed", "hello.squirrel", FrameworkVersion.Net45))
            {
                await mgr.UpdateApp();
            }
        }
    }
}
