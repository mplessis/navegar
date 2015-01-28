using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Navegar.UAP.Exemple.CRM.ViewModels;
using Navegar.UAP.Exemple.CRM.Views;
using Navegar.UAP.Win81;

namespace Navegar.UAP.Exemple.CRM.ViewModel
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
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<LandingPageViewModel, LandingPage>();
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<ClientPageViewModel, ClientPage>();
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<ListClientsPageViewModel, ListClientsPage>();
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<CommandePageViewModel, CommandePage>();
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<ListCommandesPageViewModel, ListCommandesPage>();
        }

        #region ViewModel Instance

        public LandingPageViewModel LandingPageViewModelInstance
        {
            get { return SimpleIoc.Default.GetInstance<INavigation>().GetViewModelInstance<LandingPageViewModel>(); }
        }

        public ClientPageViewModel ClientPageViewModelInstance
        {
            get { return SimpleIoc.Default.GetInstance<INavigation>().GetViewModelInstance<ClientPageViewModel>(); }
        }

        public ListClientsPageViewModel ListClientsPageViewModelInstance
        {
            get { return SimpleIoc.Default.GetInstance<INavigation>().GetViewModelInstance<ListClientsPageViewModel>(); }
        }

        public CommandePageViewModel CommandePagePageViewModelInstance
        {
            get { return SimpleIoc.Default.GetInstance<INavigation>().GetViewModelInstance<CommandePageViewModel>(); }
        }

        public ListCommandesPageViewModel ListCommandesPageViewModelInstance
        {
            get { return SimpleIoc.Default.GetInstance<INavigation>().GetViewModelInstance<ListCommandesPageViewModel>(); }
        }

        #endregion

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}