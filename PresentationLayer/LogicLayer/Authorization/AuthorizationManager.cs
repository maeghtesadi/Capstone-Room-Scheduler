using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using PresentationLayer.Hubs;

namespace CapstoneRoomScheduler.LogicLayer.AuthorizeManager
{
   
    public class LoggedIn : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated)
            { 
                var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
                hubContext.Clients.All.notLoggedIn();
                return false;
            }
            return base.AuthorizeCore(httpContext);
        }
    }
}