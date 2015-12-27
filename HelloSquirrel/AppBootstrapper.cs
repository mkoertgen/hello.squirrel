using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Autofac;
using Autofac.Core;
using Caliburn.Micro;
using Squirrel;

namespace HelloSquirrel
{
    /// <summary>
    /// DI configuration for AutoFac.
    /// Adapted from http://www.nuget.org/packages/Analects.Caliburn.Micro.Autofac/1.0.0
    /// </summary>
    internal class AppBootstrapper : BootstrapperBase
    {
        private AppBootstrapper(bool useApplication)
            : base(useApplication)
        {
            Initialize();
        }

        public AppBootstrapper()
            : this(true)
        { }

        // for tests
        private IContainer Container { get; set; }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterInstance(CreateUpdater()).SingleInstance();

            Container = builder.Build();
        }


        // ReSharper disable once NotAccessedField.Local
        internal static bool ShowTheWelcomeWizard;

        private static IUpdateManager CreateUpdater()
        {
            const string appName = "HelloSquirrel";
            const string updateUrl = "http://localhost:8080/Releases";
            var updater = new UpdateManager(updateUrl, appName);
            // Note, in most of these scenarios, the app exits after this method completes!
            SquirrelAwareApp.HandleEvents(
                // ReSharper disable RedundantArgumentName
                onInitialInstall: v => updater.CreateShortcutForThisExe(),
                onAppUpdate: v => updater.CreateShortcutForThisExe(),
                // ReSharper restore RedundantArgumentName
                onAppUninstall: v => updater.RemoveShortcutForThisExe(),
                onFirstRun: () => ShowTheWelcomeWizard = true);

            return updater;
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            if (string.IsNullOrWhiteSpace(key))
            {
                object result;
                if (Container.TryResolve(service, out result))
                    return result;
            }
            else
            {
                object result;
                if (Container.TryResolveNamed(key, service, out result))
                    return result;
            }
            throw new DependencyResolutionException(string.Format(CultureInfo.CurrentCulture, "Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(new[] { service })) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
        }
    }
}