using System;
using System.Collections.Generic;
using TDG;
using LogicLayer;
using CapstoneRoomScheduler.LogicLayer.IdentityMaps;
using System.Threading;

namespace Mappers
{
    class ReservationMapper
    {

        //Instance of this mapper object
        private static ReservationMapper instance = new ReservationMapper();

        private TDGReservation tdgReservation = TDGReservation.getInstance();
        private ReservationIdentityMap reservationIdentityMap = ReservationIdentityMap.getInstance();
        private WaitsForMapper waitsForMapper = WaitsForMapper.getInstance();


        DirectoryOfReservations directoryOfReservations = new DirectoryOfReservations();
        DirectoryOfRooms directoryOfRooms = new DirectoryOfRooms();
        DirectoryOfTimeSlots directoryOfTimeSlots = new DirectoryOfTimeSlots();
        UserCatalog userCatalog = new UserCatalog();



        // The last ID that is used
        private int lastID;

        // Lock to modify last ID
        private readonly Object lockLastID = new Object();

        //default constructor
        private ReservationMapper()
        {
            this.lastID = tdgReservation.getLastID();
        }


        public static ReservationMapper getInstance()
        {
            return instance;
        }

        /**
         *  Obtain the next ID available
         **/
        private int getNextID()
        {
            // Increments the last ID atomically, return the increment value
            int nextID;

            lock (this.lockLastID)
            {
                this.lastID++;
                nextID = this.lastID;
            }
            return nextID;
        }

        /**
         * Handles the creation of a new object of type Reservation.
         **/

        public Reservation makeNew(int userID, int roomID, string desc, DateTime date)
        {
            // Get the next reservation ID
            int reservationID = getNextID();

            //Make a new reservation object
            Reservation reservation = new Reservation();
            reservation.reservationID = (reservationID);
            reservation.userID = (userID);
            reservation.roomID = (roomID);
            reservation.description = (desc);
            reservation.date = (date);

            //Add new reservation object to the identity map, in Live memory.
            reservationIdentityMap.addTo(reservation);

            //Add reservation object to UoW registry (register as a new RESERVATION).
            //It will be  created in the DB once the user is ready to commit everything.

            UnitOfWork.getInstance().registerNew(reservation);

            return reservation;

        }

        /**
         * Retrieve a reservation given its reservationID.
         */

        public Reservation getReservation(int reservationID)
        {

            //Try to obtain the reservation from the Reservation indentity map
            Reservation reservation = reservationIdentityMap.find(reservationID);
            Object[] result = null;


            if (reservation == null)
            {
                //If not found in Reservation identity map then, it uses TDG to try to retrieve from DB.
                result = tdgReservation.get(reservationID);

                if (result != null)
                {
                    /**The reservation was obtained from the TDG.
                     * The TDG got (retrieved) reservation from the DB.
                    **/

                    //mapper must add reservation to the ReservationIdentityMap
                    reservation = new Reservation();
                    reservation.reservationID = ((int)result[0]); //reservationID
                    reservation.userID = ((int)result[1]); //userID
                    reservation.roomID = ((int)result[2]); //roomID
                    reservation.description = ((String)result[3]); //desc
                    reservation.date = (Convert.ToDateTime(result[4])); //date
                    reservationIdentityMap.addTo(reservation);

                }
            }

            //Null is returned if it is not found in the reservation identity map NOR in the DB
            return reservation;

        }


