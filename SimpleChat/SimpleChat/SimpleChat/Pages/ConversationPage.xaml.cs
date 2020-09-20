using SimpleChat.Messages;
using SimpleChat.ViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleChat.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConversationPage : ContentPage
    {
        public ConversationPage()
        {
            InitializeComponent();

            RegisterMessagingCenter();
        }

        private void RegisterMessagingCenter()
        {
            MessagingCenter.Subscribe<object>(this, MessageCenter.SCROLL_LISTVIEW_TO_LAST_INDEX, (sender) =>
            {
                ScrollListToLastItem();
            });
        }

        private void ScrollListToLastItem()
        {
            var lastItem = lstMessages.ItemsSource.Cast<object>().Last();
            lstMessages.ScrollTo(lastItem, ScrollToPosition.End, true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((ConversationViewModel)BindingContext).LoadScreen();
        }
    }
}