using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapstoneRoomScheduler.LogicLayer.CustomUserManager;
using LogicLayer;
using Microsoft.AspNet.SignalR;
using PresentationLayer.Hubs;


namespace CapstoneRoomScheduler.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Calendar()
        {
            return View();
        }
        [LoggedIn]
        [HttpPost]
        public void acceptTimeSlots(int room,string description,int day,int month,int year,int firstTimeSlot, int lastTimeSlot)
        {
       
            ReservationConsole.getInstance().makeReservation(1,room,description,new DateTime(year,month,day),firstTimeSlot,lastTimeSlot);
            updateCalendar(new DateTime(year, month, day));
        }
        public void updateCalendar(DateTime date)
        {
           var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
           hubContext.Clients.All.updateCalendar(convertToJsonObject(ReservationConsole.getInstance().getAllReservations().findByDate(date)));
        }
        [HttpPost]
        public void getAllUserReservations() {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            var JsonListofReservations = convertToJsonObject(ReservationConsole.getInstance().getAllReservations().findByUser(User.Identity.getUserId()));
            hubContext.Clients.All.getAllReservationsForOneUser(JsonListofReservations); //returns a list of reservations in the js function
        }


        public List<object> convertToJsonObject(List<Reservation> reservationList)
        {
            int firstTimeSlot;
            int lastTimeSlot;
            List<object> list = new List<object>();
            for (int i = 0; i < (reservationList.Count()); i++)
            {
                firstTimeSlot = reservationList[i].timeSlots[0].hour;
                
                lastTimeSlot = reservationList[i].timeSlots[reservationList[i].timeSlots.Count()-1].hour+1;
                list.Add(new
                {
                    initialTimeslot = firstTimeSlot,
                    finalTimeslot = lastTimeSlot,
                    roomId = reservationList[i].roomID,
                    courseName = reservationList[i].description,
                    userName = User.Identity.Name
                });

            }
            return list;
        }




    }
}