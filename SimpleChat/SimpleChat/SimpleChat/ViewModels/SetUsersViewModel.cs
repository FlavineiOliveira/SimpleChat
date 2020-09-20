using SimpleChat.Exceptions;
using SimpleChat.Models;
using SimpleChat.Services.Interfaces;
using SimpleChat.SimpleInjector;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleChat.ViewModels
{
    public class SetUsersViewModel : ViewModelBase
    {
        #region Properties

        private string _targetUserId;

        public string TargetUserId
        {
            get { return _targetUserId; }
            set 
            { 
                _targetUserId = value;
                OnPropertyChanged(nameof(TargetUserId));
            }
        }

        private string _targetName;

        public string TargetName
        {
            get { return _targetName; }
            set
            {
                _targetName = value;
                OnPropertyChanged(nameof(TargetName));
            }
        }

        private string _myUserId;

        public string MyUserId
        {
            get { return _myUserId; }
            set
            {
                _myUserId = value;
                OnPropertyChanged(nameof(MyUserId));
            }
        }

        #endregion

        #region Commands

        private ICommand _navigateToChatCommand;

        public ICommand NavigateToChatCommand
        {
            get { return _navigateToChatCommand; }
            set
            {
                _navigateToChatCommand = value;
                OnPropertyChanged(nameof(NavigateToChatCommand));
            }
        }

        #endregion

        public SetUsersViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService, dialogService)
        {
        }

        public override Task InitializeAsync(object navigationData)
        {
            base.InitializeAsync(navigationData);

            NavigateToChatCommand = new Command(NavigateToChat);

            return Task.FromResult(false);
        }

        private void NavigateToChat()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                Criticize();

                App.User.Id = MyUserId;

                var targetUser = new User
                {
                    Id = TargetUserId,
                    Name = TargetName
                };

                NavigationService.NavigateToAsync<ConversationViewModel>(targetUser);
            }
            catch(FoException fo)
            {
                DialogService.DisplayAlert("Atention", fo.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Criticize()
        {
            if (string.IsNullOrEmpty(TargetUserId))
                throw new FoException("Set the target user ID to send message.");

            if (string.IsNullOrEmpty(TargetName))
                throw new FoException("Set the target user name to show in screen in chat.");

            if (string.IsNullOrEmpty(MyUserId))
                throw new FoException("Set your user ID to send message.");

            if (TargetUserId == MyUserId)
                throw new FoException("You cannot send message for you.");
        }
    }
}
