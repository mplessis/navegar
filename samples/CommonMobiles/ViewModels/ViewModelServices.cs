using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Enums;
using Navegar.Libs.Interfaces;

namespace CommonMobiles.ViewModels
{
    public class ViewModelServices : ViewModelBase
    {
        #region properties

        /// <summary>
        /// Permet de retourner l'instance en cours de la navigation Navegar
        /// </summary>
        public INavigation NavigationService
        {
            get { return SimpleIoc.Default.GetInstance<INavigation>(); }
        }

        /// <summary>
        /// Indique si l'application est de type Windows et non Windows Phone
        /// </summary>
        private bool _isWindowsApp;

        public bool IsWindowsApp
        {
            get { return _isWindowsApp; }
            set
            {
                _isWindowsApp = value;
                RaisePropertyChanged(() => IsWindowsApp);
            }
        }

        public RelayCommand CancelCommand { get; set; }

        #endregion


        public ViewModelServices()
        {
            if (NavigationService.HasBackButton == BackButtonTypeEnum.None)
            {
                IsWindowsApp = true;
                CancelCommand = new RelayCommand(Cancel, CanCancel);
            }
        }

        #region relaycommand

        /// <summary>
        /// Permet de revenir en arriére
        /// Cette fonction n'est utile que pour les versions sans bouton physique
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
