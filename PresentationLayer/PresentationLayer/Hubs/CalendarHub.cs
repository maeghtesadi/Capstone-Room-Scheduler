using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PresentationLayer.Hubs
{
    public class CalendarHub : Hub
    {

        public void updateView()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            hubContext.Clients.All.getReservations();

        }

        //This method is only called from the js file
        public void updateCalendar()
        {
            //ReservationTest wow= new ReservationTest(10,15,3,"Harambe Tremblay","Soen 343");

            //List<ReservationTest> reservationList = new List<ReservationTest>();
            //reservationList.Add(new ReservationTest(9, 11, 1, "Nassim", "343"));
            //reservationList.Add(new ReservationTest(11, 14, 2, "Nassim", "343"));
            //reservationList.Add(new ReservationTest(13, 15, 3, "Nassim", "343"));

            ////This calls a method inside js 
            //Clients.All.getReservations(reservationList);
        }
    }
}