using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class UserCatalog
    {
        public List<User> registeredUsers { get; set; }

        public UserCatalog()
        {
            registeredUsers = new List<User>();
            foreach (KeyValuePair<int, User> user in getAllUser())
            {
                registeredUsers.Add(user.Value);
            }
        }

        public UserCatalog(List<User> regusers)
        {
            this.registeredUsers = regusers;
        }

    }
}
