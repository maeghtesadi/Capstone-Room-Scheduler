using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class DirectoryOfReservations
    {
        private List<Reservation> reservations;

        public DirectoryOfReservations()
        {
            reservations = new List<Reservation>();
        }

        public DirectoryOfReservations(List<Reservation> reslist)
        {
            this.reservations = reslist;
        }

        public int getReservationSize()
        {
            return reservations.Count;
        }

        public Reservation getReservation(int resid)
        {
            for (int i = 0; i < reservations.Count; i++)
            {
                if (reservations[i].getReservationID() == resid)
                {
                    return reservations[i];
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

        public string makeReservation(int uid, int roomid, int timeslotid, string desc, DateTime dt)
        {
            Reservation res = new Reservation(roomid, uid, timeslotid, desc, dt);
            string s = "Reservation has been created at " + timeslotid + ":00 in room " + roomid;
            return s;
        }

        public void modifyReservation(int resid, int roomid, int timeslotid, DateTime dt)
        {
            for (int i = 0; i < reservations.Count; i++)
            {
                if (reservations[i].getReservationID() == resid)
                {
                    reservations[i].setReservationRoomID(roomid);
                    reservations[i].setReservationTimeSlotID(timeslotid);
                    reservations[i].setReservationDateTime(dt.Year, dt.Month, dt.Day, dt.Hour);
                }
            }



        }
            


    }
}
