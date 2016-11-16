using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class Reservation
    {
        public static int uniqueReservationCounter = 1; //want a global variable that is incremented 
        //each time a reservation is created
        //THIS MUST BE CHANGED WHEN MERGED WITH MAPPER CODE, we need to hash for reservation id

        //private User reservationUser;
        //private TimeSlot reservationTimeSlot;
        public int reservationUserID { get; set; }
        public int reservationTimeSlotID { get; set; }
        public int reservationRoomID { get; set; }
        public DateTime reservationDate { get; set; }

        public int reservationID { get; set; }
        public  string reservationDescription { get; set; }
        public Queue<int> waitList { get; set; }

        public Reservation()
        {
            reservationID = uniqueReservationCounter;
            reservationDescription = "";
            reservationTimeSlotID = 0;
            reservationUserID = 0;
            reservationRoomID = 0;
            reservationDate = new DateTime(); //default constructor will set the date of the reservation as the current day
            waitList = new Queue<int>();

            uniqueReservationCounter++;
        }

        public Reservation(int roomid, int uid, int tid, string resdes, DateTime dt)
        {
            this.reservationID = uniqueReservationCounter;
            this.reservationDescription = resdes;
            this.reservationTimeSlotID = tid;
            this.reservationUserID = uid;
            this.reservationRoomID = roomid;
            this.reservationDate = dt;
            waitList = new Queue<int>();

            uniqueReservationCounter++; //counter goes up for every reservation created
        }

        //public User getReservationUser()
        //{
        //    return reservationUser;
        //}

        //public void setReservationUser(User u)
        //{
        //    this.reservationUser = u;
        //}

        //public TimeSlot getReservationTimeSlot()
        //{
        //    return reservationTimeSlot;
        //}

        //public void setReservationTimeSlot(TimeSlot t)
        //{
        //    this.reservationTimeSlot = t;
        //}

        public void setReservationDateTime(int year, int month, int day, int hour)
        {
            reservationDate = new DateTime(year, month, day, hour, 0, 0);
        }

        //can't set each field seperately


        public void addToWaitList(int uid)
        {
            //add to wait list
            waitList.Enqueue(uid);
        }
        
        public void removeFromWaitList()
        {
            waitList.Dequeue();
        }

    }
}
