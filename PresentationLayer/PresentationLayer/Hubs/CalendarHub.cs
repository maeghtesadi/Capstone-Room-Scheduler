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
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            hubContext.Clients.All.getReservations();
        }
    }
}