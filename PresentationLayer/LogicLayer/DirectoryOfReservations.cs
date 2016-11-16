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


    }
}
