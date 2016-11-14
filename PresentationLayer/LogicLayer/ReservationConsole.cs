using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using PresentationLayer.Hubs;

namespace LogicLayer
{
    public class ReservationConsole
    {
        DirectoryOfReservations directory = new DirectoryOfReservations();

        //ReservationConsole.updateview() can be called to push new reservaiton data to the client
        public static void updateView(DateTime date)
        {
            DirectoryOfReservations directory = new DirectoryOfReservations();
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            hubContext.Clients.All.getreservations(directory.findByDate(date));
        }

        public void makeReservation(int userID, int roomID, DateTime date, int timeSlotID, string description)
        {
           // directory.makeNewTimeSlot(roomID, date, timeSlotID);
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
