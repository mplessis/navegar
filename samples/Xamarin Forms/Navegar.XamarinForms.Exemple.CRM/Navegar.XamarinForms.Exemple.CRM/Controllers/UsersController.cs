using System.Collections.Generic;
using Navegar.XamarinForms.Exemple.CRM.POCO;

namespace Navegar.XamarinForms.Exemple.CRM.Controllers
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
                },

                new User()
                {
                    UserName = "user1",
                    Password = "pass1",
                }
            };
        }
    }
}
