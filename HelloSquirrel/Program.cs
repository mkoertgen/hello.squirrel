using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace HelloSquirrel
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // NOTE: launch debugger after signal so we will jump into a new debugger session!
#if DEBUG
                if (!Debugger.IsAttached) Debugger.Launch();
#endif

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

            // if the bootstrapper fails Application.Current.MainView will be null.
            // so we create a hidden dummy window to show the message box on.
            var dummyWindow = new Window { Visibility = Visibility.Hidden };
            dummyWindow.Show();

            MessageBox.Show("An unhandled application error occurred: " + ex.Message, "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);

            Environment.Exit(1);
        }

        internal static void Restart()
        {
            // NOTE: We need to have Squirrel start our application, i.e.
            //   Update.exe --processStart HelloSquirrel.exe
            // Just using the assembly won't start the updated app.
            
            //System.Windows.Forms.Application.Restart();
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var squirrelShortcut = Path.Combine(desktop, "HelloSquirrel.lnk");
            Process.Start(squirrelShortcut);

            Application.Current.Shutdown();
        }
    }
}