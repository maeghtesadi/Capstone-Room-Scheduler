using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    //
    class User
    {
        private int userID;
        private string firstName;
        private string lastName;
        private int numOfReservations;

        public User()
        {
            userID = 0;
            firstName = "";
            lastName = "";
            numOfReservations = 0;
        }

        public User (int idnumber, string fname, string lname, int numres)
        {
            this.userID = idnumber;
            this.firstName = fname;
            this.lastName = lname;
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

        public string getFirstName()
        {
            return firstName;
        }

        public void setFirstName(string fname)
        {
            this.firstName = lastName; 
        }

        public string getLastName()
        {
            return lastName;
        }

        public void setLastName(string lname)
        {
            this.lastName = lname;
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
