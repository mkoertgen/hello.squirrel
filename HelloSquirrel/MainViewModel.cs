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

        public MainViewModel(IUpdateManager updateManager, IMessageService messageService = null)
        {
            if (updateManager == null) throw new ArgumentNullException("updateManager");
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

        public void DoShowInfo()
        {
            try
            {
                var version = _updateManager.CurrentlyInstalledVersion();
                if (version == null) throw new InvalidOperationException("Could not retreive current version");
                _messageService.Show(version.ToString());

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

                if (updateInfo.FutureReleaseEntry.Version > updateInfo.CurrentlyInstalledVersion.Version)
                {
                    if (_messageService.Show("Update available. Dou you want to update now?", "Update",
                        MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
                        await _updateManager.UpdateApp();
                }
                else
                {
                    _messageService.Show("You are up to date", "Information", MessageButton.Ok,
                        MessageImage.Information);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Could not check for update: " + ex);
                _messageService.Show("Could not check for update: " + ex.Message, "Error", MessageButton.Ok, MessageImage.Error);
            }
        }

        async Task<TResult> InBusy<TResult>(Func<Task<TResult>> op)
        {
            IsBusy = true;
            try { return await op(); }
            finally { IsBusy = false; }
        }
    }
}