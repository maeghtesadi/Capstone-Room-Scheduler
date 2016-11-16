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

        public void makeReservation(int userID, int roomID, DateTime date, string description)
        {
            Reservation res = new Reservation(roomID, userID, timeSlotID, description, date);
            ReservationMapper.getInstance().addReservation()
           // directory.makeNewTimeSlot(roomID, date, timeSlotID);
        }

        public static void modifyReservation(int userID, int roomID, string description)
        {

        }

        public static void cancelReservation()
        {

        }

        public static DirectoryOfReservations getAllReservations()
        {
            DirectoryOfReservations directory = new DirectoryOfReservations();
            foreach (KeyValuePair<int, Reservation> reservation in RoomMapper.getInstance().getAllRooms())
            {
                directory.reservationList.Add
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
