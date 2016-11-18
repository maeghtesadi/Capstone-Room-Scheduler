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
            DirectoryOfReservations directory = new LogicLayer.DirectoryOfReservations();
            for (int i = firstTimeSlot; i <= lastTimeSlot; i ++)
            {
                directory.makeReservation(123, room, i, date, new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, i, 0, 0));
            }
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            hubContext.Clients.All.getReservations(convertToJsonObject(directory.findByDate(DateTime.Today)));
            //updateView();
        }

        public void updateView(DateTime date)
        {
            DirectoryOfReservations directory = new DirectoryOfReservations();
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            hubContext.Clients.All.getreservations(directory.findByDate(date));
        }
        public List<object> convertToJsonObject(List<Reservation> reservationList)
        {
            int firstTimeSlot;
            int lastTimeSlot;
            List<object> list = new List<object>();
            for (int i = 0; i < (reservationList.Count<Reservation>() - 1); i++)
            {
               firstTimeSlot = reservationList[i].reservationTimeSlotID;
                while ((reservationList[i].userID == reservationList[i + 1].userID) && ((reservationList[i].reservationTimeSlotID + 1) == reservationList[i + 1].reservationTimeSlotID))
                {
                    i++;
                    if (i == reservationList.Count() - 1)
                    {
                        break;
                    }
                }
                lastTimeSlot = reservationList[i].reservationTimeSlotID;
                list.Add(new
                {
                    initialTimeslot = firstTimeSlot,
                    finalTimeslot = lastTimeSlot,
                    roomId = reservationList[i - 1].roomID,
                    courseName = reservationList[i - 1].description,
                    userName = reservationList[i - 1].userID
                });
               
            }
            return list;
        }
        


        public void updateView()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
           // hubContext.Clients.All.getreservations(new ReservationTest(10, 15, 3, "Harambe Tremblay", "Soen 343"));

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