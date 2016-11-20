using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using PresentationLayer.Hubs;
using Mappers;

namespace LogicLayer
{
    public class ReservationConsole
    {

        DirectoryOfReservations directoryOfReservations = new DirectoryOfReservations();
        DirectoryOfRooms directoryOfRooms = new DirectoryOfRooms();
        DirectoryOfTimeSlots directoryOfTimeSlots = new DirectoryOfTimeSlots();

        public ReservationConsole()
        {
            udpateDirectories();
        }

        public void makeReservation(int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            Reservation res = new Reservation();
            List<int> hours = new List<int>();
            for (int i = firstHour; i < lastHour; i++)
                hours.Add(i);

            foreach (Reservation reservation in directoryOfReservations.reservationList)
            {
                // Compare if the date (not the time portion) are the same and the rooms are the same
                if (reservation.date.Date == dt.Date && reservation.roomID == roomid)
                {
                    foreach (TimeSlot timeSlot in reservation.timeSlots)
                    {
                        for (int i = firstHour; i < lastHour; i++)
                        {
                            if (timeSlot.hour == i)
                            {
                                if (!timeSlot.waitlist.Contains(uid) && reservation.userID != uid)
                                {
                                    timeSlot.waitlist.Enqueue(uid);
                                    directoryOfTimeSlots.addToWaitList(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
                                }
                                hours.Remove(i);
                            }
                        }
                    }
                }
            }

            if (hours.Count > 0)
            {
                res = directoryOfReservations.makeNewReservation(roomid, uid, resdes, dt);
                for (int i = 0; i < hours.Count; i++)
                {
                    directoryOfTimeSlots.makeNewTimeSlot(res.reservationID, hours[i]);
                    updateWaitList(uid, dt, i);
                    directoryOfTimeSlots.done();
                }
            }

            directoryOfTimeSlots.done();
            directoryOfReservations.done();
            udpateDirectories();
        }

        public void udpateDirectories()
        {
            // Only Console has visibility over DirectoryOfTimeSlot, so this was loop was put here instead of DirectoryOfReservation
            // Updating timeSlots of each reservations
            for (int i = 0; i < (directoryOfReservations.reservationList).Count; i++)
            {
                foreach (KeyValuePair<int, TimeSlot> timeSlot in directoryOfTimeSlots.getAllTimeSlot())
                {
                    if (directoryOfReservations.reservationList[i].reservationID == timeSlot.Value.reservationID)
                        directoryOfReservations.reservationList[i].timeSlots.Add(timeSlot.Value);
                }
            }

            // Updating the waitList of each timeSlot
            for (int i = 0; i < directoryOfTimeSlots.timeSlotList.Count; i++)
            {
                List<int> waitList = directoryOfTimeSlots.getAllUsers(directoryOfTimeSlots.timeSlotList[i].timeSlotID);
                if (waitList != null)
                    for (int j = 0; j < waitList.Count; j++)
                        directoryOfTimeSlots.timeSlotList[i].waitlist.Enqueue(waitList[j]);
            }

            //Updating the reservations for each room
            for (int i = 0; i < directoryOfRooms.roomList.Count; i++)
            {
                foreach (KeyValuePair<int, Reservation> reservation in directoryOfReservations.getAllReservation())
                {
                    if (reservation.Value.roomID == directoryOfRooms.roomList[i].roomID)
                        directoryOfRooms.roomList[i].roomReservations.Add(reservation.Value);
                }
            }
        }

        public void updateWaitList(int userid, DateTime date, int hour)
        {
            foreach (TimeSlot timeSlot in directoryOfTimeSlots.timeSlotList)
            {
                // Obtain the date associated with that timeslot for the current reservation
                DateTime timeSlotDate = directoryOfReservations.getReservation(timeSlot.reservationID).date;

                // We only want to remove the user from the waitlist of timeslots of the same date and hour 
                if (timeSlot.waitlist.Contains(userid) && timeSlotDate.Equals(date) && timeSlot.hour == hour)
                {
                    Queue<int> newQueue = new Queue<int>();
                    int size = timeSlot.waitlist.Count;
                    for (int i = 0; i < size; i++)
                    {
                        if (timeSlot.waitlist.Peek() == userid)
                        {
                            timeSlot.waitlist.Dequeue();
                        }
                        else
                        {
                            newQueue.Enqueue(timeSlot.waitlist.Dequeue());
                        }
                    }
                    directoryOfTimeSlots.addToWaitList(timeSlot.timeSlotID, timeSlot.reservationID, newQueue);
                }

            }
            directoryOfTimeSlots.done();
        }

        public void modifyReservation(int resid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            Reservation resToModify = new Reservation();
            for (int i = 0; i < directoryOfReservations.reservationList.Count; i++)
            {
                if (resid == directoryOfReservations.reservationList[i].reservationID)
                    resToModify = directoryOfReservations.reservationList[i];
            }

            if (resToModify.date.Date != dt.Date || resToModify.roomID != roomid)
            {

                for (int i = 0; i < resToModify.timeSlots.Count; i++)
                {
                    if (resToModify.timeSlots[i].waitlist.Count == 0)
                    {
                        //If waitList for timeSlots is empty, delete from db
                        directoryOfTimeSlots.deleteTimeSlot(resToModify.timeSlots[i].timeSlotID);
                        directoryOfTimeSlots.done();
                    }
                    else
                    {
                        //Else give new reservation to the first person in waitlist
                        int userID = resToModify.timeSlots[i].waitlist.Dequeue();
                        Reservation res = directoryOfReservations.makeNewReservation(directoryOfReservations.getReservation(resid).roomID, userID, "",
                            directoryOfReservations.getReservation(resid).date);
                        directoryOfReservations.done();
                        updateWaitList(userID, directoryOfReservations.getReservation(resid).date, resToModify.timeSlots[i].hour);
                        directoryOfTimeSlots.addToWaitList(resToModify.timeSlots[i].timeSlotID, res.reservationID, resToModify.timeSlots[i].waitlist);
                        directoryOfTimeSlots.done();
                    }
                }
            }

            //Remove timeSlots that are not in common with the new reservation (depending if they have waitlist or not)
            for (int i = 0; i < resToModify.timeSlots.Count; i++)
            {
                int hour = resToModify.timeSlots[i].hour;
                bool foundSlot = false;
                for (int j = firstHour; j < lastHour; j++)
                {
                    if (hour == j)
                        foundSlot = true;
                }
                if (!foundSlot)
                {
                    //If waitList for timeSlot exist, give to new user
                    //Else delete timeSlot
                    if (resToModify.timeSlots[i].waitlist.Count == 0)
                    {
                        //If waitList for timeSlots is empty, delete from db
                        directoryOfTimeSlots.deleteTimeSlot(resToModify.timeSlots[i].timeSlotID);
                        directoryOfTimeSlots.done();
                    }
                    else
                    {
                        //Else give new reservation to the first person in waitlist
                        int userID = resToModify.timeSlots[i].waitlist.Dequeue();
                        Reservation res = directoryOfReservations.makeNewReservation(directoryOfReservations.getReservation(resid).roomID, userID, "",
                            directoryOfReservations.getReservation(resid).date);
                        directoryOfReservations.done();

                        updateWaitList(userID, directoryOfReservations.getReservation(resid).date, resToModify.timeSlots[i].hour);
                        directoryOfTimeSlots.addToWaitList(resToModify.timeSlots[i].timeSlotID, res.reservationID, resToModify.timeSlots[i].waitlist);
                        directoryOfTimeSlots.done();
                    }
                }
            }

            //Put on waitList if the new timeSlots are already taken, else create new ones
            List<int> hours = new List<int>();
            for (int i = firstHour; i < lastHour; i++)
                hours.Add(i);

            foreach (Reservation reservation in directoryOfReservations.reservationList)
            {
                if (reservation.date == dt && reservation.roomID == roomid)
                {
                    foreach (TimeSlot timeSlot in reservation.timeSlots)
                    {
                        for (int i = firstHour; i < lastHour; i++)
                        {
                            if (timeSlot.hour == i)
                            {
                                if (!timeSlot.waitlist.Contains(resToModify.userID) && reservation.userID != resToModify.userID)
                                {
                                    timeSlot.waitlist.Enqueue(resToModify.userID);
                                    directoryOfTimeSlots.addToWaitList(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
                                }
                                hours.Remove(i);
                            }
                        }
                    }
                }
            }

            if (hours.Count > 0)
            {
                for (int i = 0; i < hours.Count; i++)
                {
                    updateWaitList(resToModify.userID, resToModify.date, i);
                    directoryOfTimeSlots.makeNewTimeSlot(resToModify.reservationID, hours[i]);
                }
            }

            directoryOfTimeSlots.done();
            directoryOfReservations.modifyReservation(resToModify.reservationID, roomid, resdes, dt);
            directoryOfReservations.done();
            udpateDirectories();
        }

        public void cancelReservation(int resid)
        {
            // Loop through each timeslot
            for (int i = 0; i < directoryOfTimeSlots.timeSlotList.Count; i++)
            {
                // For those who are belonging to the reservation to be cancelled:
                if (directoryOfTimeSlots.timeSlotList[i].reservationID == resid)
                {
                    // If no one is waiting, delete it.
                    if (directoryOfTimeSlots.timeSlotList[i].waitlist.Count == 0)
                    {
                        directoryOfTimeSlots.deleteTimeSlot(directoryOfTimeSlots.timeSlotList[i].timeSlotID);
                        directoryOfTimeSlots.done();
                    }

                    // Otherwise:
                    // - Obtain the next in line, dequeue.
                    // - Make a new reservation (done - reservation)
                    // - Update the waitlists
                    // - Update the timeslot from old reservation to the new one. (done - timeslot)
                    else
                    {
                        int userID = directoryOfTimeSlots.timeSlotList[i].waitlist.Dequeue();
                        Reservation res = directoryOfReservations.makeNewReservation(directoryOfReservations.getReservation(resid).roomID, userID, "", directoryOfReservations.getReservation(resid).date);
                        directoryOfReservations.done();

                        directoryOfTimeSlots.addToWaitList(directoryOfTimeSlots.timeSlotList[i].timeSlotID, res.reservationID, directoryOfTimeSlots.timeSlotList[i].waitlist);
                        updateWaitList(userID, res.date, directoryOfTimeSlots.timeSlotList[i].hour);
                        directoryOfTimeSlots.done();
                    }
                }
            }

            // Completely done with this reservation, delete it.
            directoryOfReservations.cancelReservation(resid);
            directoryOfReservations.done();
            udpateDirectories();
        }

        public DirectoryOfTimeSlots getAllTimeSlots()
        {
            return directoryOfTimeSlots;
        }

        public DirectoryOfReservations getAllReservations()
        {
            return directoryOfReservations;
        }

        public DirectoryOfRooms getAllRooms()
        {
            return directoryOfRooms;
        }
    }
}
