using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Interfaces;
using Navegar.Plateformes.Net.Standard;

namespace Navegar.NetCore.WPF.Exemple.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //1. Enregistrer la classe de navigation dans l'IOC
            SimpleIoc.Default.Register<INavigationStandard, Navigation>();

            //2. Générer le viewmodel principal, le type du viewmodel
            //peut être n'importe lequel
            //Cette génération va permettre de créer au sein de la 
            //navigation, une instance unique pour le viewmodel principal,
            //qui sera utilisée par la classe de navigation
            SimpleIoc.Default.GetInstance<INavigationStandard>()
                             .GenerateMainViewModelInstance<MainViewModel>();
        }

        public MainViewModel Main => SimpleIoc.Default.GetInstance<INavigationStandard>()
            .GetMainViewModelInstance<MainViewModel>();
    }
}