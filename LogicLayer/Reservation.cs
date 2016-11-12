using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class Reservation
    {
        public static int uniqueReservationCounter = 1; //want a global variable that is incremented 
        //each time a reservation is created

        private User reservationUser;
        private TimeSlot reservationTimeSlot;
        private int reservationUserID;
        private int reservationTimeSlotID;

        private int reservationRoomID;
        private int reservationID;
        private string reservationDescription;
        private List<int> waitList;

        public Reservation()
        {
            reservationID = uniqueReservationCounter;
            reservationDescription = "";
            reservationTimeSlotID = 0;
            reservationUserID = 0;
            reservationRoomID = 0;
            waitList = new List<int>();

            uniqueReservationCounter++;
        }

        public Reservation(int roomid, int uid, int tid, string resdes)
        {
            this.reservationID = uniqueReservationCounter;
            this.reservationDescription = resdes;
            this.reservationTimeSlotID = tid;
            this.reservationUserID = uid;
            reservationRoomID = roomid;
            waitList = new List<int>();

            uniqueReservationCounter++; //counter goes up for every reservation created
        }

        public User getReservationUser()
        {
            return reservationUser;
        }

        public void setReservationUser(User u)
        {
            this.reservationUser = u;
        }

        public TimeSlot getReservationTimeSlot()
        {
            return reservationTimeSlot;
        }

        public void setReservationTimeSlot(TimeSlot t)
        {
            this.reservationTimeSlot = t;
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
            this.waitList.Add(uid);
        }

    }
}
