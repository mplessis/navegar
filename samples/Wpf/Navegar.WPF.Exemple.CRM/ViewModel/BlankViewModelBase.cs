using GalaSoft.MvvmLight;

namespace Navegar.WPF.Exemple.ViewModel
{
    public class BlankViewModelBase : ViewModelBase
    {
        #region properties

        private bool _isRetour;
        public bool IsRetour
        {
            get { return _isRetour; }
            set
            {
                _isRetour = value;
                RaisePropertyChanged(() => IsRetour);
            }
        }
        #endregion

        #region public

        public void SetIsRetour(bool value)
        {
            IsRetour = value;
        }
        #endregion
    }
}
