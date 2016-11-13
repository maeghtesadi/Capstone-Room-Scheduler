using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
  
    class User
    {

        private int userID;
        private string name;
        private int numOfReservations;

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

        public int getUserID()
        {
            return userID;
        }

        public void setUserID(int idnumber)
        {
            this.userID = idnumber;
        }

        public string getName()
        {
            return name;
        }

        public void setName(string n)
        {
            this.name = n; 
        }

        public int getNumOfReservations ()
        {
            return numOfReservations;
        }

        public void setNumOfReservations (int numres)
        {
            this.numOfReservations = numres;
        }

    }

}
