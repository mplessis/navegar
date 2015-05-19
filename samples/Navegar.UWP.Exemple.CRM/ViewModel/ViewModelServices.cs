using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Navegar.UWP.Win10;

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

        /// <summary>
        /// Indique si l'application a un bouton back (physique ou virtuel)
        /// </summary>
        public bool IsWindowsApp => NavigationService.HasBackButton;

        /// <summary>
        /// Commande de retour arriére pour les device sans bouton
        /// </summary>
        public RelayCommand CancelCommand { get; set; }

        #endregion


        public ViewModelServices()
        {
            CancelCommand = new RelayCommand(Cancel, CanCancel);
        }

        #region relaycommand

        /// <summary>
        /// Permet de revenir en arriére
        /// Cette fonction n'est utile que pour les versions qui n'ont pas de bouton back (physique ou virtuel) puisque les autres utilisent le backbutton avec l'implémentation livré dans Navegar
        /// ou bien une surcharge de cette implémentation (voir l'exemple dans le fichier App.xaml.cs)
        /// </summary>
        private void Cancel()
        {
            if (NavigationService.CanGoBack())
            {
                //Lorsque l'on revient d'une commande on rafraichi la liste des commandes sur la page de liste
                if (NavigationService.GetTypeViewModelToBack() == typeof (ListCommandesPageViewModel))
                {
                    //Permet de relancer la fonction LoadDatas aprés la navigation arriére vers la liste des commandes
                    NavigationService.GoBack("LoadDatas", new object[] {});
                }
                else
                {
                    NavigationService.GoBack();
                }
            }
        }

        /// <summary>
        /// Permet de vérifier que l'on peut revenir en arriére
        /// </summary>
        /// <returns></returns>
        private bool CanCancel()
        {
            return NavigationService.CanGoBack();
        }

        #endregion

    }
}
