using SimpleChat.Messages;
using SimpleChat.Models;
using SimpleChat.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleChat.ViewModels
{
    public class ConversationViewModel : ViewModelBase
    {
        #region Properties

        private string _textIn;

        public string TextIn
        {
            get { return _textIn; }
            set 
            {
                _textIn = value;
                OnPropertyChanged(nameof(TextIn));
            }
        }


        private ObservableCollection<Message> _messages;

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set 
            { 
                _messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }
        #endregion

        #region Commands

        private ICommand _sendCommand;

        public ICommand SendCommand
        {
            get { return _sendCommand; }
            set { 
                _sendCommand = value;
                OnPropertyChanged(nameof(SendCommand));
            }
        }

        #endregion

        public ConversationViewModel(INavigationService navigationService) : base(navigationService) {}

        public override Task InitializeAsync(object navigationData)
        {
            base.InitializeAsync(navigationData);

            Messages = new ObservableCollection<Message>();

            SendCommand = new Command(Send);

            return Task.FromResult(false);
        }

        private void Send()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (string.IsNullOrEmpty(TextIn))
                    return;

                var message = new Message
                {
                    Text = TextIn,
                    MessageDateTime = DateTime.Now,
                    UserId = App.User.Id
                };

                Messages.Add(message);

                MessagingCenter.Send<object>(this, MessageCenter.SCROLL_LISTVIEW_TO_LAST_INDEX);

                TextIn = string.Empty;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
