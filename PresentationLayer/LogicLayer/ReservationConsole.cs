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

        //DirectoryOfReservations directoryOfReservations = new DirectoryOfReservations();
        //DirectoryOfRooms directoryOfRooms = new DirectoryOfRooms();
        //DirectoryOfTimeSlots directoryOfTimeSlots = new DirectoryOfTimeSlots();
        //UserCatalog userCatalog = new UserCatalog();

        public static ReservationConsole getInstance()
        {
            return instance;
        }

        //default constructor
        public ReservationConsole()
        {
            udpateDirectories();
        }

        //public void makeReservation(int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        public void makeReservation(int userID, int roomID, string desc, DateTime date, int firstHour, int lastHour)
        {
            //Reservation res = new Reservation();
            List<int> hours = new List<int>();
            for (int i = firstHour; i <= lastHour; i++)
                hours.Add(i);

            //foreach (Reservation reservation in directoryOfReservations.reservationList)
            foreach (Reservation reservation in ReservationMapper.getInstance().getListOfReservations())
            {
                // Compare if the date (not the time portion) are the same and the rooms are the same
                if (reservation.date.Date == date.Date && reservation.roomID == roomID)
                {
                    foreach (TimeSlot timeSlot in reservation.timeSlots)
                    {
                        for (int i = firstHour; i <= lastHour; i++)
                        {
                            if (timeSlot.hour == i)
                            {
                                if (!timeSlot.waitlist.Contains(userID) && reservation.userID != userID)
                                {
                                    timeSlot.waitlist.Enqueue(userID);
                                    TimeSlotMapper.getInstance().setTimeSlot(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
                                    //directoryOfTimeSlots.addToWaitList(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
                                }
                                hours.Remove(i);
                            }
                        }
                    }
                }
            }

            if (hours.Count > 0)
            {
                Reservation res = ReservationMapper.getInstance().makeNew(userID, roomID, desc, date);
                //res = directoryOfReservations.makeNewReservation(roomID, userID, desc, date);

                for (int i = 0; i < hours.Count; i++)
                {
                    TimeSlot timeSlot = TimeSlotMapper.getInstance().makeNew(res.reservationID, hours[i]);
                    //directoryOfTimeSlots.makeNewTimeSlot(res.reservationID, hours[i]);
                    updateWaitList(userID, date, i);
                }
            }

            //UnitOfWork.getInstance().commit();
            TimeSlotMapper.getInstance().done();
            ReservationMapper.getInstance().done();
            //directoryOfTimeSlots.done();
            //directoryOfReservations.done();
            udpateDirectories(); //what to do about this
        }

        public void udpateDirectories()
        {
            
            // Updating timeSlots of each reservations
            for (int i = 0; i < (ReservationMapper.getInstance().getListOfReservations()).Count; i++)
            {
                foreach (KeyValuePair<int, TimeSlot> timeSlot in TimeSlotMapper.getInstance().getAllTimeSlot())
                {
                    if (ReservationMapper.getInstance().getListOfReservations()[i].reservationID == timeSlot.Value.reservationID)
                        ReservationMapper.getInstance().getListOfReservations()[i].timeSlots.Add(timeSlot.Value);
                }
            }

            // Updating the waitList of each timeSlot
            for (int i = 0; i < TimeSlotMapper.getInstance().getListOfTimeSlots().Count; i++)
            {
                List<int> waitList = TimeSlotMapper.getInstance().getAllUsers(TimeSlotMapper.getInstance().getListOfTimeSlots()[i].timeSlotID);
                if (waitList != null)
                    for (int j = 0; j < waitList.Count; j++)
                        TimeSlotMapper.getInstance().getListOfTimeSlots()[i].waitlist.Enqueue(waitList[j]);
            }

            //Updating the reservations for each room
            for (int i = 0; i < RoomMapper.getInstance().getListOfRooms().Count; i++)
            {
                foreach (KeyValuePair<int, Reservation> reservation in ReservationMapper.getInstance().getAllReservation())
                {
                    if (reservation.Value.roomID == RoomMapper.getInstance().getListOfRooms()[i].roomID)
                        RoomMapper.getInstance().getListOfRooms()[i].roomReservations.Add(reservation.Value);
                }
            }
        }

        public void updateWaitList(int userID, DateTime date, int hour)
        {
            foreach (TimeSlot timeSlot in TimeSlotMapper.getInstance().getListOfTimeSlots()) //CHANGE THIS IN MAPPER.
            {
                // Obtain the date associated with that timeslot for the current reservation
                DateTime timeSlotDate = ReservationMapper.getInstance().getReservation(timeSlot.reservationID).date;
                // DateTime timeSlotDate = directoryOfReservations.getReservation(timeSlot.reservationID).date;

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
                    TimeSlotMapper.getInstance().setTimeSlot(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
                    //directoryOfTimeSlots.addToWaitList(timeSlot.timeSlotID, timeSlot.reservationID, newQueue);
                }

            }
            TimeSlotMapper.getInstance().done();
            //directoryOfTimeSlots.done(); wat2do
        }

        public void modifyReservation(int resID, int roomID, string desc, DateTime date, int firstHour, int lastHour)
        {
            Reservation resToModify = new Reservation();
            for (int i = 0; i < ReservationMapper.getInstance().getListOfReservations().Count; i++)
            {
                if (resID == ReservationMapper.getInstance().getListOfReservations()[i].reservationID)
                    resToModify = ReservationMapper.getInstance().getListOfReservations()[i];
            }

            if (resToModify.date.Date != date.Date || resToModify.roomID != roomID)
            {

                for (int i = 0; i < resToModify.timeSlots.Count; i++)
                {
                    if (resToModify.timeSlots[i].waitlist.Count == 0)
                    {
                        //If waitList for timeSlots is empty, delete from db
                        TimeSlotMapper.getInstance().delete(resToModify.timeSlots[i].timeSlotID);
                        TimeSlotMapper.getInstance().done();
                    }
                    else
                    {
                        //Else give new reservation to the first person in waitlist
                        int userID = resToModify.timeSlots[i].waitlist.Dequeue();
                        Reservation res = ReservationMapper.getInstance().makeNew(userID, ReservationMapper.getInstance().getReservation(resID).roomID,
                            "", ReservationMapper.getInstance().getReservation(resID).date);
                        ReservationMapper.getInstance().done();
                        updateWaitList(userID, ReservationMapper.getInstance().getReservation(resID).date, resToModify.timeSlots[i].hour);
                        TimeSlotMapper.getInstance().setTimeSlot(resToModify.timeSlots[i].timeSlotID, res.reservationID, resToModify.timeSlots[i].waitlist);
                        TimeSlotMapper.getInstance().done();
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
                        TimeSlotMapper.getInstance().delete(resToModify.timeSlots[i].timeSlotID);
                        TimeSlotMapper.getInstance().done();
                        //directoryOfTimeSlots.deleteTimeSlot(resToModify.timeSlots[i].timeSlotID);
                        //directoryOfTimeSlots.done();
                    }
                    else
                    {
                        //Else give new reservation to the first person in waitlist
                        int userID = resToModify.timeSlots[i].waitlist.Dequeue();
                        int myroomid = ReservationMapper.getInstance().getReservation(resID).roomID;
                        DateTime mydate = ReservationMapper.getInstance().getReservation(resID).date;

                        Reservation res = ReservationMapper.getInstance().makeNew(myroomid, userID, "", mydate);
                        //Reservation res = directoryOfReservations.makeNewReservation(directoryOfReservations.getReservation(resID).roomID, userID, "",
                        //   directoryOfReservations.getReservation(resID).date);
                        //directoryOfReservations.done();
                        ReservationMapper.getInstance().done();

                        updateWaitList(userID, ReservationMapper.getInstance().getReservation(resID).date, resToModify.timeSlots[i].hour);
                        TimeSlotMapper.getInstance().setTimeSlot(resToModify.timeSlots[i].timeSlotID, res.reservationID, resToModify.timeSlots[i].waitlist);
                        //directoryOfTimeSlots.done();
                        TimeSlotMapper.getInstance().done();
                    }
                }
            }

            //Put on waitList if the new timeSlots are already taken, else create new ones
            List<int> hours = new List<int>();
            for (int i = firstHour; i < lastHour; i++)
                hours.Add(i);

            foreach (Reservation reservation in ReservationMapper.getInstance().getListOfReservations()) //change this
            {
                if (reservation.date == date && reservation.roomID == roomID)
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
                                    TimeSlotMapper.getInstance().setTimeSlot(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
                                    //directoryOfTimeSlots.addToWaitList(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
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
                    //directoryOfTimeSlots.makeNewTimeSlot(resToModify.reservationID, hours[i]);
                    TimeSlotMapper.getInstance().makeNew(resToModify.reservationID, hours[i]);
                }
            }

            //directoryOfTimeSlots.done();
            ReservationMapper.getInstance().modifyReservation(resToModify.reservationID, roomID, desc, date);
            //directoryOfReservations.modifyReservation(resToModify.reservationID, roomID, desc, date);
            ReservationMapper.getInstance().done();
            //directoryOfReservations.done();
            udpateDirectories(); //put these methods in their respective mappers :)
        }

        public void cancelReservation(int resID)
        {
            // Loop through each timeslot
            for (int i = 0; i < TimeSlotMapper.getInstance().getListOfTimeSlots().Count; i++) //make method in mappers that returns the count
            {
                // For those who are belonging to the reservation to be cancelled:
                //if (directoryOfTimeSlots.timeSlotList[i].reservationID == resID)
                if (TimeSlotMapper.getInstance().getListOfTimeSlots()[i].reservationID == resID) //make a method in mappers that returns the respective directory lists
                {
                    // If no one is waiting, delete it.
                    if (TimeSlotMapper.getInstance().getListOfTimeSlots()[i].waitlist.Count == 0)
                    {
                        //directoryOfTimeSlots.deleteTimeSlot(directoryOfTimeSlots.timeSlotList[i].timeSlotID);
                        //directoryOfTimeSlots.done();
                        TimeSlotMapper.getInstance().delete(TimeSlotMapper.getInstance().getListOfTimeSlots()[i].timeSlotID);
                        TimeSlotMapper.getInstance().done();
                    }

                    // Otherwise:
                    // - Obtain the next in line, dequeue.
                    // - Make a new reservation (done - reservation)
                    // - Update the waitlists
                    // - Update the timeslot from old reservation to the new one. (done - timeslot)
                    else
                    {
                        //int userID = directoryOfTimeSlots.timeSlotList[i].waitlist.Dequeue();
                        int userID = TimeSlotMapper.getInstance().getListOfTimeSlots()[i].waitlist.Dequeue();
                        Reservation res = ReservationMapper.getInstance().makeNew(ReservationMapper.getInstance().getReservation(resID).roomID, userID, 
                            "", ReservationMapper.getInstance().getReservation(resID).date);

                        //directoryOfReservations.done();
                        ReservationMapper.getInstance().done();

                        TimeSlotMapper.getInstance().setTimeSlot(TimeSlotMapper.getInstance().getListOfTimeSlots()[i].timeSlotID, res.reservationID,
                            TimeSlotMapper.getInstance().getListOfTimeSlots()[i].waitlist);

                        updateWaitList(userID, res.date, TimeSlotMapper.getInstance().getListOfTimeSlots()[i].hour);
                        //directoryOfTimeSlots.done();
                        TimeSlotMapper.getInstance().done();
                    }
                }
            }

            // Completely done with this reservation, delete it.
            ReservationMapper.getInstance().delete(resID);
            ReservationMapper.getInstance().done();
            //directoryOfReservations.cancelReservation(resID);
            //directoryOfReservations.done();
            udpateDirectories();
        }

        //public DirectoryOfTimeSlots getAllTimeSlots()
        //{
        //    return directoryOfTimeSlots;
        //}

        //public DirectoryOfReservations getAllReservations()
        //{
        //    return directoryOfReservations;
        //}

        //public DirectoryOfRooms getAllRooms()
        //{
        //    return directoryOfRooms;
        //}
        //public UserCatalog getUserCatalog()
        //{
        //    return userCatalog;
        //}
    }
}
