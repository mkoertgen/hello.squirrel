using System;
using System.Diagnostics;
using System.Windows;
using Squirrel;

namespace HelloSquirrel
{
    static class Program
    {
        private const string AppName = "HelloSquirrel";
        private const string UpdateUrl = "http://localhost:8080/Releases";
        // ReSharper disable once NotAccessedField.Local
        static bool _showTheWelcomeWizard;

        internal static readonly UpdateManager Updater = new UpdateManager(UpdateUrl, AppName, FrameworkVersion.Net45);

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // NOTE: launch debugger after signal so we will jump into a new debugger session!
#if DEBUG
                if (!Debugger.IsAttached) Debugger.Launch();
#endif

                // Note, in most of these scenarios, the app exits after this method completes!
                SquirrelAwareApp.HandleEvents(
                    // ReSharper disable RedundantArgumentName
                    onInitialInstall: v => Updater.CreateShortcutForThisExe(),
                    onAppUpdate: v => Updater.CreateShortcutForThisExe(),
                    // ReSharper restore RedundantArgumentName
                    onAppUninstall: v => Updater.RemoveShortcutForThisExe(),
                    onFirstRun: () => _showTheWelcomeWizard = true);


                var application = new App();
                application.Run();
            }
            catch (Exception ex)
            {
                OnUnhandledException(ex);
            }

            Environment.Exit(0);
        }

        private static void OnUnhandledException(Exception ex)
        {
            Trace.TraceError("An unhandled application error occurred: {0}", ex);

            // if the bootstrapper fails Application.Current.MainWindow will be null.
            // so we create a hidden dummy window to show the message box on.
            var dummyWindow = new Window { Visibility = Visibility.Hidden };
            dummyWindow.Show();

            MessageBox.Show("An unhandled application error occurred: " + ex.Message, "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);

            Environment.Exit(1);
        }
    }
}