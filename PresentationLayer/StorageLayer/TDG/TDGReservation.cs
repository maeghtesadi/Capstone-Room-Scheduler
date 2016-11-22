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
        private readonly String[] FIELDS = { "reservationID", "userID", "roomID", "description", "date" }; 

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
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + " = " + reservationID;
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
                if(reader[0].GetType() == typeof(System.DBNull))
                {
                    return null;
                }

                record[0] = reader[0];
                record[1] = reader[1];
                record[2] = reader[2];
                record[3] = reader[3];
                record[4] = reader[4];

            }
            reader.Close();
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
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE 1;";
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
                if(reader[0].GetType() == typeof(System.DBNull))
                {
                    return null;
                }

                Object[] attributes = new Object[FIELDS.Length];
                attributes[0] = reader[0]; //reservationID
                attributes[1] = reader[1]; // userID
                attributes[2] = reader[2]; //roomID
                attributes[3] = reader[3]; //desc
                attributes[4] = reader[4]; //date
             

                records.Add((int)reader[0], attributes);


            }
            reader.Close();
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
            String mySqlDate = reservation.date.Date.ToString("yyyy-MM-dd");
            this.cmd.CommandText = "INSERT INTO " + TABLE_NAME + " VALUES (" + reservation.reservationID + "," +
                reservation.userID + "," + reservation.roomID + ",'" + reservation.description + "', '" + 
                mySqlDate + " ');";

            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
        }

        /**
         * Updates one reservation of the database
         * */

        private void updateReservation(Reservation reservation)
        {
            String mySqlDate = reservation.date.Date.ToString("yyyy-MM-dd");
            this.cmd.CommandText = "UPDATE " + TABLE_NAME + " SET " +
                FIELDS[4] + " = '" + mySqlDate + "', " + FIELDS[3] + " = '" + reservation.description + "', " +
                FIELDS[2] + " = " + reservation.roomID + ", " + FIELDS[1] + " = " + reservation.userID + " WHERE " +
                FIELDS[0] + " = " + reservation.reservationID + ";";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
        }


        /**
         * Removes one reservation from the database
         * */

        private void removeReservation(Reservation reservation)
        {
            this.cmd.CommandText = "DELETE FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + reservation.reservationID + ";";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
        }

        /**
         * Get the last ID that was entered
         */
        public int getLastID()
        {
            // lastID to be returned
            int lastID = 0;
            openConnection();
                
            // Get the max id from database
            this.cmd.CommandText = "SELECT MAX(" + FIELDS[0] + ") FROM " + TABLE_NAME;
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();

            // read it, there should only be one
            while(reader.Read())
            {
                if (reader[0].GetType() != typeof(System.DBNull))
                {
                    lastID = (int)reader[0];
                }
            }
            reader.Close();
            // Close connection
            closeConnection();

            // return the last id
            return lastID;
        }

        /**
         * Get the list of reservation IDs associated with the userID at a specific day
         */
        public List<int> getReservationIDs(int userID, DateTime date)
        {
            List<int> IDlist = new List<int>();
            //Open connection
            openConnection();

            //Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE" + FIELDS[1] + "=" + userID + ";";
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE" + FIELDS[4] + "=" + date.Date.ToString("yyyy-MM-dd") + ";";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                return null;
            }

            //For each reader, add it to the dictionary
            while (reader.Read())
            {
                Object[] attributes = new Object[FIELDS.Length];
                attributes[4] = reader[4];
                IDlist.Add(Convert.ToInt32(reader[4]));
            }

            reader.Close();
            //close connection
            closeConnection();

            //Format and return the result
            return IDlist;
        }
    }
}
