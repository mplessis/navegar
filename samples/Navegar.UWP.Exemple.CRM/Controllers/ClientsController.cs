using System.Collections.Generic;
using System.Linq;
using Navegar.UWP.Exemple.CRM.POCO;

namespace Navegar.UWP.Exemple.CRM.Controllers
{
    public static class ClientsController
    {
        private static List<Client> _clients; 
        public static List<Client> Initialize()
        {
            return _clients ?? (_clients = new List<Client>
            {
                new Client {Id = 1, Nom = "Client 01", Ville = "Limoges", NumeroClient = "Clt01"},
                new Client {Id = 2, Nom = "Client 02", Ville = "Toulouse", NumeroClient = "Clt02"},
                new Client {Id = 3, Nom = "Client 03", Ville = "Lille", NumeroClient = "Clt03"},
                new Client {Id = 4, Nom = "Client 04", Ville = "Paris", NumeroClient = "Clt04"}
            });
        }

        public static void Save(Client client)
        {
            if (_clients!= null && _clients.Any())
            {
                var clts = _clients.Where(c => c.Id == client.Id);
                if (clts.Any())
                {
                    clts.First().Nom = client.Nom;
                    clts.First().Ville = client.Ville;
                    clts.First().NumeroClient = client.NumeroClient;
                }
            }
        }
    }
}
