using System;
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

        // MySQL Connection
        private MySqlConnection conn;

        // Command object
        private MySqlCommand cmd;

        /**
         * Returns the instance
         */
        public static TDGWaitsFor getInstance()
        {
            return instance;
        }

        // Open the database connection to the database
        public bool openConnection()
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

        // Close the connection to the DB
        public void closeConnection()
        {
            this.conn.Close();
        }

        /**
         * Select all data from the table
         * Returns it as a List<int>
         * Where int is the ID of the object and Object[] contains the record of the row
         */
        public List<int> getAllUsers(int timeSlotID)
        {
            List<int> listOfUsers = new List<int>();

            // Open connection
            openConnection();

            // Write and execute the query
            this.cmd.CommandText = "SELECT " + FIELDS[1] + " FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + timeSlotID + " ORDER BY " + FIELDS[2];
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();

            // If no record is found, return null
            if (!reader.HasRows)
            {
                return null;
            }

            // For each reader, add it to the dictionary
            while (reader.Read())
            {
                listOfUsers.Add((int)reader[0]); // Selecting only the userID
            }

            // Close connection
            closeConnection();

            // Format and return the result
            return listOfUsers;
        }

        /**
         * 
         */
        public void refreshWaitsFor(List<TimeSlot> listOfTimeSlots)
        {

            // Open connection
            openConnection();

            Queue<int> waitlist;
            List<int> results;
            foreach (TimeSlot timeSlot in listOfTimeSlots)
            {
                if (timeSlot.waitlist.Count != 0)
                {
                    // Obtain all queuery for that timeSlot from the database
                    this.cmd.CommandText = "SELECT " + FIELDS[1] + " FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + timeSlot.timeSlotID;
                    this.cmd.Connection = this.conn;
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Store the results
                    results = new List<int>();
                    while (reader.Read())
                    {
                        results.Add((int)reader[0]); // Selecting only the userID
                    }

                    // Get the waitlist of the timeSlot that is refreshed
                    waitlist = timeSlot.waitlist;

                    // If a userID is found in the DB but not in the waitlist: remove from the DB
                    foreach (int userID in results)
                    {
                        if (!waitlist.Contains(userID))
                        {
                            deleteWaitsFor(timeSlot.timeSlotID, userID);
                        }
                    }

                    // If a userID is found in the waitlist but not in the DB: add it to the DB
                    foreach (int userID in waitlist)
                    {
                        if (!results.Contains(userID))
                        {
                            DateTime now = new DateTime();
                            String currentDateTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                            createWaitsFor(timeSlot.timeSlotID, userID, currentDateTime);
                        }
                    }
                }
            }
        }

        private void createWaitsFor(int timeSlotID, int userID, String currentDateTime)
        {
            this.cmd.CommandText = "INSERT INTO " + TABLE_NAME + " VALUES ( " + timeSlotID + "," + userID + "," + currentDateTime + ");";
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();
        }
        private void deleteWaitsFor(int timeSlotID, int userID)
        {
            this.cmd.CommandText = "DELETE FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + timeSlotID + " AND " + FIELDS[1] + " = " + userID;
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();
        }

    }
}
