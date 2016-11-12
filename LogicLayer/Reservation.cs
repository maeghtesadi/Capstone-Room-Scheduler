using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class Reservation
    {
        private User reservationUser;
        private TimeSlot reservationTimeSlot;
        int reservationUserID;
        int reservationTimeSlotID;

        private int reservationID;
        private string reservationDescription;
        private List<int> waitList;

        public Reservation()
        {
            reservationID = 0;
            reservationDescription = "";
            reservationTimeSlotID = 0;
            reservationUserID = 0;
            waitList = new List<int>();
        }

        public Reservation(int resid, int uid, int tid, string resdes)
        {
            this.reservationID = resid;
            this.reservationDescription = resdes;
            this.reservationTimeSlotID = tid;
            this.reservationUserID = uid;
            waitList = new List<int>();
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
