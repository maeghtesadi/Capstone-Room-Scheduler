using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappers;

namespace LogicLayer
{
    public class DirectoryOfReservations
    {
   
        private static DirectoryOfReservations instance = new DirectoryOfReservations();
        
        public List<Reservation> reservationList { get; set; }

        public DirectoryOfReservations()
        {
            reservationList = new List<Reservation>();
            foreach (KeyValuePair<int, Reservation> reservation in getAllReservation())
            {
                reservationList.Add(reservation.Value);
            }
        }

        public static DirectoryOfReservations getInstance()
        {
            return instance;
        }

        public void addToListOfReservations(Reservation r)
        {
            reservationList.Add(r);
        }

        public void deleteFromListOfReservations(int resid)
        {
            for (int i = 0; i < reservationList.Count; i++)
            {
                if (reservationList[i].reservationID == resid)
                {
                    reservationList.Remove(reservationList[i]);
                    break;
                }
            }
        }
        
        public Reservation makeNewReservation(int roomID, int userID, string desc, DateTime date)
        {
            Reservation reservation = ReservationMapper.getInstance().makeNew(userID, roomID, desc, date);
            reservationList.Add(reservation);
            return reservation;
        }
        //we do NOT need these anymore.

        public void modifyReservation(int reservationID, int roomID, string desc, DateTime date)
        {
            ReservationMapper.getInstance().modifyReservation(reservationID, roomID, desc, date);

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

        public void cancelReservation(int reservationID)
        {
            //ReservationMapper.getInstance().delete(reservationID);

            foreach (Reservation reservation in reservationList)
                if (reservation.reservationID == reservationID)
                {
                    reservationList.Remove(reservation);
                    return;
                }
        }

        public Reservation getReservation(int reservationID)
        {
            return ReservationMapper.getInstance().getReservation(reservationID);
        }

        public Dictionary<int, Reservation> getAllReservation()
        {
            return ReservationMapper.getInstance().getAllReservation();
        }

        public void done()
        {
            ReservationMapper.getInstance().done();
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
