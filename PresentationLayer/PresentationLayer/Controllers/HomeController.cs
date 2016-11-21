﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapstoneRoomScheduler.LogicLayer.AuthorizeManager;
using Microsoft.AspNet.Identity;
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
       
            ReservationConsole.getInstance().makeReservation(Int32.Parse(User.Identity.GetUserId()),room,description,new DateTime(year,month,day),firstTimeSlot,lastTimeSlot);
            updateCalendar(new DateTime(year, month, day));
        }
        public void updateCalendar(DateTime date)
        {
           var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
           hubContext.Clients.All.updateCalendar(convertToJsonObject(ReservationConsole.getInstance().getAllReservations().findByDate(date)));
        }
        [HttpPost]
        public void getReservations() {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CalendarHub>();
            var JsonListofReservations = convertToJsonObject(ReservationConsole.getInstance().getAllReservations().findByUser(Int32.Parse(User.Identity.GetUserId())));
            hubContext.Clients.All.populateReservations(JsonListofReservations); //returns a list of reservations in the js function
        }

        //Techiincally asp/signarl autmatically converts to json when you pass an object to javascript but here we just convert it into an easy to digest object
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
                    userName =  ReservationConsole.getInstance().getUserCatalog().registeredUsers.First(x => x.userID == reservationList[i].userID).name,
                    reservationId = reservationList[i].reservationID

                });

            }
            return list;
        }




    }
}