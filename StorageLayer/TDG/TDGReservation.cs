using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;

namespace StorageLayer
{
    class TDGReservation
    {

        // This instance
        private static TDGReservation instance = new TDGReservation();

        // Table name
        private const String TABLE_NAME = "reservation";

        // Field names of the table
        private readonly String[] FIELDS = { "reservationID", "UserID", "roomID" , "description", "date", "hour"};

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


         

        public static  TDGReservation getInstances()
        {
            return instance;
        }

        //open the connection to the DB
        public void openConnection()
        {
            Console.WriteLine("Connection to DB is opened");
        }

        //close the connection to the DB
        public void closeConnection()
        {
            Console.WriteLine("Connection to DB is closed");

        }

        //Adds a list of reservations to the DB
        public void addReservation(List<Reservation> newList)
        {

            openConnection();
            for (int i = 0; i < newList.Count(); i++)
            {
                Console.WriteLine("Saving new Reservation" + newList[i].getReservationID() + 
                    newList[i].getRoomID() + newList[i].getDescription() + newList[i].getDate() + newList[i].getHour() + " to the DB.");

                createNewReservation(newList[i]);



            }
            closeConnection();
        }

        // Updates a list of reservations in the DB
        public void updateReservation(List<Reservation> updateList)
        {

            openConnection();

            for (int i = 0; i < updateList.Count(); i++)
            {
                Console.WriteLine("Modifiing existing Client " + updateList[i].getReservationID() +
                    updateList[i].getRoomID() + updateList[i].getDescription() + updateList[i].getDate() + updateList[i].getHour() + " to the DB."); 
                updateReservation(updateList[i]);
            }

            closeConnection();
        }



        // Removes a list of reservations from the DB
        public void deleteReservation(List<Reservation> deleteList)
        {

            openConnection();

            for (int i = 0; i < deleteList.Count(); i++)
            {
                Console.WriteLine("Removing existing Client " + deleteList[i].getReservationID() +
                    deleteList[i].getRoomID() + deleteList[i].getDescription() + deleteList[i].getDate() + deleteList[i].getHour() + " from the DB.");
                removeReservation(deleteList[i]);
            }

            closeConnection();
        }



        /**
         * Returns a record for the room given its reservationID
         */
        public MySqlDataReader fetch(int reservationID)
        {
            openConnection();


            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + " = " + reservationID;

            closeConnection();
        }
        /**
        * Select all data from the table, returns as MySqlDataReader object, but we can
        * always format it differently to ensure that only the TDG is related to database.
        * I.e. we can return a big 2D array instead of the object.
        */
        public MySqlDataReader fetchAll()
        {
            openConnection();

            this.cmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE 1;";
            this.cmd.Connection = this.conn;
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader;

            closeConnection();
        }


        // SQL Statement to create a new Reservation/Row.
        public void createNewReservation(Reservation reservation)
        {
            
                      
            this.cmd.CommandText = "INSERT INTO " + TABLE_NAME + " VALUES (" + reservation.getReservationID() + "," + reservation.getUser() + ","
                    + reservation.roomID() + "," + reservation.getDescription() + "," +  reservation.getDate() + "," + reservation.getHour() + ");";
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();


        }




        // SQL Statement to update an exisiting Client/Row
        public void updateClient(Reservation reservation)
        {
            
            this.cmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + FIELDS[1] + "=" + reservation.getReservationID() + "," + reservation.getUser() + ","
                    + reservation.roomID() + "," + reservation.getDescription() + "," + reservation.getDate() + "," + reservation.getHour() + ";";
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();


        }


        // SQL Statement to delete an existing Client/Row.
        public void removeClient(Reservation reservation)
        {
            
            this.cmd.CommandText = "DELETE FROM " + TABLE_NAME + " WHERE " + FIELDS[0] + "=" + reservation.getReservationID() + "," + reservation.getUser() + ","
                    + reservation.roomID() + "," + reservation.getDescription() + reservation.getDate() + reservation.getHour() + ";";
            this.cmd.Connection = this.conn;
            cmd.ExecuteReader();


        }

       















    }
}
