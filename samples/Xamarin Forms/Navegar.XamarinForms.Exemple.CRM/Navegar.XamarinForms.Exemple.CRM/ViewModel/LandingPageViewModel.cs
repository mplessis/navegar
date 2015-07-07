using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;

namespace Navegar.XamarinForms.Exemple.CRM.ViewModel
{
    public class LandingPageViewModel : ViewModelBase
    {
        #region properties
        
        public RelayCommand GoPageCommand { get; set; }
        #endregion

        public LandingPageViewModel()
        {
            GoPageCommand = new RelayCommand(GoPage);
        }

        private void GoPage()
        {
            ServiceLocator.Current.GetInstance<INavigation>().NavigateTo<ListClientsPageViewModel>(true);
        }
    }
}
