using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class UserCatalog
    {
        private List<int> registeredUsers;

        public UserCatalog()
        {
            registeredUsers = new List<int>();
        }

        public UserCatalog(List<int> regusers)
        {
            this.registeredUsers = regusers;
        }

    }
}
