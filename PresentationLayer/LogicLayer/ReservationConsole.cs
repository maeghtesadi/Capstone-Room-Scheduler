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

        public void makeReservation(int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            Reservation res = ReservationMapper.getInstance().makeNew(uid, roomid, resdes, dt);

            for (int i = firstHour; i < lastHour; i++)
            {
                TimeSlot ts = TimeSlotMapper.getInstance().makeNew(res.reservationUserID, i); //update Later
            }

            UnitOfWork.getInstance().commit();
        }

        public static void modifyReservation(int resid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            DirectoryOfReservations reservationDirectory = getAllReservations();
            Reservation resToModify = new Reservation();
            for (int i = 0; i < reservationDirectory.reservationList.Count; i++)
                if (resid == reservationDirectory.reservationList[i].reservationID)
                    resToModify = reservationDirectory.reservationList[i];

            if (resToModify.reservationDate.Date != dt.Date)
            {
                //If waitList for timeSlots exist, give to new user
                //Else delete current timeSlots

            }
            else
            if (resToModify.reservationRoomID != roomid)
            {
                //If waitList for timeSlots exist, give to new user
                //Else delete current timeSlots
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
                if(!foundSlot)
                {
                    //add new time slot to the list in reservation to modify
                    //TimeSlot ts = TimeSlotMapper.getInstance().makeNew(resToModify.reservationUserID, i); //update Later
                }
            }

            ReservationMapper.getInstance().modifyReservation(resToModify.reservationID, roomid, resdes, dt);
            UnitOfWork.getInstance().commit();
        }

        public static void cancelReservation(int resid)
        {
            DirectoryOfTimeSlots directory = getAllTimeSlots();
            for (int i = 0; i < directory.timeSlotList.Count; i++)
            {
                if (directory.timeSlotList[i].reservationID == resid)
                {
                    if(directory.timeSlotList[i].waitlist.Count == 0)
                    {
                        TimeSlotMapper.getInstance().delete(directory.timeSlotList[i].timeSlotID);
                    }
                    else
                    {
                        int userID = directory.timeSlotList[i].waitlist.Dequeue();
                        Reservation res = ReservationMapper.getInstance().makeNew(userID, ReservationMapper.getInstance().getReservation(resid).reservationRoomID,
                                                                                "", ReservationMapper.getInstance().getReservation(resid).reservationDate);

                        TimeSlotMapper.getInstance().modifyTimeSlot(directory.timeSlotList[i].timeSlotID, res.reservationID, directory.timeSlotList[i].waitlist);
                    }
                }
            }
            ReservationMapper.getInstance().delete(resid);
        }

        public static DirectoryOfTimeSlots getAllTimeSlots()
        {
            DirectoryOfTimeSlots timeSlotDirectory = new DirectoryOfTimeSlots();
           // WaitsForMapper.getInstance().getAllUsers();
            foreach (KeyValuePair<int, TimeSlot> timeSlot in TimeSlotMapper.getInstance().getAllTimeSlot())
            {
                timeSlotDirectory.timeSlotList.Add(timeSlot.Value);
            }

            for (int i = 0; i < timeSlotDirectory.timeSlotList.Count; i++ )
            {
                List <int> waitList = WaitsForMapper.getInstance().getWaitList(timeSlotDirectory.timeSlotList[i].timeSlotID);
                for (int j = 0; j < waitList.Count; j++)
                    timeSlotDirectory.timeSlotList[i].waitlist.Enqueue(waitList[j]);
            }

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
                    if (reservationDirectory.reservationList[j].reservationRoomID == roomDirectory.roomList[i].roomID)
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
