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
    public class ConsoleController : Controller
    {
        public ActionResult Calendar()
        {
            return View();
        }
        [LoggedIn]
        [HttpPost]
        public void acceptTimeSlots(int room,string description,string date,int firstTimeSlot, int lastTimeSlot)
        {
            ReservationConsole.makeReservation(1, room, description, new DateTime(), firstTimeSlot, lastTimeSlot);
           // updateCalendar();
        }
        public void updateCalendar(DateTime date)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            hubContext.Clients.All.updateCalendar();
        }
        //public List<object> convertToJsonObject(List<Reservation> reservationList)
        //{
        //    int firstTimeSlot;
        //    int lastTimeSlot;
        //    List<object> list = new List<object>();
        //    for (int i = 0; i < (reservationList.Count<Reservation>() - 1); i++)
        //    {
        //       firstTimeSlot = reservationList[i].reservationTimeSlotID;
        //        while ((reservationList[i].userID == reservationList[i + 1].userID) && ((reservationList[i].reservationTimeSlotID + 1) == reservationList[i + 1].reservationTimeSlotID))
        //        {
        //            i++;
        //            if (i == reservationList.Count() - 1)
        //            {
        //                break;
        //            }
        //        }
        //        lastTimeSlot = reservationList[i].reservationTimeSlotID;
        //        list.Add(new
        //        {
        //            initialTimeslot = firstTimeSlot,
        //            finalTimeslot = lastTimeSlot,
        //            roomId = reservationList[i - 1].roomID,
        //            courseName = reservationList[i - 1].description,
        //            userName = reservationList[i - 1].userID
        //        });
               
        //    }
        //    return list;
        //}
        



    }
}