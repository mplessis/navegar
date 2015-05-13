using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using Navegar.UWP.Exemple.CRM.Controllers;
using Navegar.UWP.Exemple.CRM.POCO;

namespace Navegar.UWP.Exemple.CRM.ViewModel
{
    public class ListClientsPageViewModel : ViewModelServices
    {
        #region properties

        private List<Client> _clients;
        public List<Client> Clients
        {
            get { return _clients; }
            set
            {
                _clients = value;
                RaisePropertyChanged(() => Clients);
            }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                if (value != null)
                {
                    OpenClient(value);
                }
                RaisePropertyChanged(() => SelectedClient);
            }
        }

        public RelayCommand DisconnectCommand { get; set; }
        #endregion

        #region cstor

        public ListClientsPageViewModel()
        {
            CommandesController.Initialize();
            Clients = ClientsController.Initialize();
            DisconnectCommand = new RelayCommand(Disconnect);
        }

        #endregion

        #region relaycommand

        /// <summary>
        /// Navigue vers la page de login
        /// </summary>
        private void Disconnect()
        {
            //Permet de vide l'historique de navigation est ainsi ne pas avoir de retour possible
            NavigationService.Clear();

            //Retourne vers la page de login sans historiser la page actuelle et comme l'on a vidé la navigation, si l'utilisateur appuie sur la touche de retour, l'application va se fermer
            //en Windows Phone
            NavigationService.NavigateTo<LandingPageViewModel>(true);
        }

        #endregion

        #region private

        /// <summary>
        /// Permet de naviguer la page de gestion du client
        /// </summary>
        private void OpenClient(Client client)
        {
            //Permet de tester la fonction de Pre-navigation, comme l'utilisateur est marqué déconnecté
            //La navigation va retourner à la page de login
            //Voir les fonctions définies pour cela dans le App.xaml.cs
            //UsersController.IsConnected = false;

            //Navigation vers la page ClientPage
            //this sert à indiquer que le ViewModel actuel (et donc par extension la page) sera ajouté à l'historique de navigation, afin que Navegar puisse savoir qu'il doit revenir vers cette page au Back
            //new object[]{client} permet de passer l'objet client au constructeur du ViewModel ClientPageViewModel
            //true indique que l'on souhaite une nouvelle instance du ViewModel ClientPageViewModel
            NavigationService.NavigateTo<ClientPageViewModel>(this, new object[] {client}, true);
        }
        #endregion
    }
}
