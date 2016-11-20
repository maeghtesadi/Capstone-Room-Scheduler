using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappers;
namespace LogicLayer
{
    public class DirectoryOfReservations
    {
        public List<Reservation> reservationList { get; set; }

        public DirectoryOfReservations()
        {
            DirectoryOfTimeSlots timeSlotsDirectory = new DirectoryOfTimeSlots();

            foreach (KeyValuePair<int, Reservation> reservation in ReservationMapper.getInstance().getAllReservation())
            {
                reservationList.Add(reservation.Value);
            }

            for (int i = 0; i < reservationList.Count; i++)
            {
                for (int j = 0; j < timeSlotsDirectory.timeSlotList.Count; j++)
                {
                    if (reservationList[i].reservationID == timeSlotsDirectory.timeSlotList[j].reservationID)
                        reservationList[i].timeSlots.Add(timeSlotsDirectory.timeSlotList[j]);
                }
            }
        }

        public void updateWaitList(int userid, DateTime date, int hour)
        {
            DirectoryOfTimeSlots timeSlotsDirectory = new DirectoryOfTimeSlots();

            foreach (TimeSlot timeSlot in timeSlotsDirectory.timeSlotList)
            {
                // Obtain the date associated with that timeslot for the current reservation
                DateTime timeSlotDate = ReservationMapper.getInstance().getReservation(timeSlot.reservationID).date;

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
                    TimeSlotMapper.getInstance().setTimeSlot(timeSlot.timeSlotID, timeSlot.reservationID, newQueue);
                }

            }
            TimeSlotMapper.getInstance().done();
        }

        public void makeReservation(int uid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            //DirectoryOfRooms roomDirectory = getAllRooms();
            Reservation res = new Reservation();
            List<int> hours = new List<int>();
            for (int i = firstHour; i < lastHour; i++)
                hours.Add(i);

            foreach (Reservation reservation in reservationList)
            {
                if (reservation.date == dt && reservation.roomID == roomid)
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
                                    TimeSlotMapper.getInstance().setTimeSlot(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
                                }
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
                    updateWaitList(uid, dt, i);
                    TimeSlotMapper.getInstance().done();
                }
            }

            TimeSlotMapper.getInstance().done();
            ReservationMapper.getInstance().done();
        }

        public void cancelReservation(int resid)
        {
            DirectoryOfTimeSlots directory = new DirectoryOfTimeSlots();

            // Loop through each timeslot
            for (int i = 0; i < directory.timeSlotList.Count; i++)
            {
                // For those who are belonging to the reservation to be cancelled:
                if (directory.timeSlotList[i].reservationID == resid)
                {
                    // If no one is waiting, delete it.
                    if (directory.timeSlotList[i].waitlist.Count == 0)
                    {
                        TimeSlotMapper.getInstance().delete(directory.timeSlotList[i].timeSlotID);
                        TimeSlotMapper.getInstance().done();
                    }

                    // Otherwise:
                    // - Obtain the next in line, dequeue.
                    // - Make a new reservation (done - reservation)
                    // - Update the waitlists
                    // - Update the timeslot from old reservation to the new one. (done - timeslot)
                    else
                    {
                        int userID = directory.timeSlotList[i].waitlist.Dequeue();
                        Reservation res = ReservationMapper.getInstance().makeNew(userID, ReservationMapper.getInstance().getReservation(resid).roomID,
                                                                                "", ReservationMapper.getInstance().getReservation(resid).date);
                        ReservationMapper.getInstance().done();


                        TimeSlotMapper.getInstance().setTimeSlot(directory.timeSlotList[i].timeSlotID, res.reservationID, directory.timeSlotList[i].waitlist);
                        updateWaitList(userID, res.date, directory.timeSlotList[i].hour);
                        TimeSlotMapper.getInstance().done();


                        //updateWaitList(userID);
                    }
                }
            }

            // Completely done with this reservation, delete it.
            ReservationMapper.getInstance().delete(resid);
            ReservationMapper.getInstance().done();
        }

        public void modifyReservation(int resid, int roomid, string resdes, DateTime dt, int firstHour, int lastHour)
        {
            Reservation resToModify = new Reservation();
            for (int i = 0; i < reservationList.Count; i++)
            {
                if (resid == reservationList[i].reservationID)
                    resToModify = reservationList[i];
            }

            if (resToModify.date.Date != dt.Date || resToModify.roomID != roomid)
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
                        Reservation res = ReservationMapper.getInstance().makeNew(userID, ReservationMapper.getInstance().getReservation(resid).roomID,
                                                                            "", ReservationMapper.getInstance().getReservation(resid).date);
                        ReservationMapper.getInstance().done();

                        updateWaitList(userID, ReservationMapper.getInstance().getReservation(resid).date, resToModify.timeSlots[i].hour);
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
                    }
                    else
                    {
                        //Else give new reservation to the first person in waitlist
                        int userID = resToModify.timeSlots[i].waitlist.Dequeue();
                        Reservation res = ReservationMapper.getInstance().makeNew(userID, ReservationMapper.getInstance().getReservation(resid).roomID,
                                                                            "", ReservationMapper.getInstance().getReservation(resid).date);
                        ReservationMapper.getInstance().done();

                        updateWaitList(userID, ReservationMapper.getInstance().getReservation(resid).date, resToModify.timeSlots[i].hour);
                        TimeSlotMapper.getInstance().setTimeSlot(resToModify.timeSlots[i].timeSlotID, res.reservationID, resToModify.timeSlots[i].waitlist);
                        TimeSlotMapper.getInstance().done();
                    }
                }
                else
                {
                    //do nothing (keep the timeslot)
                }
            }


            //Put on waitList if the new timeSlots are already taken, else create new ones
            List<int> hours = new List<int>();
            for (int i = firstHour; i < lastHour; i++)
                hours.Add(i);

            foreach (Reservation reservation in reservationList)
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
                                    TimeSlotMapper.getInstance().setTimeSlot(timeSlot.timeSlotID, timeSlot.reservationID, timeSlot.waitlist);
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
                    TimeSlotMapper.getInstance().makeNew(resToModify.reservationID, hours[i]); //update Later
                }

            }

            TimeSlotMapper.getInstance().done();
            ReservationMapper.getInstance().modifyReservation(resToModify.reservationID, roomid, resdes, dt);
            ReservationMapper.getInstance().done();
        }

        public List<Reservation> findByDate(DateTime date)
        {
            List<Reservation> listByDate = new List<Reservation>();
            foreach(Reservation reservation in reservationList ) {
                if(reservation.date.Date == date.Date) {
                    listByDate.Add(reservation);
                }
            }
            return listByDate;
        }

        public List<Reservation> findByUser(int userId)
        {
            List<Reservation> listByuserId = new List<Reservation>();
            foreach (Reservation reservation in reservationList)
            {
                if (reservation.reservationID == userId)
                {
                    listByuserId.Add(reservation);
                }
            }
            return listByuserId;
        }

        public List<Reservation> filterByBlock(DateTime date) //for front end, in progress
        {
            List<Reservation> listByDate = new List<Reservation>();
            foreach (Reservation reservation in reservationList)
            {
                if (reservation.date == date)
                {
                    listByDate.Add(reservation);
                }
            }
            return listByDate;
        }


    }
}
