using System.Collections.Generic;
using System.Linq;
using Navegar.XamarinForms.Exemple.CRM.POCO;

namespace Navegar.XamarinForms.Exemple.CRM.Controllers
{
    public static class CommandesController
    {
        private static List<Commande> _commandes; 
        public static List<Commande> Initialize()
        {
            return _commandes ?? (_commandes = new List<Commande>
            {
                new Commande
                {
                    Id = 1,
                    NumeroCommande = "Cde01",
                    NumeroClient = "Clt01",
                    TotalCommande = 123.56,
                },
                new Commande
                {
                    Id = 2,
                    NumeroCommande = "Cde02",
                    NumeroClient = "Clt01",
                    TotalCommande = 345.78
                },
                new Commande
                {
                    Id = 3,
                    NumeroCommande = "Cde03",
                    NumeroClient = "Clt03",
                    TotalCommande = 23.56
                },
            });
        }

        public static double GetTotalCommandesForClient(Client client)
        {
            if (_commandes != null && _commandes.Any())
            {
                var cdes = _commandes.Where(c => c.NumeroClient == client.NumeroClient);
                if (cdes.Any())
                {
                    return cdes.Sum(c => c.TotalCommande);
                }
            }
            return 0;
        }

        public static void Save(Commande commande)
        {
            if (_commandes != null && _commandes.Any())
            {
                var cdes = _commandes.Where(c => c.Id == commande.Id);
                if (cdes.Any())
                {
                    var cde = cdes.First();
                    cde.NumeroClient = commande.NumeroClient;
                    cde.NumeroCommande = commande.NumeroCommande;
                    cde.TotalCommande = commande.TotalCommande;
                }
                else
                {
                    _commandes.Add(commande);
                }
            }
        }

        public static int GetNextIdCde()
        {
            if (_commandes != null && _commandes.Any())
            {
                return _commandes.Select(c => c.Id).Max() + 1;
            }
            return 1;
        }
    }
}
