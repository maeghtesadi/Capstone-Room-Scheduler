﻿using System;
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
        }

        public Reservation makeNewReservation(int roomid, int userid, string desc, DateTime date)
        {
            Reservation reservation = ReservationMapper.getInstance().makeNew(userid, roomid, desc, date);
            reservationList.Add(reservation);
            return reservation;
        }

        public void modifyReservation(int reservationid, int roomid, string desc, DateTime date)
        {
            ReservationMapper.getInstance().modifyReservation(reservationid, roomid, desc, date);

            foreach (Reservation reservation in reservationList)
            {
                if (reservation.reservationID == reservationid)
                {
                    reservation.roomID = roomid;
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
            ReservationMapper.getInstance().getAllReservation();
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
