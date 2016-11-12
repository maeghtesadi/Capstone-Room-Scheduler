using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PresentationLayer.Hubs
{
    public class CalendarHub : Hub
    {
        public void updateCalendar(string name, string message)
        {
            Clients.All.getReservations(name, message);
        }
    }
}