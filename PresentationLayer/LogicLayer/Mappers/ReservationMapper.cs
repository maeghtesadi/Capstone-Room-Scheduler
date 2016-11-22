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

            lock(this.lockLastID)
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
            IDlist = ReservationIdentityMap.getInstance().findReservationIDs(userID, date);

            if (IDlist == null)
            {
                IDlist = tdgReservation.findByReservationId(userID, date);
                if (IDlist == null)
                {
                    return null;
                }
            }
            return IDlist;
        }

    }
}