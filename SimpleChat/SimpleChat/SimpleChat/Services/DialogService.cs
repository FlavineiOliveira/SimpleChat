using SimpleChat.Services.Interfaces;
using Xamarin.Forms;

namespace SimpleChat.Services
{
    public class DialogService : IDialogService
    {
        public void DisplayAlert(string title, string message, string cancel)
        {
            Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
