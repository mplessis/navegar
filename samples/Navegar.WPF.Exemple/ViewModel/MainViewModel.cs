using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace Navegar.WPF.Exemple.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DispatcherTimer _timerLoadAccueil;

        /// <summary>
        /// Permet de contrôler la propriété CurrentView
        /// </summary>
        private ViewModelBase _currentView;

        /// <summary>
        /// L'attribut CurrentViewNavigation permet de définir automatiquement, quelle propriété du viewmodel
        /// devra être utilisé pour charger le viewmodel vers lequel la navigation va s'effectuer
        /// </summary>
        [CurrentViewNavigation]
        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }

        public MainViewModel()
        {
            //Permet d'éviter l'appel à la navigation pendant le chargement du viewmodel principal. Sans ceci le chargement du MainViewModel
            //ne se passe pas correctement puisque le chargement du viewmodel n'est pas possible
            _timerLoadAccueil = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 500)
            };
            _timerLoadAccueil.Tick += LoadAccueil;
            _timerLoadAccueil.Start();
        }

        /// <summary>
        /// Navigation vers le premier viewmodel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadAccueil(object sender, EventArgs e)
        {
            _timerLoadAccueil.Stop();
            SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<FirstViewModel>();
        }
    }
}