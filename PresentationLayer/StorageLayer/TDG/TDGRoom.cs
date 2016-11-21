﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LogicLayer;


namespace TDG
{
    /**
     * Class: TDGRoom
     * 
     * Table data gateway of Room table
     * 
     * This class acts as a bridge from the software application to the database.
     * It allows to create, update, delete and find data from the room database table.
     */

    public class TDGRoom
    {
        // This instance
        private static TDGRoom instance = new TDGRoom();

        // Table name
        private const String TABLE_NAME = "room";

        // Field names of the table
        private readonly String[] FIELDS = { "RoomID", "RoomNum" };

        // Database server (localhost)
        private const String DATABASE_SERVER = "127.0.0.1";

        // Database to which we will connect
        private const String DATABASE_NAME = "reservation_system";

        // Credentials to connect to the database
        private const String DATABASE_UID = "root";
        private const String DATABASE_PWD = "";

        // The whole connection string used to connect to the database
        // In our case, we will always connect to the same database, thus it can
        // be defined as a constant. But we can always change it.
        private const String DATABASE_CONNECTION_STRING = "server=" + DATABASE_SERVER + ";uid=" + DATABASE_UID + ";pwd=" + DATABASE_PWD + ";database=" + DATABASE_NAME + ";";

        // Determine after how much time a command (query) should be timed out
        private const int COMMAND_TIMEOUT = 60;

        // MySQL Connection
        private MySqlConnection conn;

        // Command object
        private MySqlCommand cmd;


        /**
         * Returns the instance
         */
        public static TDGRoom getInstance()
        {
            return instance;
        }

        /**
         * Default constructor
         */
        private TDGRoom()
        {
            this.cmd = new MySqlCommand();
            this.cmd.CommandTimeout = COMMAND_TIMEOUT;
        }

        /**
         * Open the database connection to the database
         */
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
         * Close the connection to the database
         */
        public void closeConnection()
        {
            this.conn.Close();
        }



        /**
         * Add new rooms to the database
         */
        public Boolean addRoom(List<Room> newList)
        {
            if (!openConnection())
                return false;
            for (int i = 0; i < newList.Count; i++)
            {
                if(!createRoom(newList[i]))
                {
                    closeConnection();
                    return false;
                }
                    
            }
            closeConnection();
            return true;
        }

        /**
         * Update rooms of the database
         */
        public Boolean updateRoom(List<Room> updateList)
        {
            if (!openConnection())
                return false;
            for (int i = 0; i < updateList.Count; i++)
            {
                if (!updateRoom(updateList[i]))
                {
                    closeConnection();
                    return false;
                }
                    
            }
            closeConnection();
            return true;
        }

        /**
         * Delete room(s) from the database
         */
        public Boolean deleteRoom(List<Room> deleteList)
        {
            if (!openConnection())
                return false;
            for (int i = 0; i < deleteList.Count; i++)
            {
                if(!removeRoom(deleteList[i]))
                {
                    closeConnection();
                    return false;
                }

            }
            closeConnection();
            return true;
        }

        /**
         * Returns a record for the room given its roomID
         */
        public Object[] get(int roomID)
        {
            // Open connection
            if (!openConnection())
                return null;

            // Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + " = " + roomID;
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = null;
            Object[] record = null; // to be returned

            bool successful = true;

            try
            {
                reader = cmd.ExecuteReader();

                // If no record is found, return null
                if (!reader.HasRows)
                {
                    return null;
                }

                // There is only one result since we find it by id
                record = new Object[FIELDS.Length];
                while (reader.Read())
                {
                    if (reader[0].GetType() == typeof(System.DBNull))
                    {
                        return null;
                    }
                    record[0] = reader[0];
                    record[1] = reader[1];
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                successful = false;
            }
            finally
            {
                // Close reader
                reader.Close();
                // Close connection
                closeConnection();
            }


            // Format and return the result
            if (successful)
                return record;
            else
                return null;
        }

        /**
         * Select all data from the table
         * Returns it as a Dictionary<int, Object[]>
         * Where int is the ID of the object and Object[] contains the record of the row
         */
        public Dictionary<int, Object[]> getAll()
        {
            Dictionary<int, Object[]> records = new Dictionary<int, Object[]>();
            // Open connection
            if (!openConnection())
                return null;

            // Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE 1;";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = null;
            bool successful = true;

            try
            {
                reader = cmd.ExecuteReader();

                // If no record is found, return null
                if (!reader.HasRows)
                {
                    return null;
                }

                // For each reader, add it to the dictionary
                while (reader.Read())
                {
                    if (reader[0].GetType() == typeof(System.DBNull))
                    {
                        return null;
                    }
                    Object[] attributes = new Object[FIELDS.Length];
                    attributes[0] = reader[0]; // roomID
                    attributes[1] = reader[1]; // roomNum
                    records.Add((int)reader[0], attributes);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                successful = false;
            }
            finally
            {
                // Close reader
                reader.Close();
                // Close connection
                closeConnection();
            }

            // Format and return the result
            if (successful)
                return records;
            else
                return null;
        }

        /**
         * Adds one room to the database
         */
        private Boolean createRoom(Room room)
        {
            this.cmd.CommandText = "INSERT INTO " + TABLE_NAME + " VALUES (" + room.roomID + ",'" + room.roomNum + "');";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = null;
            bool successful = true;
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                successful = false;
            }
            finally
            {
                reader.Close();
            }
            return successful;
        }

        /**
         * Updates one room of the database
         */
        private Boolean updateRoom(Room room)
        {
            this.cmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + FIELDS[1] + "= '" + room.roomNum + "' WHERE " + FIELDS[0] + " = " + room.roomID + ";";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = null;
            bool successful = true;

            try
            {
                reader = cmd.ExecuteReader();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                successful = false;
            }
            finally
            {
                reader.Close();
            }
            return successful;
        }

        /**
         * Removes one room from the database
         */
        private Boolean removeRoom(Room room)
        {
            this.cmd.CommandText = "DELETE FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + room.roomID + ";";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = null;
            bool successful = true;

            try
            {
                reader = cmd.ExecuteReader();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                successful = false;
            }
            finally
            {
                reader.Close();
            }
            return successful;
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
                if (reader[0].GetType() != typeof(System.DBNull))
                {
                    lastID = (int)reader[0];
                }
            }

            // Close connection
            closeConnection();
            reader.Close();
            // return the last id
            return lastID;
        }
    }
}
