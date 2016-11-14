using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using Microsoft.AspNet.SignalR;
using PresentationLayer.Hubs;


namespace CapstoneRoomScheduler.Controllers
{
    public class ConsoleController : Controller
    {
        public ActionResult Calendar()
        {
            return View();
        }

        [HttpPost]
        public void acceptTimeSlots(string inputCourseName,int firstTimeSlot, int lastTimeSlot, int room, string date)
        {
            List<ReservationTest> list = new List<ReservationTest>();
            list.Add(new ReservationTest(10, 15, 3, "Harambe Tremblay", "Soen 343"));
            list.Add(new ReservationTest(8 ,10, 5, "Harambe Tremblay", "Soen 343"));
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            hubContext.Clients.All.getReservations(list);
        }

        public void updateView()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            hubContext.Clients.All.getreservations(new ReservationTest(10, 15, 3, "Harambe Tremblay", "Soen 343"));

        }
        public ActionResult About()
        {
            ViewBag.Message = "This is Harambook";

            return View();
        }

        public ActionResult Reservations()
        {      
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Harambe's Home :'(";

            return View();
        }
    }
}