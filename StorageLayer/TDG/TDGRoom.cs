using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LogicLayer;


namespace StorageLayer
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
         * Constructor taking the name of the table
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
        public void addRoom(List<Room> newList)
        {
            openConnection();
            for (int i = 0; i < newList.Count; i++)
            {
                createRoom(newList[i]);
            }
            closeConnection();
        }

        /**
         * Update rooms of the database
         */
        public void updateRoom(List<Room> updateList)
        {
            openConnection();
            for (int i = 0; i < updateList.Count; i++)
            {
                updateRoom(updateList[i]);
            }
            closeConnection();
        }

        /**
         * Delete room(s) from the database
         */
        public void deleteRoom(List<Room> deleteList)
        {
            openConnection();
            for (int i = 0; i < deleteList.Count; i++)
            {
                removeRoom(deleteList[i]);
            }
            closeConnection();
        }

        /**
         * Returns a record for the room given its roomID
         */
        public List<Object[]> fetch(int roomID)
        {
            // Open connection
            openConnection();

            // Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + " = " + roomID;
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();

            // Close connection
            closeConnection();

            // Format and return the result
            return readerToListOfArrayOfObject(reader);
        }

        /**
         * Select all data from the table, returns as MySqlDataReader object, but we can
         * always format it differently to ensure that only the TDG is related to database.
         * I.e. we can return a big 2D array instead of the object.
         */
        public List<Object[]> fetchAll()
        {
            // Open connection
            openConnection();

            // Write and execute the query
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE 1;";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();

            // Close connection
            closeConnection();

            // Format and return the result
            return readerToListOfArrayOfObject(reader);
        }

        /**
         * Adds one room to the database
         */ 
        private void createRoom(Room room)
        {
            this.cmd.CommandText = "INSERT INTO " + TABLE_NAME + " VALUES (" + room.getRoomID() + "," + room.getRoomNum() + ");";
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();
        }

        /**
         * Updates one room of the database
         */
        private void updateRoom(Room room)
        {
            this.cmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + FIELDS[1] + "=" + room.getRoomNum() + " WHERE " + FIELDS[0] + " = " + room.getRoomID() + ";";
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();
        }

        /**
         * Removes one room from the database
         */
        private void removeRoom(Room room)
        {
            this.cmd.CommandText = "DELETE FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + room.getRoomID() + ";";
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();
        }

        /**
         * Formats the result of a fetch into a list of array of object
         * Returns null if no rows are found for the given reader
         */
        private List<Object[]> readerToListOfArrayOfObject(MySqlDataReader reader)
        {
            // If the reader has no rows, return null
            if(!reader.HasRows)
            {
                return null;
            }

            // Create the list to be returned
            List<Object[]> result = new List<Object[]>();

            // As long as there is a row to be read:
            //  Create a new array of object
            //  Assigned the attribute to the appropriate index
            //  Add the array of object to the list
            while (reader.Read())
            {
                Object[] toAdd = new Object[FIELDS.Length];
                toAdd[0] = reader[0];
                toAdd[1] = reader[1];
                result.Add(toAdd);
            }
            
            return result;
        }  
    }
}
