using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Interfaces;
using Navegar.WPF.Exemple.Class;

namespace Navegar.WPF.Exemple.ViewModel
{
    public class FirstViewModel : BlankViewModelBase
    {
        public RelayCommand NextViewModelCommand { get; set; }

        /// <summary>
        /// Permet de contrôler la propriété Data
        /// </summary>
        private Data _data;
        public Data Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged("Data");
            }
        }

        public FirstViewModel()
        {
            NextViewModelCommand = new RelayCommand(NextViewModel);
            Data = new Data();
        }

        /// <summary>
        /// Permet de charger le second viewmodel
        /// <remarks>Voius pouvez tester la recherche de méthodes, filtrées par types de paramétres, en changeant la ligne d'appel ci-dessous</remarks>
        /// </summary>
        private void NextViewModel()
        {
            //Version avec paramétre de type classe personnalisée
            SimpleIoc.Default.GetInstance<INavigationWpf>()
                     .NavigateTo<SecondViewModel>(this, null, "LoadData", new object[] { Data }, true);

            //version avec paramétre de type string
            //SimpleIoc.Default.GetInstance<INavigationWpf>()
            //         .NavigateTo<SecondViewModel>(this, null, "LoadData", new object[] { "Version avec string" }, true);
        }
    }
}
