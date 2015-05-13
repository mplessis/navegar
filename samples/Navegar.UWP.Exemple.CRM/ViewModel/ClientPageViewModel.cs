using GalaSoft.MvvmLight.Command;
using Navegar.UWP.Exemple.CRM.Controllers;
using Navegar.UWP.Exemple.CRM.POCO;

namespace Navegar.UWP.Exemple.CRM.ViewModel
{
    public class ClientPageViewModel : ViewModelServices
    {
        #region properties

        private Client _clientCurrent;
        public Client ClientCurrent
        {
            get { return _clientCurrent; }
            set
            {
                _clientCurrent = value;
                RaisePropertyChanged(() => ClientCurrent);
            }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand LoadCommandesCommand { get; set; }
        #endregion

        #region cstor

        public ClientPageViewModel(Client client)
        {
            ClientCurrent = client;
            SaveCommand = new RelayCommand(Save);
            LoadCommandesCommand = new RelayCommand(LoadCommandes);
        }

        #endregion

        #region relaycommand

        private void Save()
        {
            ClientsController.Save(ClientCurrent);
        }

        /// <summary>
        /// Permet de naviguer dans la liste des commandes du client
        /// </summary>
        private void LoadCommandes()
        {
            //Navigation vers la page ListCommandesPage
            //this sert à indiquer que le ViewModel actuel (et donc par extension la page) sera ajouté à l'historique de navigation, afin que Navegar puisse savoir qu'il doit revenir vers cette page au Back
            //new object[]{ ClientCurrent } indique le paramétre de type client à passer au constucteur
            //"LoadDatas" est une fonction (public ou private) du ViewModel voulu et qui sera donc appelée aprés initialisation de celui-çi
            //new object[]{} représente un tableau de paramétres que l'on peut passer à la fonction, comme pour les constructeurs, ici aucun paramètres
            //true indique que l'on souhaite une nouvelle instance du ViewModel ListCommandesPageViewModel
            NavigationService.NavigateTo<ListCommandesPageViewModel>(this, new object[] { ClientCurrent  }, "LoadDatas", new object[] {}, true);
        }

        #endregion
    }
}
