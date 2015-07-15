using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Navegar.XamarinForms.Exemple.CRM.Views;

namespace Navegar.XamarinForms.Exemple.CRM.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //Enregistrement de Navegar dans l'IOC
            SimpleIoc.Default.Register<INavigation, Navigation>();

            //Liaisons entre les ViewModels et les Views
            ServiceLocator.Current.GetInstance<INavigation>().RegisterView<ListClientsPageViewModel, ListClientsPage>();
            ServiceLocator.Current.GetInstance<INavigation>().RegisterView<ListCommandesPageViewModel, ListCommandesPage>();
            ServiceLocator.Current.GetInstance<INavigation>().RegisterView<ClientPageViewModel, ClientPage>();
            ServiceLocator.Current.GetInstance<INavigation>().RegisterView<CommandePageViewModel, CommandePage>();
        }
    }
}
