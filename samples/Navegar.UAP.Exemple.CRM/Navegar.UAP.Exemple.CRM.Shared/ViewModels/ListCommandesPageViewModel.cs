using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Command;
using Navegar.UAP.Exemple.CRM.Controlers;
using Navegar.UAP.Exemple.CRM.Controllers;
using Navegar.UAP.Exemple.CRM.POCO;

namespace Navegar.UAP.Exemple.CRM.ViewModels
{
    public class ListCommandesPageViewModel : ViewModelServices
    {
        private Client _clientCurrent;

        #region properties

        private List<Commande> _commandes;
        public List<Commande> Commandes
        {
            get { return _commandes; }
            set
            {
                _commandes = value;
                RaisePropertyChanged(() => Commandes);
            }
        }

        private Commande _selectedCommande;
        public Commande SelectedCommande
        {
            get { return _selectedCommande; }
            set
            {
                _selectedCommande = value;
                if (value != null)
                {
                    OpenCommande(value);
                }
                RaisePropertyChanged(() => SelectedCommande);
            }
        }

        public RelayCommand AddCommand { get; set; }
        #endregion

        #region cstor

        public ListCommandesPageViewModel(Client client)
        {
            AddCommand = new RelayCommand(Add);
            _clientCurrent = client;
        }

        #region 

        private void Add()
        {
            var cde = new Commande
            {
                Id = CommandesController.GetNextIdCde(),
                NumeroClient = _clientCurrent != null ? _clientCurrent.NumeroClient : String.Empty
            };
            OpenCommande(cde);
        }

        #endregion


        #endregion

        #region public

        public void LoadDatas()
        {
            Commandes = CommandesController.Initialize().Where(c => c.NumeroClient == _clientCurrent.NumeroClient).ToList();
        }
        #endregion

        #region private

        /// <summary>
        /// Permet de naviguer la page de gestion de la commande
        /// </summary>
        private void OpenCommande(Commande commande)
        {
            //Navigation vers la page CommandePage
            //this sert à indiquer que le ViewModel actuel (et donc par extension la page) sera ajouté à l'historique de navigation, afin que Navegar puisse savoir qu'il doit revenir vers cette page au Back
            //new object[]{commande} permet de passer l'objet client au constructeur du ViewModel CommandePageViewModel
            //true indique que l'on souhaite une nouvelle instance du ViewModel CommandePageViewModel
            NavigationService.NavigateTo<CommandePageViewModel>(this, new object[] { commande }, true);
        }
        #endregion
    }
}
