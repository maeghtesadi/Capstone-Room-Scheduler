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

        public void makeReservation(int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            Reservation res = ReservationMapper.getInstance().makeNew(uid, roomid, resdes, dt);

            for (int i = firstHour; i < lastHour; i++)
            {
                TimeSlot ts = TimeSlotMapper.getInstance().makeNew(res.reservationUserID, i); //update Later
            }

            UnitOfWork.getInstance().commit();
        }

        public static void modifyReservation(int resid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            Reservation res = ReservationMapper.getInstance().modifyReservation(resid, roomid, resdes, dt);
            for (int i = firstHour; i < lastHour; i++)
            {
                TimeSlot ts = TimeSlotMapper.getInstance().makeNew(res.reservationUserID, i); //update Later
            }
        }

        public static void cancelReservation()
        {

        }

        public static DirectoryOfTimeSlots getAllTimeSlots()
        {
            DirectoryOfTimeSlots directory = new DirectoryOfTimeSlots();
            foreach (KeyValuePair<int, TimeSlot> timeSlot in TimeSlotMapper.getInstance().getAllTimeSlot())
            {
                directory.timeSlotList.Add(timeSlot.Value);
            }
            return directory;
        }

        public static DirectoryOfReservations getAllReservations()
        {
            DirectoryOfReservations reservationDirectory = new DirectoryOfReservations();
            DirectoryOfTimeSlots timeSlotsDirectory = getAllTimeSlots();

            foreach (KeyValuePair<int, Reservation> reservation in ReservationMapper.getInstance().getAllReservation())
            {
                reservationDirectory.reservationList.Add(reservation.Value);
            }

            for (int i = 0; i < reservationDirectory.reservationList.Count; i++)
            {
                for (int j = 0; j < timeSlotsDirectory.timeSlotList.Count; j++)
                {
                    if (reservationDirectory.reservationList[i].reservationID == timeSlotsDirectory.timeSlotList[j].reservationID)
                        reservationDirectory.reservationList[i].timeSlots.Add(timeSlotsDirectory.timeSlotList[j]);
                }
            }

            return reservationDirectory;
        }

        public static DirectoryOfRooms getAllRooms()
        {
            DirectoryOfRooms roomDirectory = new DirectoryOfRooms();
            DirectoryOfReservations reservationDirectory = getAllReservations();

            foreach (KeyValuePair<int, Room> room in RoomMapper.getInstance().getAllRooms())
            {
                roomDirectory.roomList.Add(room.Value);
            }

            for (int i = 0; i < roomDirectory.roomList.Count; i++)
            {
                for (int j = 0; j < reservationDirectory.reservationList.Count; j++)
                {
                    if (reservationDirectory.reservationList[j].reservationRoomID == roomDirectory.roomList[i].roomID)
                        roomDirectory.roomList[i].roomReservations.Add(reservationDirectory.reservationList[j]);
                }
            }


            return roomDirectory;
        }

        public static void addToWaitList(int roomID, int timeSlotID, DateTime date, int userID)
        {

        }

    }
}
