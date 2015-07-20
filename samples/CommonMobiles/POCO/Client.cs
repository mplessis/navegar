using CommonMobiles.Controllers;

namespace CommonMobiles.POCO
{
    public class Client : PocoBase
    {
        private string _numeroClient;
        public string NumeroClient
        {
            get { return _numeroClient; }
            set
            {
                _numeroClient = value;
                RaisePropertyChanged(() => NumeroClient);
            }
        }

        private string _nom;
        public string Nom
        {
            get { return _nom; }
            set
            {
                _nom = value;
                RaisePropertyChanged(() => Nom);
            }
        }

        private string _ville;
        public string Ville
        {
            get { return _ville; }
            set
            {
                _ville = value;
                RaisePropertyChanged(() => Ville);
            }
        }

        public double ChiffreAffaire
        {
            get { return CommandesController.GetTotalCommandesForClient(this); }
        }
    }
}
