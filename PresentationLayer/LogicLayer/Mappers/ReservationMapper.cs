using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDG;
using LogicLayer;


namespace Mappers
{
    class ReservationMapper
    {

        //Instance of this mapper object
        private static ReservationMapper instance = new ReservationMapper();

        private TDGReservation tdgReservation = TDGReservation.getInstance();
        private ReservationIdentityMap reservationIdentityMap = ReservationIdentityMap.getInstance();
        private WaitsForMapper waitsForMapper = WaitsForMapper.getInstance();

        //default constructor
        private ReservationMapper() { }


        public static ReservationMapper getInstance()
        {
            return instance;
        }

        /**
         * Handles the creation of a new object of type Reservation.
         **/

        public Reservation makeNew(int userID, int roomID, string desc, DateTime date, int hour)
        {

            //Make a new reservation object
            Reservation reservation = new Reservation();
            reservation.reservationID = (reservation.GetHashCode());
            reservation.reservationUserID = (userID);
            reservation.reservationRoomID = (roomID);
            reservation.reservationDescription = (desc);
            reservation.reservationDate = (date);
            reservation.reservationDate = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);

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
                result = tdgReservation.fetch(reservationID);

                if (result != null)
                {
                    /**The reservation was obtained from the TDG.
                     * The TDG got (retrieved) reservation from the DB.
                    **/

                    //mapper must add reservation to the ReservationIdentityMap
                    reservation = new Reservation();
                    reservation.reservationID = ((int)result[0]); //reservationID
                    reservation.reservationUserID = ((int)result[1]); //userID
                    reservation.reservationRoomID = ((int)result[2]); //roomID
                    reservation.reservationDescription = ((String)result[3]); //desc
                    reservation.reservationDate = (Convert.ToDateTime(result[4])); //date
                    //reservation.setHour((int)result[5]);//hour
                    reservation.reservationHour = (int)result[5];
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
            Dictionary<int, Object[]> result = tdgReservation.fetchAll();

            //Loop trhough each of the result:
            foreach (KeyValuePair<int, Object[]> record in result)
            {
                //The reservation is not in the reservation identity map.
                //Create an instance, add it to the reservation indentity map and to the return variable

                if (!reservations.ContainsKey(record.Key))
                {

                    Reservation reservation = new Reservation();
                    reservation.reservationID = ((int)record.Key); //reservationID
                    reservation.reservationUserID = ((int)record.Value[1]); //userID
                    reservation.reservationRoomID = ((int)record.Value[2]); //roomID
                    reservation.reservationDescription = ((string)record.Value[3]); //desc
                    reservation.reservationDate = ((DateTime)record.Value[4]); //date
                    reservation.reservationDate = new DateTime(reservation.reservationDate.Year, reservation.reservationDate.Month, reservation.reservationDate.Day, (int)record.Value[5], 0, 0);//hour

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

        public void modifyReservation(int reservationID, int roomID, string desc, DateTime date, int hour)
        {
            //Get the reservation that needs to be updated
            Reservation reservation = getReservation(reservationID);

            //Update the reservation

            //reservation.reservationUserID = (userID); //mutator function to set the NEW userID
            reservation.reservationRoomID = (roomID); //mutator function to set the NEW roomID
            reservation.reservationDescription = (desc); //mutator function to set the NEW description
            reservation.reservationDate = (date); //mutator function to set the NEW date
            reservation.reservationDate = new DateTime (reservation.reservationDate.Year, reservation.reservationDate.Month, reservation.reservationDate.Day, hour, 0, 0); //mutator function to set the NEW hour

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
            waitsForMapper.refreshWaitsFor(newList);
        }

        //For Unit of Work: A list of reservations to be updated in the DB is passed to the TDG.
        public void updateReservation(List<Reservation> updateList)
        {
            tdgReservation.updateReservation(updateList);
            waitsForMapper.refreshWaitsFor(updateList);
        }

        //For Unit of Work : A list of reservation to be deleted in the DB is passes to the TDG.
        public void deleteReservation(List<Reservation> deleteList)
        {
            tdgReservation.deleteReservation(deleteList);
        }

    }
}