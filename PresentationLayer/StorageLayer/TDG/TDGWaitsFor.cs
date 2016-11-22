﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LogicLayer;

namespace TDG
{
    class TDGWaitsFor
    {
        // This instance
        private const String TABLE_NAME = "waitsfor";

        // Table name
        private static TDGWaitsFor instance = new TDGWaitsFor();

        // Field names of the table
        private static readonly String[] FIELDS = { "timeSlotID", "userID", "dateTime" };

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

        /**
         * Returns the instance
         */
        public static TDGWaitsFor getInstance()
        {
            return instance;
        }

        /**
         * Default constructor, private for Singleton
         */
        private TDGWaitsFor()
        {
        }

        /**
         * Select all data from the table
         * Returns it as a List<int>
         * Where int is the ID of the object and Object[] contains the record of the row
         */
        public List<int> getAllUsers(int timeSlotID)
        {
            List<int> listOfUsers = new List<int>();
            String commandLine = "SELECT " + FIELDS[1] + " FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + timeSlotID + " ORDER BY " + FIELDS[2] + ";";
            MySqlDataReader reader = null;
            MySqlConnection conn = new MySqlConnection(DATABASE_CONNECTION_STRING);

            // Open connection
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(commandLine, conn);
                reader = cmd.ExecuteReader();

                // If no record is found, return empty list
                if (!reader.HasRows)
                {
                    reader.Close();
                    conn.Close();
                    return listOfUsers;
                }

                // For each reader, add it to the dictionary
                while (reader.Read())
                {
                    if (reader[0].GetType() == typeof(System.DBNull))
                    {
                        reader.Close();
                        conn.Close();
                        return listOfUsers;
                    }
                    listOfUsers.Add((int)reader[0]); // Selecting only the userID
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                conn.Close();
            }

            // If successful, return the list
            return listOfUsers;
        }

        /**
         * 
         */
        public void refreshWaitsFor(List<TimeSlot> listOfTimeSlots)
        {

            
            foreach (TimeSlot timeSlot in listOfTimeSlots)
            {
                // The list is not empty, refresh the content of the database
                if (timeSlot.waitlist.Count != 0)
                {
                    // Obtain all queuery for that timeSlot from the database
                    String commandLine = "SELECT " + FIELDS[1] + " FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + timeSlot.timeSlotID;
                    MySqlDataReader reader = null;
                    MySqlConnection conn = new MySqlConnection(DATABASE_CONNECTION_STRING);

                    // Try to exeucte the command and read
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(commandLine, conn);
                        reader = cmd.ExecuteReader();

                        // Store the results
                        List<int> results = new List<int>(); // Will store the results found after querying the database
                        while (reader.Read())
                        {
                            results.Add((int)reader[0]); // Selecting only the userID
                        }
                        reader.Close();

                        // Get the waitlist of the timeSlot that is refreshed
                        Queue<int>  waitlist = timeSlot.waitlist;

                        // If a userID is found in the DB but not in the waitlist: remove from the DB
                        foreach (int userID in results)
                        {
                            if (!waitlist.Contains(userID))
                            {
                                deleteWaitsFor(conn, timeSlot.timeSlotID, userID);
                            }
                        }

                        // If a userID is found in the waitlist but not in the DB: add it to the DB
                        foreach (int userID in waitlist)
                        {
                            if (!results.Contains(userID))
                            {
                                String currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                createWaitsFor(conn, timeSlot.timeSlotID, userID, currentDateTime);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                        conn.Close();
                    }

                }
                // If the queue is empty, ensure it is empty by deleting all rows that have that timeslot id
                else
                {
                    String commandLine = "DELETE FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + " = " + timeSlot.timeSlotID;
                    MySqlConnection conn = new MySqlConnection(DATABASE_CONNECTION_STRING);
                    MySqlDataReader reader = null;

                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(commandLine, conn);
                        reader = cmd.ExecuteReader();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if(reader!=null)
                            reader.Close();
                        conn.Close();
                    }
                }
            }
        }

        private void createWaitsFor(MySqlConnection conn, int timeSlotID, int userID, String currentDateTime)
        {
            String commandLine = "INSERT INTO " + TABLE_NAME + " VALUES ( " + timeSlotID + "," + userID + ", '" + currentDateTime + "');";
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand(commandLine, conn);
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
                if(reader != null)
                    reader.Close();
            }
        }
        private void deleteWaitsFor(MySqlConnection conn, int timeSlotID, int userID)
        {
            String commandLine = "DELETE FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + timeSlotID + " AND " + FIELDS[1] + " = " + userID;
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand(commandLine, conn);
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
                if(reader!=null)
                    reader.Close();
            }
        }
    }
}
