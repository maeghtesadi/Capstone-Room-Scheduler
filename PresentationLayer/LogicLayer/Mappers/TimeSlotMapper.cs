using System;
using System.Collections.Generic;
using TDG;
using LogicLayer;
using CapstoneRoomScheduler.LogicLayer.IdentityMaps;

namespace Mappers
{
    class TimeSlotMapper
    {

        //Instance of this mapper object
        private static TimeSlotMapper instance = new TimeSlotMapper();

        private TDGTimeSlot tdgTimeSlot = TDGTimeSlot.getInstance();
        private TimeSlotIdentityMap timeSlotIdentityMap = TimeSlotIdentityMap.getInstance();
        private WaitsForMapper waitsForMapper = WaitsForMapper.getInstance();

        //default constructor
        private TimeSlotMapper() { }


        public static TimeSlotMapper getInstance()
        {
            return instance;
        }

        /**
         * Handles the creation of a new object of type TimeSlot.
         **/

        public TimeSlot makeNew(int reservationID, int hour)
        {

            //Make a new TimeSlot object
            TimeSlot timeslot = new TimeSlot();
            timeslot.timeSlotID = (timeslot.GetHashCode());
            timeslot.hour = (hour);

            //Add new TimeSlot object to the identity map, in Live memory.
            timeSlotIdentityMap.addTo(timeslot);

            //Add TimeSlot object to UoW registry (register as a new TIMESLOT).
            //It will be  created in the DB once the user is ready to commit everything.
            UnitOfWork.getInstance().registerNew(timeslot);

            return timeslot;

        }

        /**
         * Retrieve a TimeSlot given its TimeSlot ID.
         */

        public TimeSlot getTimeSlot(int timeSlotID)
        {

            //Try to obtain the TimeSlot from the TimeSlot indentity map
            TimeSlot timeslot = timeSlotIdentityMap.find(timeSlotID);
            Object[] result = null;


            if (timeslot == null)
            {
                //If not found in TimeSlot identity map then, it uses TDG to try to retrieve from DB.
                result = tdgTimeSlot.get(timeSlotID);

                if (result != null)
                {
                    /**The TimeSlot was obtained from the TDG.
                     * The TDG got (retrieved) TimeSlot from the DB.
                    **/

                    //mapper must add TimeSlot to the TimeSlot IdentityMap
                    timeslot = new TimeSlot();
                    timeslot.timeSlotID = ((int)result[0]); //timeSlotID
                    timeslot.reservationID = ((int)result[1]); //reservationID
                    timeslot.hour = ((int)result[2]); //hour
                    timeSlotIdentityMap.addTo(timeslot);

                }
            }

            //Null is returned if it is not found in the TimeSlot identity map NOR in the DB
            return timeslot;

        }

        /**
         * Retrieve all timeslots
         * */
        public Dictionary<int, TimeSlot> getAllTimeSlot()
        {
            //Get all timeslots from the Time Slot Identity Map.
            Dictionary<int, TimeSlot> timeslots = timeSlotIdentityMap.findAll();

            //Get all timeslots in the DB
            Dictionary<int, Object[]> result = tdgTimeSlot.getAllTimeSlot();

            //Loop through each of the result:
            foreach (KeyValuePair<int, Object[]> record in result)
            {
                //The timeSlot is not in the Time Slot identity map.
                //Create an instance, add it to the Time Slot indentity map and to the return variable
                if (!timeslots.ContainsKey(record.Key))
                {
                    TimeSlot timeslot = new TimeSlot();
                    timeslot.timeSlotID = ((int)record.Key); //timeslotID
                    timeslot.reservationID = ((int)record.Value[1]); //reservationID
                    timeslot.hour = ((int)record.Value[2]); //roomID

                    timeSlotIdentityMap.addTo(timeslot);
                    timeslots.Add(timeslot.timeSlotID, timeslot);
                }
            }
            return timeslots;
        }

        /**
         * Retrieve all timeslots that have the same Reservation ID
         * */
        public Dictionary<int, TimeSlot> getAllTimeSlot(int reservationID)
        {
            //Get all timeslots from the Time Slot Identity Map.
            Dictionary<int, TimeSlot> timeslots = timeSlotIdentityMap.findAll(reservationID);

            //Get all timeslots in the DB
            Dictionary<int, Object[]> result = tdgTimeSlot.getAllTimeSlot(reservationID);

            //Loop through each of the result:
            foreach (KeyValuePair<int, Object[]> record in result)
            {
                //The timeslot is not in the Time Slot identity map.
                //Create an instance, add it to the Time Slot indentity map and to the return variable
                if (!timeslots.ContainsKey(record.Key))
                {
                    TimeSlot timeslot = new TimeSlot();
                    timeslot.timeSlotID = ((int)record.Key); //timeslotID
                    timeslot.reservationID = ((int)record.Value[1]); //reservationID
                    timeslot.hour = ((int)record.Value[2]); //roomID

                    timeSlotIdentityMap.addTo(timeslot);
                    timeslots.Add(timeslot.timeSlotID, timeslot);
                }
            }
            return timeslots;
        }

        /**
         * Return all users waiting for a TimeSlot given the
         * TimeSlot ID.
         */
        public List<int> getAllUsers(int timeSlotID)
        {
            return waitsForMapper.getAllUsers(timeSlotID);
        }

        /**
         * Set time slot attributes
         */
        public void setTimeSlot(int timeSlotID, int reservationID, Queue<int> waitList)
        {
            // Get the room that needs to be updated
            TimeSlot timeSlot = getTimeSlot(timeSlotID);

            // Update the room
            timeSlot.timeSlotID = timeSlotID;
            timeSlot.reservationID = reservationID;
            timeSlot.waitlist = waitList;

            // Register it to the unit of work
            UnitOfWork.getInstance().registerDirty(timeSlot); //someone check pls
        }

        /**
         * Delete timeslot
         * */
        public void delete(int timeSlotID)
        {
            //Get the timeslot to be deleted by checking the identity map
            TimeSlot timeslot = timeSlotIdentityMap.find(timeSlotID);

            //If TimeSlot IdentityMap returned the object, remove it from identity map
            if (timeslot != null)
            {
                timeSlotIdentityMap.removeFrom(timeslot);
            }

            //Register as deleted in the Unit Of Work. 
            //Object will be deleted from the DB
            UnitOfWork.getInstance().registerDeleted(timeslot);

        }
        /**
         * Done: commit
         * When it is time to commit, UoW writes changes to the DB
         * */

        public void done()
        {
            UnitOfWork.getInstance().commit();

        }


        //For Unit of Work: A list of timeslots to be added to the DB is passed to the TDG. 
        public void addTimeSlot(List<TimeSlot> newList)
        {
            tdgTimeSlot.addTimeSlot(newList);
            waitsForMapper.refreshWaitsFor(newList);
        }

        // For Unit of Work: A list of timeslots to be updated to the DB is passed to the TDG. 
        public void updateTimeSlot(List<TimeSlot> updateList)
        {
            tdgTimeSlot.updateTimeSlot(updateList);
            waitsForMapper.refreshWaitsFor(updateList);
        }

        //For Unit of Work : A list of timeslots to be deleted in the DB is passes to the TDG.
        public void deleteTimeSlot(List<TimeSlot> deleteList)
        {
            tdgTimeSlot.deleteTimeSlot(deleteList);
        }


    }
}