using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Navegar.Libs.Interfaces;
using Navegar.Plateformes.NET;
using Navegar.Plateformes.NET.WPF;

namespace Navegar.WPF.Exemple.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //1. Enregistrer la classe de navigation dans l'IOC
            SimpleIoc.Default.Register<INavigationWpf, Navigation>();

            //2. G�n�rer le viewmodel principal, le type du viewmodel
            //peut �tre n'importe lequel
            //Cette g�n�ration va permettre de cr�er au sein de la 
            //navigation, une instance unique pour le viewmodel principal,
            //qui sera utilis�e par la classe de navigation
            SimpleIoc.Default.GetInstance<INavigationWpf>()
                             .GenerateMainViewModelInstance<MainViewModel>();
        }

        public MainViewModel Main => SimpleIoc.Default.GetInstance<INavigationWpf>()
            .GetMainViewModelInstance<MainViewModel>();
    }
}