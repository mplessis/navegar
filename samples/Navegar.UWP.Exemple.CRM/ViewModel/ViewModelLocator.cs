using CommonMobiles.ViewModels;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Enums;
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
            //Si le service de navigation n'est pas enregistr� dans l'IOC
            if (!SimpleIoc.Default.IsRegistered<INavigation>())
            {
                SimpleIoc.Default.Register<INavigation, Navigation>();
            }

            //Association des ViewModels et des Views associ�es
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<LandingPageViewModel, LandingPage>();
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<ClientPageViewModel, ClientPage>(BackButtonViewEnum.Manual);
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<ListClientsPageViewModel, ListClientsPage>(BackButtonViewEnum.None);
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<CommandePageViewModel, CommandePage>(BackButtonViewEnum.Auto);
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<ListCommandesPageViewModel, ListCommandesPage>(BackButtonViewEnum.Auto);
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