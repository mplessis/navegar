using System.Collections.Generic;
using Navegar.UWP.Exemple.CRM.POCO;

namespace Navegar.UWP.Exemple.CRM.Controllers
{
    public static class UsersController
    {
        public static bool IsConnected { get; set; }

        public static List<User> Initialize()
        {
            return new List<User>
            {
                new User()
                {
                    UserName = "user",
                    Password = "pass",
                }
            };
        }
    }
}
