using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class DirectoryOfReservations
    {
        public List<Reservation> reservationList { get; set; }

        public DirectoryOfReservations()
        {
            reservationList = new List<Reservation>();
        }
       
        public List<Reservation> findByDate(DateTime date)
        {
            List<Reservation> listByDate = new List<Reservation>();
            foreach(Reservation reservation in reservationList ) {
                if(reservation.reservationDate.Date == date.Date) {
                    listByDate.Add(reservation);
                }
            }
            return listByDate;
        }

        public List<Reservation> findByUser(int userId)
        {
            List<Reservation> listByuserId = new List<Reservation>();
            foreach (Reservation reservation in reservationList)
            {
                if (reservation.reservationID == userId)
                {
                    listByuserId.Add(reservation);
                }
            }
            return listByuserId;
        }

        public List<Reservation> filterByBlock(DateTime date) //for front end, in progress
        {
            List<Reservation> listByDate = new List<Reservation>();
            foreach (Reservation reservation in reservationList)
            {
                if (reservation.reservationDate == date)
                {
                    listByDate.Add(reservation);
                }
            }
            return listByDate;
        }


        /* public DirectoryOfReservations(List<Reservation> reslist)
         {
             this.reservations = reslist;
         } */

        public int getReservationSize()
        {
            return reservationList.Count;
        }

        public Reservation getReservation(int resid)
        {
            for (int i = 0; i < reservationList.Count; i++)
            {
                if (reservationList[i].reservationID == resid)
                {
                    return reservationList[i];
                }
            }
            return null;
        }

        //public void displayReservations()
        //{
        //    //this function should return a string in the future so Nim can use it
        //    string message = "The current reservations made in the system are: ";
           
        //    for (int i = 0; i < reservations.Count; i++)
        //    {

        //        Console.Write(reservations[i].getReservationUser().getFirstName() + " "); //finish this later
        //        Console.Write(reservations[i].getReservationUser().getLastName() + " reserved at: "); //finish this later
        //        Console.WriteLine(reservations[i].getReservationTimeSlot().getTimeSlotID() + ":00 "); //finish this later
                
        //        //also need the room... how to access based on our current class diagram?
        //    }
        //}

        public void makeReservation(int uid, int roomid, int timeslotid, string desc, DateTime dt)
        {
            //  Reservation res = new Reservation(roomid, uid, timeslotid, desc, dt);
            //  string s = "Reservation has been created at " + timeslotid + ":00 in room " + roomid;
            //  return s;
            reservations.Add(new Reservation(roomid, uid, timeslotid, desc, dt));


        }

        public void modifyReservation(int resid, int roomid, int timeslotid, DateTime dt, string des)
        {
            for (int i = 0; i < reservationList.Count; i++)
            {
                if (reservationList[i].reservationID == resid)
                {
                    reservationList[i].reservationID = roomid;
                    reservationList[i].reservationTimeSlotID = timeslotid;
                    reservationList[i].reservationDate = dt;
                    reservationList[i].reservationDescription = des;
                }
            }
        }
            
        public void cancelReservation(int resid)
        {
            for (int i=0; i < reservationList.Count; i++)
            {
                if (reservationList[i].reservationID == resid)
                {
                    reservationList.Remove(reservationList[i]); //remove reservation from list of reservations
                }
            }
        }

        


    }
}
