using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
// In order for MySql to be found, install the package:
// Tools > NuGet Package Manager > Package Manager Console
// Install-Package MySql.Data


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
      
    class TDGRoom
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
        public Boolean OpenConnection()
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
        public void CloseConnection()
        {
            this.conn.Close();
        }



        /**
         * Add new rooms to the database
         */ 
        public void addRoom(List<Room> newList)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                addRoom(newList[i]);
            }
        }

        /**
         * Update rooms of the database
         */
        public void updateRoom(List<Room> updateList)
        {
            for (int i = 0; i < updateList.Count; i++)
            {
                updateRoom(updateList[i]);
            }
        }

        /**
         * Delete room(s) from the database
         */
        public void removeRoom(List<Room> removeList)
        {
            for (int i = 0; i < removeList.Count; i++)
            {
                removeRoom(removeList[i]);
            }
        }

        /**
         * Returns a record for the room given its roomID
         */
        public MySqlDataReader fetch(int roomID)
        {
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + " = " + roomID;
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        /**
        * Select all data from the table, returns as MySqlDataReader object, but we can
        * always format it differently to ensure that only the TDG is related to database.
        * I.e. we can return a big 2D array instead of the object.
        */
        public MySqlDataReader fetchAll()
        {
            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE 1;";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        /**
         * Adds one room to the database
         */ 
        private void addRoom(Room room)
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
    }
}
