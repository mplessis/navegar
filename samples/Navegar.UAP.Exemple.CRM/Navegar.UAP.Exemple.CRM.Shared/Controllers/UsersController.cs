using System;
using System.Collections.Generic;
using System.Text;
using Navegar.UAP.Exemple.CRM.POCO;

namespace Navegar.UAP.Exemple.CRM.Controllers
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
