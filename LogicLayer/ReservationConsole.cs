using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace LogicLayer
{
    public class ReservationConsole
    {
        DirectoryOfRooms directory = new DirectoryOfRooms();


        public static void viewReservations()
        {

        }

        public void makeReservation(int userID, int roomID, DateTime date, int timeSlotID, string description)
        {
            directory.makeNewTimeSlot(roomID, date, timeSlotID);
        }

        public static void modifyReservation(int userID, int roomID, int timeSlotID, string description)
        {

        }

        public static void cancelReservation()
        {

        }

        public static void viewRoomAvailabilites()
        {

        }

        public static void addToWaitList(int roomID, int timeSlotID, DateTime date, int userID)
        {

        }

    }
}
