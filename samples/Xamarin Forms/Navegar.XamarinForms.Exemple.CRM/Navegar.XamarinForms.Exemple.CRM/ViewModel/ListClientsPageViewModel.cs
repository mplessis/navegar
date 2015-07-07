using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;

namespace Navegar.XamarinForms.Exemple.CRM.ViewModel
{
    public class ListClientsPageViewModel : ViewModelBase
    {
        public string MainText { get; set; }

        #region properties

        public RelayCommand GoPageCommand { get; set; }

        #endregion

        public ListClientsPageViewModel()
        {
            MainText = "Hello Word depuis Navegar pour Xamarin.Forms !";
            GoPageCommand = new RelayCommand(GoPage);
        }

        private void GoPage()
        {
            ServiceLocator.Current.GetInstance<INavigation>().NavigateTo<ListCommandesPageViewModel>(new object[] {}, "OnLoad", new object[] {}, true);
        }
    }
}
