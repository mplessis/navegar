namespace Navegar.UWP.Exemple.CRM.POCO
{
    public class Commande : PocoBase
    {
        private string _numeroCommande;
        public string NumeroCommande
        {
            get { return _numeroCommande; }
            set
            {
                _numeroCommande = value;
                RaisePropertyChanged(() => NumeroCommande);
            }
        }

        private double _totalCommand;
        public double TotalCommande
        {
            get { return _totalCommand; }
            set
            {
                _totalCommand = value;
                RaisePropertyChanged(() => TotalCommande);
            }
        }

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
    }
}
