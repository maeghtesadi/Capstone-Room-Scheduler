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

        private static UserCatalog instance = new UserCatalog();

        public List<User> registeredUsers { get; set; }

        private UserCatalog()
        {
            registeredUsers = new List<User>();
        }

        public UserCatalog(List<User> regusers)
        {
            this.registeredUsers = regusers;
        }

        public static UserCatalog getInstance()
        {
            return instance;
        }

        public User makeNewUser(int userID, String username, String password, String name)
        {
            User user = new User(userID, username, password, name);
            return user;
        }

        public User modifyUser(int userID, String name)
        {
            for(int i = 0; i < registeredUsers.Count; i++)
            {
                if (registeredUsers[i].userID == userID)
                {
                    registeredUsers[i].userID = userID;
                    registeredUsers[i].name = name;
                    return registeredUsers[i];
                }
            }
            return null;
        }
    }
}
