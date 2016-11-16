using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class Reservation
    {
        public int reservationUserID { get; set; }
        public int reservationRoomID { get; set; }
        public DateTime reservationDate { get; set; }
        public int reservationID { get; set; }
        public string reservationDescription { get; set; }
        public List<TimeSlot> timeSlots { get; set; }

        public Reservation()
        {
            reservationID = 0;
            reservationDescription = "";
            reservationUserID = 0;
            reservationRoomID = 0;
            reservationDate = new DateTime(); //default constructor will set the date of the reservation as the current day
            timeSlots = new List<TimeSlot>(); 
        }

        public Reservation(int resid, int uid, int roomid, string resdes, DateTime dt, List<TimeSlot> timeslotlist)
        {
            this.reservationID = resid;
            this.reservationDescription = resdes;
            this.reservationUserID = uid;
            this.reservationRoomID = roomid;
            this.reservationDate = dt;
            this.timeSlots = timeslotlist;
        }

    }
}
