using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace Navegar.WPF.Exemple.ViewModel
{
    public class FirstViewModel : BlankViewModelBase
    {
        public RelayCommand NextViewModelCommand { get; set; }

        /// <summary>
        /// Permet de contrôler la propriété Data
        /// </summary>
        private string _data;
        public string Data
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
        }

        //permet de charger le second viewmodel
        private void NextViewModel()
        {
            SimpleIoc.Default.GetInstance<INavigation>()
                     .NavigateTo<SecondViewModel>(this, null, "LoadData", new object[]{ Data }, true);
        }
    }
}
