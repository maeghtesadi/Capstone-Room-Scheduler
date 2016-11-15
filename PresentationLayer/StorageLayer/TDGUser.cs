﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
//using LogicLayer;

//namespace StorageLayer
//{
//    /**
//  * Class: TDGUser
//  * 
//  * Table data gateway of User table
//  * 
//  * This class acts as a bridge from the software application to the database.
//  * It allows to create, update, delete and find data from the user database table.
//  */
//    public class TDGUser
//    {
//        // This instance
//        private const String TABLE_NAME = "user";

//        // Table name
//        private static TDGUser instance = new TDGUser();

//        // Field names of the table
//        private static readonly string[] FIELDS = { "userID", "username", "password", "name", "numOfReservations" };
        
//        // Database server (localhost)
//        private const String DATABASE_SERVER = "127.0.0.1";

//        // Database to which we will connect
//        private const String DATABASE_NAME = "reservation_system";

//        // Credentials to connect to the database
//        private const String DATABASE_UID = "root";
//        private const String DATABASE_PWD = "";

//        // The whole connection string used to connect to the database
//        // In our case, we will always connect to the same database, thus it can
//        // be defined as a constant. But we can always change it.
//        private const String DATABASE_CONNECTION_STRING = "server=" + DATABASE_SERVER + ";uid=" + DATABASE_UID + ";pwd=" + DATABASE_PWD + ";database=" + DATABASE_NAME + ";";

//        // Determine after how much time a command (query) should be timed out
//        private const int COMMAND_TIMEOUT = 60;

//        // MySQL Connection
//        private MySqlConnection conn;

//        // Command object
//        private MySqlCommand cmd;

//        // Constructor
//        public TDGUser()
//        {
//            this.cmd = new MySqlCommand();
//            this.cmd.CommandTimeout = COMMAND_TIMEOUT;
//        }

//        public static TDGUser getInstance()
//        {
//            return instance;
//        }

//        // Open the database connection to the database
//        public bool openConnection()
//        {
//            try
//            {
//                this.conn = new MySqlConnection(DATABASE_CONNECTION_STRING);
//                this.conn.Open();
//                return true;
//            }
//            catch (MySqlException ex)
//            {
//                Console.WriteLine(ex.Message);
//                return false;
//            }
//        }

//        // Close the connection to the DB
//        public void closeConnection()
//        {
//            this.conn.Close();
//        }

//        /**
//       * Returns a record for the user given its userID
//       */
//        public Object[] fetch(int userID)
//        {
//            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " \n" +
//                    "WHERE " + FIELDS[0] + "=" + userID + ";";
//            this.cmd.Connection = this.conn;
//            MySqlDataReader reader = cmd.ExecuteReader();

//            // If no record is found, return null
//            if (!reader.HasRows)
//            {
//                return null;
//            }
//            // There is only one result since we find it by id
//            Object[] record = new Object[FIELDS.Length];
//            while (reader.Read())
//            {
//                record[0] = reader[0];
//                record[1] = reader[1];
//                record[2] = reader[2];
//                record[3] = reader[3];
//                record[4] = reader[4];
//            }
//            // Close connection
//            closeConnection();

//            // Format and return the result
//            return record;
//        }

//        /**
//         * Select all data from the table
//         * Returns it as a Dictionary<int, Object[]>
//         * Where int is the ID of the object and Object[] contains the record of the row
//         */
//        public Dictionary<int, Object[]> fetchAll()
//        {
//            Dictionary<int, Object[]> records = new Dictionary<int, Object[]>();

//            // Open connection
//            openConnection();

//            // Write and execute the query
//            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE 1;";
//            this.cmd.Connection = this.conn;
//            MySqlDataReader reader = cmd.ExecuteReader();

//            // If no record is found, return null
//            if (!reader.HasRows)
//            {
//                return null;
//            }

//            // For each reader, add it to the dictionary
//            while (reader.Read())
//            {
//                Object[] attributes = new Object[FIELDS.Length];
//                attributes[0] = reader[0]; // userID
//                attributes[1] = reader[1]; // userName
//                attributes[2] = reader[2]; // password
//                attributes[3] = reader[3]; // name
//                attributes[4] = reader[4]; // numOfReservations
//                records.Add((int)reader[0], attributes);
//            }

//            // Close connection
//            closeConnection();

//            // Format and return the result
//            return records;
//        }
//    }
//}