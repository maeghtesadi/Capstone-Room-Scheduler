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
        //Instance of ReservationConsole class
        private static ReservationConsole instance = new ReservationConsole();

        DirectoryOfReservations directoryOfReservations = new DirectoryOfReservations();
        DirectoryOfRooms directoryOfRooms = new DirectoryOfRooms();
        DirectoryOfTimeSlots directoryOfTimeSlots = new DirectoryOfTimeSlots();
        UserCatalog userCatalog = new UserCatalog();
        
        public static ReservationConsole getInstance()
        {
            return instance;
        }

        //default constructor
        public ReservationConsole()
        {
            updateDirectories();
        }

        //public void makeReservation(int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        public void makeReservation(int userID, int roomID, string desc, DateTime date, int firstHour, int lastHour)
        {
            Reservation res = new Reservation();
            List<int> hours = new List<int>();
            for (int i = firstHour; i <= lastHour; i++)
                hours.Add(i);

            foreach (Reservation reservation in directoryOfReservations.reservationList)
            {
                // Compare if the date (not the time portion) are the same and the rooms are the same
                if (reservation.date.Date == date.Date && reservation.roomID == roomID)
                {
                    foreach (TimeSlot timeSlot in reservation.timeSlots)
                    {
                        for (int i = firstHour; i <=lastHour; i++)
                        {
                            if (timeSlot.hour == i)
                            {
                                if (!timeSlot.waitlist.Contains(userID) && reservation.userID != userID)
                                {
                                    timeSlot.waitlist.Enqueue(userID);
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
                res = directoryOfReservations.makeNewReservation(roomID, userID, desc, date);
                for (int i = 0; i < hours.Count; i++)
                {
                    directoryOfTimeSlots.makeNewTimeSlot(res.reservationID, hours[i]);
                    
                    updateWaitList(userID, date, i);
                    
                }
            }

            directoryOfTimeSlots.done();
            directoryOfReservations.done();
            updateDirectories();
        }

        public void updateDirectories()
        {
            // Only Console has visibility over DirectoryOfTimeSlot, so this was loop was put here instead of DirectoryOfReservation
            // Updating timeSlots of each reservations
            List<TimeSlot> timeSlotList = directoryOfTimeSlots.getAllTimeSlot().Values.ToList();
            timeSlotList.Sort((x, y) => x.hour.CompareTo(y.hour));
            for (int i = 0; i < directoryOfReservations.reservationList.Count; i++)
            {
                foreach (TimeSlot timeSlot in timeSlotList)
                    if (directoryOfReservations.reservationList[i].reservationID == timeSlot.reservationID && !directoryOfReservations.reservationList[i].timeSlots.Contains(timeSlot))
                        directoryOfReservations.reservationList[i].timeSlots.Add(timeSlot);

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
            //directoryOfRooms.roomList.Clear();
            for (int i = 0; i < directoryOfRooms.roomList.Count; i++)
            {
                foreach (KeyValuePair<int, Reservation> reservation in directoryOfReservations.getAllReservation())
                {
                    if (reservation.Value.roomID == directoryOfRooms.roomList[i].roomID)
                        directoryOfRooms.roomList[i].roomReservations.Add(reservation.Value);
                }
            }
        }

        public void updateWaitList(int userID, DateTime date, int hour)
        {
            foreach (TimeSlot timeSlot in directoryOfTimeSlots.timeSlotList)
            {
                // Obtain the date associated with that timeslot for the current reservation
                DateTime timeSlotDate = directoryOfReservations.getReservation(timeSlot.reservationID).date;

                // We only want to remove the user from the waitlist of timeslots of the same date and hour 
                if (timeSlot.waitlist.Contains(userID) && timeSlotDate.Equals(date) && timeSlot.hour == hour)
                {
                    Queue<int> newQueue = new Queue<int>();
                    int size = timeSlot.waitlist.Count;
                    for (int i = 0; i < size; i++)
                    {
                        if (timeSlot.waitlist.Peek() == userID)
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

        public void modifyReservation(int resID, int roomID, string desc, DateTime date, int firstHour, int lastHour)
        {
            Reservation resToModify = new Reservation();
            for (int i = 0; i < directoryOfReservations.reservationList.Count; i++)
            {
                if (resID == directoryOfReservations.reservationList[i].reservationID)
                    resToModify = directoryOfReservations.reservationList[i];
            }

            if (resToModify.date.Date != date.Date || resToModify.roomID != roomID)
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
                        Reservation res = directoryOfReservations.makeNewReservation(directoryOfReservations.getReservation(resID).roomID, userID, "",
                            directoryOfReservations.getReservation(resID).date);
                        directoryOfReservations.done();
                        updateWaitList(userID, directoryOfReservations.getReservation(resID).date, resToModify.timeSlots[i].hour);
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
                for (int j = firstHour; j <= lastHour; j++)
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
                        Reservation res = directoryOfReservations.makeNewReservation(directoryOfReservations.getReservation(resID).roomID, userID, "",
                            directoryOfReservations.getReservation(resID).date);
                        directoryOfReservations.done();

                        updateWaitList(userID, directoryOfReservations.getReservation(resID).date, resToModify.timeSlots[i].hour);
                        directoryOfTimeSlots.addToWaitList(resToModify.timeSlots[i].timeSlotID, res.reservationID, resToModify.timeSlots[i].waitlist);
                        directoryOfTimeSlots.done();
                    }
                }
            }

            //Put on waitList if the new timeSlots are already taken, else create new ones
            List<int> hours = new List<int>();
            for (int i = firstHour; i <= lastHour; i++)
                hours.Add(i);

            foreach (Reservation reservation in directoryOfReservations.reservationList)
            {
                if (reservation.date == date && reservation.roomID == roomID)
                {
                    foreach (TimeSlot timeSlot in reservation.timeSlots)
                    {
                        for (int i = firstHour; i <= lastHour; i++)
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
            directoryOfReservations.modifyReservation(resToModify.reservationID, roomID, desc, date);
            directoryOfReservations.done();
            updateDirectories();
        }

        public void cancelReservation(int resID)
        {
            // Loop through each timeslot
            for (int i = 0; i < directoryOfTimeSlots.timeSlotList.Count; i++)
            {
                // For those who are belonging to the reservation to be cancelled:
                if (directoryOfTimeSlots.timeSlotList[i].reservationID == resID)
                {
                    // If no one is waiting, delete it.
                    if (directoryOfTimeSlots.timeSlotList[i].waitlist.Count == 0)
                    {
                        directoryOfTimeSlots.deleteTimeSlot(directoryOfTimeSlots.timeSlotList[i].timeSlotID);
                        i--;
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
                        Reservation res = directoryOfReservations.makeNewReservation(directoryOfReservations.getReservation(resID).roomID, userID, "", directoryOfReservations.getReservation(resID).date);
                        directoryOfReservations.done();

                        directoryOfTimeSlots.addToWaitList(directoryOfTimeSlots.timeSlotList[i].timeSlotID, res.reservationID, directoryOfTimeSlots.timeSlotList[i].waitlist);
                        updateWaitList(userID, res.date, directoryOfTimeSlots.timeSlotList[i].hour);
                        directoryOfTimeSlots.done();
                    }
                }
            }

            // Completely done with this reservation, delete it.
            directoryOfReservations.cancelReservation(resID);
            directoryOfReservations.done();
            updateDirectories();
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
        public UserCatalog getUserCatalog()
        {
            return userCatalog;
        }

        /**
         * method for the constraint of not exceeding a reservation of 4 hours per day per person
         */
        public Boolean dailyConstraintCheck(int userID, DateTime date, int firstHour, int lastHour)
        {
            //interval of hours for the desired reservvation
            int newHours = lastHour - firstHour + 1;
            //number of hours of reservation currently for chosen day
            int currentHours = directoryOfTimeSlots.findHoursByReservationIDs(directoryOfReservations.findReservationsByIDAndDate(userID, date));
            //checks of reservation is possible according to constraint
            if (currentHours + newHours < 4)
            {
                return true;
            }
        
            return false;
        }
       
    }
}
