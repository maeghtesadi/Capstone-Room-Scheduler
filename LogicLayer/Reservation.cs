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
        private int reservationUserID;
        private int reservationTimeSlotID;
        private int reservationRoomID;
        private DateTime date;

        private int reservationID;
        private string reservationDescription;
        private Queue<int> waitList;

        public Reservation()
        {
            reservationID = uniqueReservationCounter;
            reservationDescription = "";
            reservationTimeSlotID = 0;
            reservationUserID = 0;
            reservationRoomID = 0;
            date = new DateTime(); //default constructor will set the date of the reservation as the current day
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
            this.date = dt;
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

        public int getReservationUserID()
        {
            return reservationUserID;
        }

        public void setReservationUserID(int uid)
        {
            this.reservationUserID = uid;
        }

        public int getReservationTimeSlotID()
        {
            return reservationTimeSlotID;
        }

        public void setReservationTimeSlotID(int tid)
        {
            this.reservationTimeSlotID = tid;
        }

        public DateTime getReservationDateTime()
        {
            return date;
        }

        public void setReservationDateTime(int year, int month, int day, int hour)
        {
            date = new DateTime(year, month, day, hour, 0, 0);
        }

        public int getReservationRoomID()
        {
            return reservationRoomID;
        }

        public void setReservationRoomID(int roomid)
        {
            this.reservationRoomID = roomid;
        }

        //can't set each field seperately

        public int getReservationYear()
        {
            return date.Year;
        }

        public int getReservationMonth()
        {
            return date.Month;
        }

        public int getReservationDay()
        {
            return date.Day;
        }

        public int getReservationHour()
        {
            return date.Hour;
        }

        public int getReservationID()
        {
            return reservationID;
        }

        public void setReservationID(int resid)
        {
            this.reservationID = resid;
        }
        
        public string getReservationDescription()
        {
            return reservationDescription;
        }

        public void setReservationDescription(string desc)
        {
            this.reservationDescription = desc;
        }

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
