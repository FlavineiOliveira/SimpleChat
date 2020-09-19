using SimpleChat.Services.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleChat.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Attributes

        public event PropertyChangedEventHandler PropertyChanged;

        protected readonly INavigationService NavigationService;

        #endregion

        #region Properties

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
        #endregion

        #region Commands

        private ICommand _backPageCommand;

        public ICommand BackPageCommand
        {
            get { return _backPageCommand; }
            set
            {
                _backPageCommand = value;
                OnPropertyChanged(nameof(BackPageCommand));
            }
        }

        #endregion

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            BackPageCommand = new Command(async () => await BackPage());

            return Task.FromResult(false);
        }

        private async Task BackPage()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                await NavigationService.RemoveLastFromBackStackAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
