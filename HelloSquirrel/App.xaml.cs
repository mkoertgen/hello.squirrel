using System;

namespace HelloSquirrel
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            // using the bootstrapper here instead of inside XAML enables better exception handling
            var bootstrapper = new AppBootstrapper();

            // dont let the compiler optimize this field away
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable HeuristicUnreachableCode
            if (bootstrapper == null)
                throw new InvalidOperationException();
            // ReSharper restore HeuristicUnreachableCode
        }
    }
}
