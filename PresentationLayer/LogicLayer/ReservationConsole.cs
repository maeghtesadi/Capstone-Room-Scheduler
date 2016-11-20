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
        private static DirectoryOfReservations directoryOfReservations;
        private static DirectoryOfRooms directoryOfRooms;
        private static DirectoryOfTimeSlots directoryOfTimeSlots;

        public static void makeReservation(int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            directoryOfReservations = new DirectoryOfReservations();
            directoryOfReservations.makeReservation(uid, roomid, resdes, dt, firstHour, lastHour);
        }

        public static void modifyReservation(int resid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            directoryOfReservations = new DirectoryOfReservations();
            directoryOfReservations.modifyReservation(resid, roomid, resdes, dt, firstHour, lastHour);
        }

        public static void cancelReservation(int resid)
        {
            directoryOfReservations = new DirectoryOfReservations();
            directoryOfReservations.cancelReservation(resid);
        }

        //get up-to-date timeslots from database 
        public static DirectoryOfTimeSlots getAllTimeSlots()
        {
            directoryOfTimeSlots = new DirectoryOfTimeSlots();
            return directoryOfTimeSlots;
        }

        public static DirectoryOfReservations getAllReservations()
        {
            directoryOfReservations = new DirectoryOfReservations();
            return directoryOfReservations;
        }      

        public static DirectoryOfRooms getAllRooms()
        {
            directoryOfRooms = new DirectoryOfRooms();
            return directoryOfRooms;
        }
    }
}
