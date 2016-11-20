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
        public List<Reservation> reservationList { get; set; }

        public DirectoryOfReservations()
        {
            reservationList = new List<Reservation>();
            foreach (KeyValuePair<int, Reservation> reservation in getAllReservation())
            {
                reservationList.Add(reservation.Value);
            }
        }

        public Reservation makeNewReservation(int roomID, int userID, string desc, DateTime date)
        {
            Reservation reservation = ReservationMapper.getInstance().makeNew(userID, roomID, desc, date);
            reservationList.Add(reservation);
            return reservation;
        }

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

        public void cancelReservation(int reservationid)
        {
            ReservationMapper.getInstance().delete(reservationid);

            foreach (Reservation reservation in reservationList)
                if (reservation.reservationID == reservationid)
                {
                    reservationList.Remove(reservation);
                    return;
                }
        }

        public Reservation getReservation(int id)
        {
            return ReservationMapper.getInstance().getReservation(id);
        }

        public Dictionary<int, Reservation> getAllReservation()
        {
            return ReservationMapper.getInstance().getAllReservation();
        }

        public void done()
        {
            ReservationMapper.getInstance().done();
        }

        public List<Reservation> findByDate(DateTime date)
        {
            List<Reservation> listByDate = new List<Reservation>();
            foreach (Reservation reservation in reservationList)
            {
                if (reservation.date.Date == date.Date)
                {
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

        public List<Reservation> filterByBlock(DateTime date)
        {
            List<Reservation> listByDate = new List<Reservation>();
            foreach (Reservation reservation in reservationList)
            {
                if (reservation.date == date)
                {
                    listByDate.Add(reservation);
                }
            }
            return listByDate;
        }


    }
}
