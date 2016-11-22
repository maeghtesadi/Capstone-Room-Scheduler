using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappers;
namespace LogicLayer
{
    public class UserCatalog
    {
        public List<User> registeredUsers { get; set; }

        public UserCatalog()
        {
            registeredUsers = new List<User>();
            foreach (KeyValuePair<int, User> user in UserMapper.getInstance().getAllUser())
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
