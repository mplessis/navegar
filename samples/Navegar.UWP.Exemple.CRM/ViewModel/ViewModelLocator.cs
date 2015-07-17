using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Navegar.Libs.Interfaces;
using Navegar.Plateformes.NetCore.UWP.Win10;
using Navegar.UWP.Exemple.CRM.Views;

namespace Navegar.UWP.Exemple.CRM.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //Si le service de navigation n'est pas enregistré dans l'IOC
            if (!SimpleIoc.Default.IsRegistered<INavigation>())
            {
                SimpleIoc.Default.Register<INavigation, Navigation>();
            }

            //Association des ViewModels et des Views associées
            ServiceLocator.Current.GetInstance<INavigation>().RegisterView<LandingPageViewModel, LandingPage>();
            ServiceLocator.Current.GetInstance<INavigation>().RegisterView<ClientPageViewModel, ClientPage>();
            ServiceLocator.Current.GetInstance<INavigation>().RegisterView<ListClientsPageViewModel, ListClientsPage>();
            ServiceLocator.Current.GetInstance<INavigation>().RegisterView<CommandePageViewModel, CommandePage>();
            ServiceLocator.Current.GetInstance<INavigation>().RegisterView<ListCommandesPageViewModel, ListCommandesPage>();
        }

        #region ViewModel Instance

        public LandingPageViewModel LandingPageViewModelInstance
        {
            get { return ServiceLocator.Current.GetInstance<INavigation>().GetViewModelInstance<LandingPageViewModel>(); }
        }

        public ClientPageViewModel ClientPageViewModelInstance
        {
            get { return ServiceLocator.Current.GetInstance<INavigation>().GetViewModelInstance<ClientPageViewModel>(); }
        }

        public ListClientsPageViewModel ListClientsPageViewModelInstance
        {
            get { return ServiceLocator.Current.GetInstance<INavigation>().GetViewModelInstance<ListClientsPageViewModel>(); }
        }

        public CommandePageViewModel CommandePagePageViewModelInstance
        {
            get { return ServiceLocator.Current.GetInstance<INavigation>().GetViewModelInstance<CommandePageViewModel>(); }
        }

        public ListCommandesPageViewModel ListCommandesPageViewModelInstance
        {
            get { return ServiceLocator.Current.GetInstance<INavigation>().GetViewModelInstance<ListCommandesPageViewModel>(); }
        }

        #endregion

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}