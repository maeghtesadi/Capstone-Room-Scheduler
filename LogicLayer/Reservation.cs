using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class Reservation
    {
        int reservationID;
        string reservationDescription;
        List<int> waitList;

        public Reservation()
        {
            reservationID = 0;
            reservationDescription = "";
            waitList = new List<int>();
        }
    }
}
