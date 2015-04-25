using System.Windows;

namespace HelloSquirrel
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();

            Loaded += (s, a) =>
            {
                if (AppBootstrapper.ShowTheWelcomeWizard)
                    MessageBox.Show("First Run. Welcome to HelloSquirrel. TODO: some intro");
            };
        }
    }
}
