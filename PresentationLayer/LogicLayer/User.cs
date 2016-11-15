using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
  
    class User
    {

        public int userID { get; set; }
        public string name { get; set; }
        public int numOfReservations { get; set; }

        public User()
        {
            userID = 0;
            name = "";
            numOfReservations = 0;

        }

        public User (int idnumber, string n, int numres)
        {
            this.userID = idnumber;
            this.name = n;
            this.numOfReservations = numres;

        }

    }

}
