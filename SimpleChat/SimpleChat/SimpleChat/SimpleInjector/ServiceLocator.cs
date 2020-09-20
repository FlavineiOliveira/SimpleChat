using SimpleChat.Services;
using SimpleChat.Services.Interfaces;
using SimpleInjector;

namespace SimpleChat.SimpleInjector
{
    public class ServiceLocator
    {
        public static Container Container { get; private set; }

        public static void RegisterInterfaces()
        {
            Container = new Container();
            Container.Options.ResolveUnregisteredConcreteTypes = true;

            Container.Register<INavigationService, NavigationService>();
            Container.Register<IDialogService, DialogService>();
        }
    }
}
