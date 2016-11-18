using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class Reservation
    {
        public int userID { get; set; }
        public int roomID { get; set; }
        public DateTime date { get; set; }
        public int reservationID { get; set; }
        public string description { get; set; }
        public List<TimeSlot> timeSlots { get; set; }

        public Reservation()
        {
            reservationID = 0;
            description = "";
            userID = 0;
            roomID = 0;
            date = new DateTime(); //default constructor will set the date of the reservation as the current day
            timeSlots = new List<TimeSlot>(); 
        }

        public Reservation(int resid, int uid, int roomid, string resdes, DateTime dt, List<TimeSlot> timeslotlist)
        {
            this.reservationID = resid;
            this.description = resdes;
            this.userID = uid;
            this.roomID = roomid;
            this.date = dt;
            this.timeSlots = timeslotlist;
        }

    }
}
