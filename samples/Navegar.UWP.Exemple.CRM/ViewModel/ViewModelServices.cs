using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Navegar.Libs.Interfaces;

namespace Navegar.UWP.Exemple.CRM.ViewModel
{
    public class ViewModelServices : ViewModelBase
    {
        #region properties

        /// <summary>
        /// Permet de retourner l'instance en cours de la navigation Navegar
        /// </summary>
        public INavigation NavigationService
        {
            get { return ServiceLocator.Current.GetInstance<INavigation>(); }
        }

        #endregion


        public ViewModelServices()
        {}
    }
}
