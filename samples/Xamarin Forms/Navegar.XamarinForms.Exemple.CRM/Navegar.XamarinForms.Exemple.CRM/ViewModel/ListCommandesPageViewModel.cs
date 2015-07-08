using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;

namespace Navegar.XamarinForms.Exemple.CRM.ViewModel
{
    public class ListCommandesPageViewModel : ViewModelBase
    {
        private string _maintText2;

        public string MainText2
        {
            get
            {
                return _maintText2;
            }
            set
            {
                _maintText2 = value;
                RaisePropertyChanged(() => MainText2);
            }
        }

        public ListCommandesPageViewModel()
        {
            ServiceLocator.Current.GetInstance<INavigation>().RegisterBackPressedAction<ListCommandesPageViewModel>(OnBackPressed);
        }

        public void OnLoad()
        {
            MainText2 = "new text";
        }

        private void OnBackPressed()
        {
            if (ServiceLocator.Current.GetInstance<INavigation>().CanGoBack())
            {
                ServiceLocator.Current.GetInstance<INavigation>().GoBack();
            }
        }
    }
}
