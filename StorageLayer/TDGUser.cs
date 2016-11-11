using MySql.Data.MySqlClient;

public class TDGUser {
    private static TDGUser instance = new TDGUser();

    private static readonly string[] fields = {'userID','username','password','name','numOfReservations'};
    // Database server (localhost)
    private const String DATABASE_SERVER = "127.0.0.1";

    // Database to which we will connect
    private const String DATABASE_NAME = "harambedb";

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

    // Name of the table to which we will connect
    private const String tableName;

    // Command object
    private MySqlCommand cmd;

    // Constructor
    public TDGUser(String tableName){
        this.tableName = tableName;
        this.cmd = new MySqlCommand();
        this.cmd.CommandTimeout = COMMAND_TIMEOUT;
    }

    public static TDGUser getInstance (){
        return instance;
    }

    //Open the connection to the DB
    public bool openConnection (){
         try
            {
                conn = new MySqlConnection(DATABASE_CONNECTION_STRING);
                conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
    }

    //Close the connection to the DB
    public void closeConnection (){
        Console.WriteLine("Connection to DB is closed");
    }

    // Adds a list of users to the DB
    public void addUser(ArrayList<User> newList){

        openConnection();
        // You have a list of users and you add all the users in the list to the DB
        for(int i = 0 ; i < newList.Count() ; i++){
            Console.WriteLine("Saving new User " + newList[i].getName() + " to the DB.");
            createNewUser(newList[i]);
        }

        closeConnection();
    }

    // Updates a list of users in the DB
    public void updateUser(ArrayList<User> updateList){

        openConnection();

        for(int i = 0 ; i < updateList.Count() ; i++){
            Console.WriteLine("Modifying existing User " + updateList[i].getFirstName() + " in the DB.");
            updateUser(updateList[i]);
        }

        closeConnection();
    }

    // Removes a list of users from the DB
    public void deleteUser(ArrayList<User> deleteList){

        openConnection();

        for(int i = 0 ; i < deleteList.Count() ; i++){
            Console.WriteLine("Removing existing user " + deleteList[i].getFirstName() + " from the DB.");
            removeUser(deleteList[i]);
        }

        closeConnection();
    }

    // SQL Statement to create a new User/Row.
    public void createNewUser(User user){
        //ex. INSERT INTO `user` VALUES ('1','h_ortiz',password('hortiz'),'Hannah Ortiz','0');
        this.cmd.CommandText = "INSERT INTO " + tableName + " \n" +
                "VALUES (" + user.getUserID() + "," + user.getUsername() + ","
                + user.getPassword() + "," + user.getName() + "," + user.getNumOfReservations() + ");";
        this.cmd.Connection = this.conn;
        MySqlDataReader reader = cmd.ExecuteReader();
    }

    // SQL Statement to update an existing User/Row
    public void updateUser(User user) {
        this.cmd.CommandText = "UPDATE " + tableName + " \n" +
                "SET " + fields[1] + "=" + user.getUsername() + "," + fields[2] + "=" + user.getPassword() + "," +
                fields[3] + "=" + user.getName() + "," + fields[4] + "=" + user.getNumOfReservations() + ";\n" 
                "WHERE " + fields[0] + "=" + user.getUserID() + ";";
        this.cmd.Connection = this.conn;
        MySqlDataReader reader = cmd.ExecuteReader();
    }

    // SQL Statement to delete an existing User/Row.
    public void removeUser(User user) {
        this.cmd.CommandText = "DELETE FROM " + tableName + " \n" +
                "WHERE " + fields[0] + "=" + user.getUserID() + ";";
        this.cmd.Connection = this.conn;
        MySqlDataReader reader = cmd.ExecuteReader();
    }

    // SQL Statement to fetch an existing User/Row
    public MySqlDataReader fetchUser(int id){
        this.cmd.CommandText = "SELECT FROM " + tableName + " \n" +
                "WHERE " + fields[0] + "=" + user.getUserID() + ";";
        this.cmd.Connection = this.conn;
        MySqlDataReader reader = cmd.ExecuteReader();
    }

    /**
     * Select all data from the table, returns as MySqlDataReader object, but we can
     * always format it differently to ensure that only the TDG is related to database.
     * I.e. we can return a big 2D array instead of the object.
     */ 
    public MySqlDataReader fetchAll()
    {
        this.cmd.CommandText = "SELECT * FROM " + this.tableName + " WHERE 1;";
        this.cmd.Connection = this.conn;
        MySqlDataReader reader = cmd.ExecuteReader();


        // Print the 2nd element of each object found (for testing purposes)
        while (reader.Read())
        {
            Console.WriteLine(reader[1]);
        }
        return reader;
    }
}
