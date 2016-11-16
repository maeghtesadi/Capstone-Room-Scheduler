using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using LogicLayer;
using MySql.Data.MySqlClient;


namespace TDG
{

    public class TDGReservation
    {
        //This instance

        private static TDGReservation instance = new TDGReservation();

        //Table name
        private const String TABLE_NAME = "reservation";

        //Fields names of the table
        private readonly String[] FIELDS = { "reservationID", "userID", "roomID", "desc", "date" }; 

        //Database server (localhost)
        private const String DATABASE_SERVER = "127.0.0.1";

        //Database to which we will connect
        private const String DATABASE_NAME = "reservation_system";

        //Credentials to connect to the databas
        private const String DATABASE_UID = "root";
        private const String DATABASE_PWD = " ";

        //The whole connection string used to connect to the database
        //In our case, we will always connect to the same database, this it can
        //be defined as a constant. But we can always change it.
        private const String DATABASE_CONNECTION_STRING = "server=" + DATABASE_SERVER + ";uid=" + DATABASE_UID + ";pwd=" + DATABASE_PWD + ";database=" + DATABASE_NAME + ";";

        //Determine after how much time a command (query) should be timed out
        private const int COMMAND_TIMEOUT = 60;

        //MySQL Connection
        private MySqlConnection conn;

        //Command object
        private MySqlCommand cmd;

        /**
         * Returns the instance
         * */

        public static TDGReservation getInstance()
        {
            return instance;
        }

        /**
         * Default Constructor
         */

        private TDGReservation()
        {
            this.cmd = new MySqlCommand();
            this.cmd.CommandTimeout = COMMAND_TIMEOUT;

        }

        /**
         * Open connection to the database
         * */
        public Boolean openConnection()
        {
            try
            {
                this.conn = new MySqlConnection(DATABASE_CONNECTION_STRING);
                this.conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;

            }
        }


        /**
         * Close connection to the database
         * */

        public void closeConnection()
        {
            this.conn.Close();
        }


        /**
         * Add new reservations to the database
         * */


        public void addReservation(List<Reservation> newList)
        {
            openConnection();
            for (int i = 0; i < newList.Count; i++)
            {
                createReservation(newList[i]);
            }
            closeConnection();

        }


        /**
         * Update reservations of the database
         * */

        public void updateReservation(List<Reservation> updateList)
        {
            openConnection();
            for (int i = 0; i < updateList.Count; i++)
            {
                updateReservation(updateList[i]);
            }
            closeConnection();
        }


        /**
         * Delete reservation(s) from the databas
         *
         * */

        public void deleteReservation(List<Reservation> deleteList)
        {
            openConnection();
            for (int i = 0; i < deleteList.Count; i++)
            {
                removeReservation(deleteList[i]);

            }
            closeConnection();

        }

        /**
         * Returns a record for the reservation given its reservationID
         * */

        public Object[] get(int reservationID)
        {
            //Open connection
            openConnection();

            //Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + "WHERE" + FIELDS[0] + " = " + reservationID;
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();


            //If no record is found, return null
            if (!reader.HasRows)
            {
                return null;
            }

            //There is only one result since we find it by id
            Object[] record = new Object[FIELDS.Length];
            while (reader.Read())
            {
                record[0] = reader[0];
                record[1] = reader[1];

            }
            //Close connection
            closeConnection();

            //Format and return the result
            return record;
        }


        /**
         * Select all data from the table
         * Returns it as a Dictionary <int, Object[]>
         * Where int is the ID of the object and Object[] contains the record of the row
         * */

        public Dictionary<int, Object[]> getAll()
        {
            Dictionary<int, Object[]> records = new Dictionary<int, Object[]>();
            //Open Connection
            openConnection();

            //Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + "WHERE 1;";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();

            //If no record is found, return null
            if (!reader.HasRows)
            {
                return null;

            }

            //For each reader, add it to the dictionary
            while (reader.Read())
            {
                Object[] attributes = new Object[FIELDS.Length];
                attributes[0] = reader[0]; //reservationID
                attributes[1] = reader[1]; // userID
                attributes[2] = reader[2]; //roomID
                attributes[3] = reader[3]; //desc
                attributes[4] = reader[4]; //date
             

                records.Add((int)reader[0], attributes);


            }

            //close connection
            closeConnection();

            //Format and return the result
            return records;
        }

        /**
         * Adds one reservation to the database
         * */
        private void createReservation(Reservation reservation)
        {
            this.cmd.CommandText = "INSERT INTO " + TABLE_NAME + " VALUES (" + reservation.reservationID + "," +
                reservation.reservationUserID + "," + reservation.reservationRoomID + "," + reservation.reservationDescription + "," +
                reservation.reservationDate +");";

            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();
        }

        /**
         * Updates one reservation of the database
         * */

        private void updateReservation(Reservation reservation)
        {

            this.cmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + 
                FIELDS[4] + " = " + reservation.reservationDate + "," + FIELDS[3] + " = " + reservation.reservationDescription + "," +
                FIELDS[2] + " = " + reservation.reservationRoomID + "," + FIELDS[1] + "=" + reservation.reservationUserID + " WHERE " +
                FIELDS[0] + " = " + reservation.reservationID + ";";
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();
        }


        /**
         * Removes one reservation from the database
         * */

        private void removeReservation(Reservation reservation)
        {
            this.cmd.CommandText = "DELETE FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + reservation.reservationID + ";";
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();
        }




    }
}
