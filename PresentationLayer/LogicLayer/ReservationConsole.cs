using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using PresentationLayer.Hubs;
using Mappers;

namespace LogicLayer
{
    public class ReservationConsole
    {

        public void makeReservation(int resid, int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            List<Reservation> newReservation = new List<Reservation>();
            for (int i = firstHour; i < lastHour; i++)
            {
                //Reservation res = new Reservation(resid, uid, roomid, resdes, dt, i);
                Reservation res = ReservationMapper.getInstance().makeNew(uid, roomid, resdes, dt, i);
                newReservation.Add(res);
            }
            UnitOfWork.getInstance().commit();
        }

        public static void modifyReservation(int resid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            List<Reservation> reservationBlock = new List<Reservation>();
            reservationBlock = ReservationMapper.getInstance().getReservationBlock(blockID);

            if (lastHour - firstHour > reservationBlock.Count)
            {
                for (int i = lastHour - firstHour; i > 0; i--)
                    reservationBlock.Add(new Reservation());
            }

            if (lastHour - firstHour < reservationBlock.Count)
            {
                for (int i = 0; i < lastHour - firstHour - reservationBlock.Count; i++)
                    reservationBlock.RemoveAt(i);
            }

            ReservationMapper.getInstance().modifyReservation(resid, roomid, resdes, dt, hour); //blockid later
        }

        public static void cancelReservation()
        {

        }

        public static DirectoryOfReservations getAllReservations()
        {
            DirectoryOfReservations directory = new DirectoryOfReservations();
            foreach (KeyValuePair<int, Reservation> reservation in RoomMapper.getInstance().getAllRooms())
            {
                directory.reservationList.Add();
            }
        }

        public static DirectoryOfRooms getAllRooms()
        {
            DirectoryOfRooms directory = new DirectoryOfRooms();
            foreach(KeyValuePair<int, Room> room in RoomMapper.getInstance().getAllRooms())
            {
                directory.roomList.Add(room.Value);
            }
            return directory;
        }

        public static void addToWaitList(int roomID, int timeSlotID, DateTime date, int userID)
        {

        }

    }
}
