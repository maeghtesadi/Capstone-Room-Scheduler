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

    public class TDGTimeSlot
    {
        //This instance

        private static TDGTimeSlot instance = new TDGTimeSlot();

        //Table name
        private const String TABLE_NAME = "timeslot";

        //Fields names of the table
        private readonly String[] FIELDS = { "timeslotID", "reservationID", "hour" };

        //Database server (localhost)
        private const String DATABASE_SERVER = "127.0.0.1";

        //Database to which we will connect
        private const String DATABASE_NAME = "reservation_system";

        //Credentials to connect to the databas
        private const String DATABASE_UID = "root";
        private const String DATABASE_PWD = "";

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

        public static TDGTimeSlot getInstance()
        {
            return instance;
        }

        /**
         * Default Constructor
         */

        private TDGTimeSlot()
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
         * Add new timeslot(s) to the database
         */
        public void addTimeSlot(List<TimeSlot> newList)
        {
            if (!openConnection())
                return;
            for (int i = 0; i < newList.Count; i++)
            {
                createTimeSlot(newList[i]);
            }
            closeConnection();
        }

        /**
         * Add new timeslot(s) to the database
         */
        public void updateTimeSlot(List<TimeSlot> updateList)
        {
            if (!openConnection())
                return;
            for (int i = 0; i < updateList.Count; i++)
            {
                updateTimeSlot(updateList[i]);
            }
            closeConnection();
        }

        /**
         * Delete timeSlot(s) from the databas
         *
         * */

        public void deleteTimeSlot(List<TimeSlot> deleteList)
        {
            if (!openConnection())
                return;
            for (int i = 0; i < deleteList.Count; i++)
            {
                removeTimeSlot(deleteList[i]);
            }
            closeConnection();
        }


        /**
         * Returns a record for the timeslot given its timeslotID
         */

        public Object[] get(int timeslotID)
        {
            //Open connection
            if (!openConnection())
                return null;

            //Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + " = " + timeslotID;
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = null;
            Object[] record = null; // to be returned

            try
            {
                reader = cmd.ExecuteReader();
                //If no record is found, return null
                if (!reader.HasRows)
                {
                    return null;
                }
                //There is only one result since we find it by id
                record = new Object[FIELDS.Length];
                while (reader.Read())
                {
                    if (reader[0].GetType() == typeof(System.DBNull))
                    {
                        return null;
                    }
                    record[0] = reader[0]; // timeslotID
                    record[1] = reader[1]; // reservationID
                    record[2] = reader[2]; // dateTime

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //Close connection
                reader.Close();
                closeConnection();
            }

            //Format and return the result
            return record;
        }


        /**
         * Select all data from the table
         * Returns it as a Dictionary <int, Object[]>
         * Where int is the ID of the object and Object[] contains the record of the row
         * */

        public Dictionary<int, Object[]> getAllTimeSlot()
        {
            Dictionary<int, Object[]> records = new Dictionary<int, Object[]>();
            //Open Connection
            if (!openConnection())
                return records;

            //Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE 1;";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();

                //If no record is found, return empty records
                if (!reader.HasRows)
                {
                    return records;

                }

                //For each reader, add it to the dictionary
                while (reader.Read())
                {
                    if (reader[0].GetType() == typeof(System.DBNull))
                    {
                        return records;
                    }
                    Object[] attributes = new Object[FIELDS.Length];
                    attributes[0] = reader[0]; // timeslotID
                    attributes[1] = reader[1]; // reservationID
                    attributes[2] = reader[2]; // hour

                    records.Add((int)reader[0], attributes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //close connection
                reader.Close();
                closeConnection();
            }

            //Format and return the result
            return records;
        }

        /**
         * Select all data from the table
         * Returns it as a Dictionary <int, Object[]>
         * Where int is the ID of the object and Object[] contains the record of the row
         * For a given reservation ID
         * */

        public Dictionary<int, Object[]> getAllTimeSlot(int reservationID)
        {
            Dictionary<int, Object[]> records = new Dictionary<int, Object[]>();
            //Open Connection
            if (!openConnection())
                return records;

            //Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + FIELDS[1] + " = " + reservationID;
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();
                //If no record is found, return empty records
                if (!reader.HasRows)
                {
                    return records;

                }

                //For each reader, add it to the dictionary
                while (reader.Read())
                {
                    if (reader[0].GetType() == typeof(System.DBNull))
                    {
                        return records;
                    }
                    Object[] attributes = new Object[FIELDS.Length];
                    attributes[0] = reader[0]; // timeslotID
                    attributes[1] = reader[1]; // reservationID
                    attributes[2] = reader[2]; // hour

                    records.Add((int)reader[0], attributes);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //close connection
                reader.Close();
                closeConnection();
            }

            //Format and return the result
            return records;
        }

        /**
         * Adds one timeslot to the database
         * */
        private void createTimeSlot(TimeSlot timeslot)
        {
            this.cmd.CommandText = "INSERT INTO " + TABLE_NAME + " VALUES (" + timeslot.timeSlotID + "," +
                timeslot.reservationID + "," + timeslot.hour + ");";

            this.cmd.Connection = this.conn;
            
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                reader.Close();
            }
        }


        /**
         * Updates one timeslot to the database
         * */
        private void updateTimeSlot(TimeSlot timeslot)
        {
            this.cmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + FIELDS[1] + " = " + timeslot.reservationID + " WHERE " + FIELDS[0] + " = " + timeslot.timeSlotID;
            this.cmd.Connection = this.conn;

            MySqlDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                reader.Close();
            }
        }

        /**
         * Removes one timeslot from the database
         * */

        private void removeTimeSlot(TimeSlot timeslot)
        {
            this.cmd.CommandText = "DELETE FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + timeslot.timeSlotID + ";";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                reader.Close();
            }
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
            while (reader.Read())
            {
                if(reader[0].GetType() != typeof(System.DBNull))
                {
                    lastID = (int)reader[0];
                }
            }

            // Close connection
            reader.Close();
            closeConnection();

            // return the last id
            return lastID;
        }

        /**
         * Get the summed up values of hours for the list of reservation IDs
         */
        public int getTotalHoursforReservationID(List<int> IDlist)
        {
            List<Object> hours = new List<Object>();
            //Open connection
            openConnection();

            foreach (int reservationID in IDlist)
            {
                //Write and execute the query
                this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + FIELDS[1] + " = " + reservationID;
                this.cmd.Connection = this.conn;
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    hours.Add(reader[2]);
                }
            }
            return hours.Count;
        }

    }
}
