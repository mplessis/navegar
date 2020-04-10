using System.Collections.Generic;
using CommonMobiles.POCO;

namespace CommonMobiles.Controllers
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
