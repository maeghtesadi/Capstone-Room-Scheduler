using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PresentationLayer.Hubs
{
    public class CalendarHub : Hub
    {
        public void updateCalendar()
        {
            ReservationTest wow= new ReservationTest(10,15,3,"Harambe Tremblay","Soen 343");

            Clients.All.getReservations(wow);
        }
    }
}