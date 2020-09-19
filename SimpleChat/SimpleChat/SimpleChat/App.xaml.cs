using SimpleChat.Models;
using SimpleChat.Services.Interfaces;
using SimpleChat.SimpleInjector;
using SimpleChat.ViewModels;
using Xamarin.Forms;


namespace SimpleChat
{
    public partial class App : Application
    {
        public static User User = new User();

        public App()
        {
            InitializeComponent();

            User.Id = "1";

            ServiceLocator.RegisterInterfaces();

            var navigationService = ServiceLocator.Container.GetInstance<INavigationService>();
            navigationService.NavigateToAsync<ConversationViewModel>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