        /**
         * Retrieve all resevations
         * */
        public Dictionary<int, Reservation> getAllReservation()
        {
            //Get all reservations from the reservation Identity Map.
            Dictionary<int, Reservation> reservations = reservationIdentityMap.findAll();

            //Get all reservations in the DB
            Dictionary<int, Object[]> result = tdgReservation.getAll();

            // If it's empty, simply return those from the identity map
            if (result == null)
            {
                return reservations;
            }

            //Loop trhough each of the result:
            foreach (KeyValuePair<int, Object[]> record in result)
            {
                //The reservation is not in the reservation identity map.
                //Create an instance, add it to the reservation indentity map and to the return variable

                if (!reservations.ContainsKey(record.Key))
                {

                    Reservation reservation = new Reservation();
                    reservation.reservationID = ((int)record.Key); //reservationID
                    reservation.userID = ((int)record.Value[1]); //userID
                    reservation.roomID = ((int)record.Value[2]); //roomID
                    reservation.description = ((string)record.Value[3]); //desc
                    reservation.date = ((DateTime)record.Value[4]); //date
                    //reservation.date = new DateTime(reservation.reservationDate.Year, reservation.reservationDate.Month, reservation.reservationDate.Day, (int)record.Value[5], 0, 0);//hour
                    reservationIdentityMap.addTo(reservation);
                    reservations.Add(reservation.reservationID, reservation);

                }

            }
            return reservations;

        }

        /**
         * Return all users waiting for a reservation given the
         * reservation ID.
         */
        public List<int> getAllUsers(int reservationID)
        {
            return waitsForMapper.getAllUsers(reservationID);
        }

        /**
         * Set reservation attributes
         **/

        public void modifyReservation(int reservationID, int roomID, string desc, DateTime date)
        {
            //Get the reservation that needs to be updated
            Reservation reservation = getReservation(reservationID);

            //Update the reservation

            //reservation.reservationUserID = (userID); //mutator function to set the NEW userID
            reservation.roomID = (roomID); //mutator function to set the NEW roomID
            reservation.description = (desc); //mutator function to set the NEW description
            reservation.date = (date); //mutator function to set the NEW date
            //reservation.reservationDate = new DateTime(reservation.reservationDate.Year, reservation.reservationDate.Month, reservation.reservationDate.Day, reservation.reservationDate.Hour, 0, 0); //mutator function to set the NEW hour

            //Register instances as Dirty in the Unit Of Work since the object has been modified.
            UnitOfWork.getInstance().registerDirty(reservation);
        }

        /**
         * Delete reservation
         * */

        public void delete(int reservationID)
        {
            //Get the reservation to be deleted by checking the identity map
            Reservation reservation = reservationIdentityMap.find(reservationID);

            //If resrvation IdentityMap returned the object, remove it from identity map
            if (reservation != null)
            {
                reservationIdentityMap.removeFrom(reservation);
            }
            else
            {
                reservation = getReservation(reservationID);
            }
            //Register as deleted in the Unit Of Work. 
            //Object will be deleted from the DB
            UnitOfWork.getInstance().registerDeleted(reservation);

        }
        /**
         * Done: commit
         * When it is time to commit, UoW writes changes to the DB
         * */

        public void done()
        {
            UnitOfWork.getInstance().commit();

        }


        //For Unit of Work: A list of reservations to be added to the DB is passed to the TDG. 
        public void addReservation(List<Reservation> newList)
        {
            tdgReservation.addReservation(newList);

        }

        //For Unit of Work: A list of reservations to be updated in the DB is passed to the TDG.
        public void updateReservation(List<Reservation> updateList)
        {
            tdgReservation.updateReservation(updateList);

        }

        //For Unit of Work : A list of reservation to be deleted in the DB is passes to the TDG.
        public void deleteReservation(List<Reservation> deleteList)
        {
            tdgReservation.deleteReservation(deleteList);
        }


        /**
       * Retrieve all resevation IDs associated with the unique userID & date
       * */
        public List<int> findReservationID(int userID, DateTime date)
        {
            List<int> IDlist = new List<int>();
            IDlist = ReservationIdentityMap.getInstance().findIDs(userID, date);

            if (IDlist == null)
            {
                IDlist = tdgReservation.findById(userID, date);
                if (IDlist == null)
                {
                    return null;
                }
            }
            return IDlist;
        }


    public void createReservation(int userID, int roomID, string desc, DateTime date, int firstHour, int lastHour)
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
                    for (int i = firstHour; i <= lastHour; i++)
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
        udpateDirectories();
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
            for (int i = firstHour; i < lastHour; i++)
                hours.Add(i);

            foreach (Reservation reservation in directoryOfReservations.reservationList)
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
            udpateDirectories();
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










    }


}