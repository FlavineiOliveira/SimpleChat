using SimpleChat.Services.Interfaces;
using SimpleChat.SimpleInjector;
using SimpleChat.ViewModels;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleChat.Services
{
    public class NavigationService : INavigationService
    {
        public async Task RemoveLastFromBackStackAsync()
        {
            NavigationPage navigationPage = Application.Current.MainPage as NavigationPage;

            if (navigationPage != null)
            {
                await navigationPage.PopAsync();
            }
        }

        public async Task RemoveLastFromBackStackAsync(object parameter)
        {
            NavigationPage navigationPage = Application.Current.MainPage as NavigationPage;

            if (navigationPage != null)
            {
                await navigationPage.PopAsync();
            }

            Page page = navigationPage.CurrentPage;

            if (page != null)
            {
                await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePageAndBinding(viewModelType);

            if (!string.IsNullOrEmpty(viewModelType.Name) &&
                (viewModelType.Name.Contains("Home") || viewModelType.Name.Contains("Main")))
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
            else
            {
                var navigationPage = Application.Current.MainPage as NavigationPage;
                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = ReceivePathPageViewModel(viewModelType.FullName);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePageAndBinding(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            ViewModelBase viewModel = ServiceLocator.Container.GetInstance(viewModelType) as ViewModelBase;
            page.BindingContext = viewModel;

            return page;
        }

        private string ReceivePathPageViewModel(string pPath)
        {
            string path = pPath.Replace("ViewModels.", "Pages.").Replace("ViewModel", "Page");

            return path;
        }
    }
}
