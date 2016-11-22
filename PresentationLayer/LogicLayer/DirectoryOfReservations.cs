using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class DirectoryOfReservations
    {
        private static DirectoryOfReservations instance = new DirectoryOfReservations();
        
        public List<Reservation> reservationList { get; set; }

        // Constructor
        private DirectoryOfReservations()
        {
            reservationList = new List<Reservation>();
        }

        // Get the instance object
        public static DirectoryOfReservations getInstance()
        {
            return instance;
        }

        // Method to make a new reservation
        public Reservation makeNewReservation(int reservationid, int userID, int roomID, string desc, DateTime date)
        {
            Reservation reservation = new Reservation(reservationid, userID, roomID, desc, date);
            reservationList.Add(reservation);
            return reservation;
        }

        // Method to modify a reservation
        public void modifyReservation(int reservationID, int roomID, string desc, DateTime date)
        {
            foreach (Reservation reservation in reservationList)
            {
                if (reservation.reservationID == reservationID)
                {
                    reservation.roomID = roomID;
                    reservation.description = desc;
                    reservation.date = date;
                }
            }
        }

        // Method to cancel a reservation
        public void cancelReservation(int reservationID)
        {
            foreach (Reservation reservation in this.reservationList)
                if (reservation.reservationID == reservationID)
                {
                    reservationList.Remove(reservation);
                    return;
                }
        }

    //    public List<Reservation> findByDate(DateTime date)
    //    {
    //        List<Reservation> listByDate = new List<Reservation>();
    //        foreach (Reservation reservation in reservationList)
    //        {
    //            if (reservation.date.Date == date.Date)
    //            {
    //                listByDate.Add(reservation);
    //            }
    //        }
    //        return listByDate;
    //    }

    //    public List<Reservation> findByUser(int userID)
    //    {
    //        List<Reservation> listByuserId = new List<Reservation>();
    //        foreach (Reservation reservation in reservationList)
    //        {
    //            if (reservation.userID == userID)
    //            {
    //                listByuserId.Add(reservation);
    //            }
    //        }
    //        return listByuserId;
    //    }
   

    //public List<Reservation> filterByBlock(DateTime date)
    //    {
    //        List<Reservation> listByDate = new List<Reservation>();
    //        foreach (Reservation reservation in reservationList)
    //        {
    //            if (reservation.date == date)
    //            {
    //                listByDate.Add(reservation);
    //            }
    //        }
    //        return listByDate;
    //    }

    //    public Boolean check4HourConstraint(int userID, DateTime date, int interval)
    //    {
    //        if ((TimeSlotMapper.getInstance().findHoursByReservationID(ReservationMapper.getInstance().findReservationID(userID, date)) + interval) < 4)
    //        {
    //            return true;
    //        }
    //        else
    //            return false;
    //    }

    }
}
