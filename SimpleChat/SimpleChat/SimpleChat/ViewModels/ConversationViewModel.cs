using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using SimpleChat.FirebaseRealTime;
using SimpleChat.Messages;
using SimpleChat.Models;
using SimpleChat.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleChat.ViewModels
{
    public class ConversationViewModel : ViewModelBase
    {
        #region Attributes

        private FirebaseClient _firebaseClient;

        #endregion

        #region Properties

        public string TargetId { get; set; }

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

        public ConversationViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService, dialogService)
        {
        }

        public override Task InitializeAsync(object navigationData)
        {
            base.InitializeAsync(navigationData);

            if(navigationData != null && navigationData is User)
            {
                var obj = navigationData as User;

                TargetId = obj.Id;
                TargetName = obj.Name;
            }

            if(Messages == null)
                Messages = new ObservableCollection<Message>();

            SendCommand = new Command(Send);

            return Task.FromResult(false);
        }

        public void LoadScreen()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                _firebaseClient = new FirebaseClient(FirebaseEnvironment.ADDRESS_FIREBASE);

                AddListenerMessage();
            }
            finally
            {
                IsBusy = false;
            }
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

                AddMessage(TextIn);

                MessagingCenter.Send<object>(this, MessageCenter.SCROLL_LISTVIEW_TO_LAST_INDEX);

                TextIn = string.Empty;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddListenerMessage()
        {
            _firebaseClient
                .Child(FirebaseEnvironment.CHILD_MESSAGES)
                .AsObservable<Message>()
                .Subscribe(d =>
                {
                    if (d.EventType == FirebaseEventType.InsertOrUpdate)
                    {
                        if (d.Object != null)
                        {
                            Message item = null;
                            
                            if(Messages != null)
                                item = Messages.Where(m => m.KeyMessage == d.Object.KeyMessage).FirstOrDefault();
                            else
                                Messages = new ObservableCollection<Message>();

                            if(item == null)
                            {
                                Messages.Add(d.Object);

                                MessagingCenter.Send<object>(this, MessageCenter.SCROLL_LISTVIEW_TO_LAST_INDEX);
                            }
                        }
                    }
                });
        }

        private void AddMessage(string text)
        {
            AddMessage(text, DateTime.Now, App.User.Id);
        }

        private void AddMessage(string text, DateTime messageDateTime, string userId)
        {
            var message = new Message
            {
                Text = text,
                MessageDateTime = messageDateTime,
                UserId = userId,
                KeyMessage = (Messages.Count + 1).ToString()
            };

            Messages.Add(message);

            _firebaseClient
                .Child(FirebaseEnvironment.CHILD_MESSAGES)
                .Child(message.KeyMessage)
                .PutAsync(message);
        }
    }
}
