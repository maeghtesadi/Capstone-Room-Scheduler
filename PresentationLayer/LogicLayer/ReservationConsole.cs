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

        public static void makeReservation(int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            DirectoryOfRooms roomDirectory = getAllRooms();
            DirectoryOfReservations reservationDirectory = getAllReservations();
            Reservation res = new Reservation();
            List<int> hours = new List<int>();
            for (int i = firstHour; i < lastHour; i++)
                hours.Add(i);

            foreach (Reservation reservation in reservationDirectory.reservationList)
            {
                if (reservation.date == dt && reservation.roomID == roomid)
                {
                    foreach (TimeSlot timeSlot in reservation.timeSlots)
                    {
                        for (int i = firstHour; i < lastHour; i++)
                        {
                            if (timeSlot.hour == i)
                            {
                                timeSlot.waitlist.Enqueue(uid);
                                TimeSlotMapper.getInstance().setTimeSlot(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
                                TimeSlotMapper.getInstance().done();
                                hours.Remove(i);
                            }
                        }
                    }
                }
            }

            if (hours.Count > 0)
            {
                res = ReservationMapper.getInstance().makeNew(uid, roomid, resdes, dt);
                for (int i = 0; i < hours.Count; i++)
                {
                    TimeSlotMapper.getInstance().makeNew(res.reservationID, hours[i]); //update Later
                    TimeSlotMapper.getInstance().done();
                }
            }

            TimeSlotMapper.getInstance().done();
            ReservationMapper.getInstance().done();
            //updateWaitLIst(uid);
            //UnitOfWork.getInstance().commit();
        }

        //Used when calling create reservation 
        public static void updateWaitList(int userid)
        {
            DirectoryOfTimeSlots timeSlotsDirectory = getAllTimeSlots();

            foreach (TimeSlot timeSlot in timeSlotsDirectory.timeSlotList)
            {
                if (timeSlot.waitlist.Contains(userid))
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
                    TimeSlotMapper.getInstance().setTimeSlot(timeSlot.timeSlotID, timeSlot.reservationID, newQueue);
                }
             
            }
            TimeSlotMapper.getInstance().done();
        }

        public static void modifyReservation(int resid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            DirectoryOfReservations reservationDirectory = getAllReservations();
            Reservation resToModify = new Reservation();
            for (int i = 0; i < reservationDirectory.reservationList.Count; i++)
            {
                if (resid == reservationDirectory.reservationList[i].reservationID)
                    resToModify = reservationDirectory.reservationList[i];
            }

            if (resToModify.date.Date != dt.Date || resToModify.roomID != roomid)
            {

                for (int i = 0; i < resToModify.timeSlots.Count; i++)
                {
                    if (resToModify.timeSlots[i].waitlist.Count == 0)
                    {
                        //If waitList for timeSlots is empty, delete from db
                        TimeSlotMapper.getInstance().delete(resToModify.timeSlots[i].timeSlotID);
                    }
                    else
                    {
                        //Else give new reservation to the first person in waitlist
                        int userID = resToModify.timeSlots[i].waitlist.Dequeue();
                        Reservation res = ReservationMapper.getInstance().makeNew(userID, ReservationMapper.getInstance().getReservation(resid).roomID,
                                                                            "", ReservationMapper.getInstance().getReservation(resid).date);
                        updateWaitList(userID);
                        TimeSlotMapper.getInstance().setTimeSlot(resToModify.timeSlots[i].timeSlotID, res.reservationID, resToModify.timeSlots[i].waitlist);
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
                    }
                    else
                    {
                        //Else give new reservation to the first person in waitlist
                        int userID = resToModify.timeSlots[i].waitlist.Dequeue();
                        Reservation res = ReservationMapper.getInstance().makeNew(userID, ReservationMapper.getInstance().getReservation(resid).roomID,
                                                                            "", ReservationMapper.getInstance().getReservation(resid).date);
                        updateWaitList(userID);
                        TimeSlotMapper.getInstance().setTimeSlot(resToModify.timeSlots[i].timeSlotID, res.reservationID, resToModify.timeSlots[i].waitlist);
                    }
                }
                else
                {
                    //do nothing (keep the timeslot)
                }
            }

            for (int i = firstHour; i < lastHour; i++)
            {
                bool foundSlot = false;
                for (int j = 0; j < resToModify.timeSlots.Count; j++)
                {
                    if (i == resToModify.timeSlots[j].hour)
                        foundSlot = true;
                }
                if (!foundSlot)
                {
                    //add new time slot to the list in reservation to modify
                    TimeSlot ts = TimeSlotMapper.getInstance().makeNew(resToModify.reservationID, i); //update Later
                }
            }

            ReservationMapper.getInstance().modifyReservation(resToModify.reservationID, roomid, resdes, dt);

            TimeSlotMapper.getInstance().done();
            ReservationMapper.getInstance().done();
            //UnitOfWork.getInstance().commit();
        }

        public static void cancelReservation(int resid)
        {
            DirectoryOfTimeSlots directory = getAllTimeSlots();
            for (int i = 0; i < directory.timeSlotList.Count; i++)
            {
                if (directory.timeSlotList[i].reservationID == resid)
                {
                    if (directory.timeSlotList[i].waitlist.Count == 0)
                    {
                        TimeSlotMapper.getInstance().delete(directory.timeSlotList[i].timeSlotID);
                    }
                    else
                    {
                        int userID = directory.timeSlotList[i].waitlist.Dequeue();
                        Reservation res = ReservationMapper.getInstance().makeNew(userID, ReservationMapper.getInstance().getReservation(resid).roomID,
                                                                                "", ReservationMapper.getInstance().getReservation(resid).date);
                        updateWaitList(userID);
                        TimeSlotMapper.getInstance().setTimeSlot(directory.timeSlotList[i].timeSlotID, res.reservationID, directory.timeSlotList[i].waitlist);
                    }
                }
            }
            ReservationMapper.getInstance().delete(resid);
            TimeSlotMapper.getInstance().done();
            ReservationMapper.getInstance().done();
        }

        //get up-to-date timeslots from database 
        public static DirectoryOfTimeSlots getAllTimeSlots()
        {
            DirectoryOfTimeSlots timeSlotDirectory = new DirectoryOfTimeSlots();
            // WaitsForMapper.getInstance().getAllUsers();
            foreach (KeyValuePair<int, TimeSlot> timeSlot in TimeSlotMapper.getInstance().getAllTimeSlot())
            {
                timeSlotDirectory.timeSlotList.Add(timeSlot.Value);
            }

            for (int i = 0; i < timeSlotDirectory.timeSlotList.Count; i++)
            {
                List<int> waitList = WaitsForMapper.getInstance().getAllUsers(timeSlotDirectory.timeSlotList[i].timeSlotID);
                for (int j = 0; j < waitList.Count; j++)
                    timeSlotDirectory.timeSlotList[i].waitlist.Enqueue(waitList[j]);
            }
            //WaitsForMapper.getInstance().Done();
            return timeSlotDirectory;
        }

        public static DirectoryOfReservations getAllReservations()
        {
            DirectoryOfReservations reservationDirectory = new DirectoryOfReservations();
            DirectoryOfTimeSlots timeSlotsDirectory = getAllTimeSlots();

            foreach (KeyValuePair<int, Reservation> reservation in ReservationMapper.getInstance().getAllReservation())
            {
                reservationDirectory.reservationList.Add(reservation.Value);
            }

            for (int i = 0; i < reservationDirectory.reservationList.Count; i++)
            {
                for (int j = 0; j < timeSlotsDirectory.timeSlotList.Count; j++)
                {
                    if (reservationDirectory.reservationList[i].reservationID == timeSlotsDirectory.timeSlotList[j].reservationID)
                        reservationDirectory.reservationList[i].timeSlots.Add(timeSlotsDirectory.timeSlotList[j]);
                }
            }
            return reservationDirectory;
        }

        public static DirectoryOfRooms getAllRooms()
        {
            DirectoryOfRooms roomDirectory = new DirectoryOfRooms();
            DirectoryOfReservations reservationDirectory = getAllReservations();

            foreach (KeyValuePair<int, Room> room in RoomMapper.getInstance().getAllRooms())
            {
                roomDirectory.roomList.Add(room.Value);
            }

            for (int i = 0; i < roomDirectory.roomList.Count; i++)
            {
                for (int j = 0; j < reservationDirectory.reservationList.Count; j++)
                {
                    if (reservationDirectory.reservationList[j].roomID == roomDirectory.roomList[i].roomID)
                        roomDirectory.roomList[i].roomReservations.Add(reservationDirectory.reservationList[j]);
                }
            }

            return roomDirectory;
        }

        public static void addToWaitList(int roomID, int timeSlotID, DateTime date, int userID)
        {

        }

    }
}
