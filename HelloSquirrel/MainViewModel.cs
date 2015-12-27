using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Caliburn.Micro;
using Squirrel;

namespace HelloSquirrel
{
    public class MainViewModel : Screen
    {
        private readonly IUpdateManager _updateManager;
        private readonly IMessageService _messageService;
        private bool _isBusy;
        private bool _isUpdating;
        private int _progress;

        public MainViewModel(IUpdateManager updateManager, IMessageService messageService = null)
        {
            if (updateManager == null) throw new ArgumentNullException(nameof(updateManager));
            _updateManager = updateManager;
            _messageService = messageService ?? new MessageService();

            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            DisplayName = "HelloSquirrel";
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsUpdating
        {
            get { return _isUpdating; }
            private set
            {
                if (value.Equals(_isUpdating)) return;
                _isUpdating = value;
                NotifyOfPropertyChange();
            }
        }

        public int Progress
        {
            get { return _progress; }
            private set
            {
                if (value.Equals(_progress)) return;
                _progress = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(IsIndeterminate));
            }
        }

        public bool IsIndeterminate => Progress <= 0;

        public void DoShowInfo()
        {
            try
            {
                var version = 
                    _updateManager.CurrentlyInstalledVersion()?.ToString() 
                    ?? GitVersionInformation.FullSemVer;
                _messageService.Show($"You are running: {version}", "Information");
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Could not show info: " + ex);
                _messageService.Show("Could not show info: " + ex.Message, "Error", MessageButton.Ok, MessageImage.Error);
            }
        }

        public async void DoCheckForUpdate()
        {
            try
            {
                var updateInfo = await InBusy(() => _updateManager.CheckForUpdate());

                if (updateInfo.CurrentlyInstalledVersion == null)
                    throw new InvalidOperationException("Could not determine currently installed version");

                var currentVersion = updateInfo.CurrentlyInstalledVersion.Version;
                var newVersion = updateInfo.FutureReleaseEntry.Version;
                if (newVersion > currentVersion)
                {
                    if (_messageService.Show("Update " + newVersion + " available. Dou you want to update now?",
                        "Update", MessageButton.YesNo, MessageImage.Question) != MessageResult.Yes) return;

                    await InUpdate(() => _updateManager.UpdateApp(p => Progress = p));

                    if (_messageService.Show("Update applied. Restart application to take effect.",
                        "Information", MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
                        Program.Restart();
                }
                else
                {
                    _messageService.Show("You are up to date: " + currentVersion, "Information", MessageButton.Ok,
                        MessageImage.Information);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Could not check for update: " + ex);
                _messageService.Show("Could not check for update: " + ex.Message, "Error", MessageButton.Ok, MessageImage.Error);
            }
        }

        private async Task<TResult> InBusy<TResult>(Func<Task<TResult>> op)
        {
            IsBusy = true;
            try { return await op(); }
            finally { IsBusy = false; }
        }

        private async Task<TResult> InUpdate<TResult>(Func<Task<TResult>> op)
        {
            IsUpdating = true;
            Progress = 0;
            try { return await op(); }
            finally { IsUpdating = false; }
        }
    }
}