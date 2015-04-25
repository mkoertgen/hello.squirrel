using System;
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
            
        }

        public async void DoCheckForUpdate()
        {
            IsBusy = true;
            try
            {
                var updateInfo = await _updateManager.CheckForUpdate();
                if (updateInfo.CurrentlyInstalledVersion != null 
                    && updateInfo.FutureReleaseEntry.Version > updateInfo.CurrentlyInstalledVersion.Version)
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
                _messageService.Show("An error occurred during checking for updates: " + ex.Message, "Error",
                    MessageButton.Ok, MessageImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}