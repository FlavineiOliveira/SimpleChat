using SimpleChat.ViewModels;
using System.Threading.Tasks;

namespace SimpleChat.Services.Interfaces
{
    public interface INavigationService
    {
        Task RemoveLastFromBackStackAsync();

        Task RemoveLastFromBackStackAsync(object parameter);

        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
    }
}
