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

        public static void makeReservation(int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            DirectoryOfReservations directory = new DirectoryOfReservations();
            directory.makeReservation(uid, roomid, resdes, dt, firstHour, lastHour);
        }

        public static void modifyReservation(int resid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            DirectoryOfReservations directory = new DirectoryOfReservations();
            directory.modifyReservation(resid, roomid, resdes, dt, firstHour, lastHour);
        }

        public static void cancelReservation(int resid)
        {
            DirectoryOfReservations directory = new DirectoryOfReservations();
            directory.cancelReservation(resid);
        }

        //get up-to-date timeslots from database 
        public static DirectoryOfTimeSlots getAllTimeSlots()
        {
            DirectoryOfTimeSlots timeSlotDirectory = new DirectoryOfTimeSlots();
            return timeSlotDirectory;
        }

        public static DirectoryOfReservations getAllReservations()
        {
            DirectoryOfReservations reservationDirectory = new DirectoryOfReservations();
            return reservationDirectory;
        }
       

        public static DirectoryOfRooms getAllRooms()
        {
            DirectoryOfRooms roomDirectory = new DirectoryOfRooms();
            return roomDirectory;
        }
    }
}
