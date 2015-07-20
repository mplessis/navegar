using CommonMobiles.Controllers;
using CommonMobiles.POCO;
using GalaSoft.MvvmLight.Command;

namespace CommonMobiles.ViewModels
{
    public class CommandePageViewModel : ViewModelServices
    {
         #region properties

        private Commande _commandetCurrent;
        public Commande CommandeCurrent
        {
            get { return _commandetCurrent; }
            set
            {
                _commandetCurrent = value;
                RaisePropertyChanged(() => CommandeCurrent);
            }
        }

        public RelayCommand SaveCommand { get; set; }
        #endregion

        #region cstor

        public CommandePageViewModel(Commande commande)
        {
            CommandeCurrent = commande;
            SaveCommand = new RelayCommand(Save);
        }

        #endregion

        #region relaycommand

        private void Save()
        {
            CommandesController.Save(CommandeCurrent);
        }

        #endregion
    }
}
