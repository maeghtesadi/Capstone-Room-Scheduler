public class UserMapper {

    private static UserMapper instance = new UserMapper();

    private TDGUser tdgUser = TDGUser.getInstance();
    private UserIdentityMap userIdentityMap = UserIdentityMap.getInstance();

    private UserMapper(){}

    public static UserMapper getInstance() {
        return instance;
    }

    // Handles the Creation of a new object of type User
    public User makeNew (int userID, string userName, string password, string name, int numOfReservations){

        User user = new User(userID, userName, password, name, numOfReservations);

        //Add the new User to the list of existing objects in Live memory
        userIdentityMap.addTo(user);

        //Add to UoW registry so that we create it in the DB once the user is ready to commit everything.
        UnitOfWork.getInstance().registerNew(user);

        return user;
    }


    //Fetch Message for retrieving a User with the user ID.
    public User getUser (int userID){

        User user = userIdentityMap.getInstance().find(userID);
        Object result[] = null;

        // If Identity Map doesn't have it then use TDG.
        if (user == null){
            result = tdgUser.fetchUser(userID);
        }

        // If TDG doesn't have it then it doens't exist.
        if (result == null){
            //Sorry no such user exist in the DB.
            return null;
        }
        else {
            //We got the user from the TDG who got it from the DB and now the mapper must add it to the UserIdentityMap
            User = new User((int)result[0], (String)result[1], (String)result[2], (String)result[3]), (int)result[4];
            userIdentityMap.getInstance().addTo(user);
            return user;
        }

    }


    public void setUser(int userID, string userName, string password, string name, int numOfReservations){

        // First we fetch the User || We could have passed the User as a Param. But this assumes you might not have
        // access to the instance of the desired object.
        User user = getUser(userID);

        // Mutator function to SET the new username.
        user.setUserName(userName);

        // Mutator function to SET the new password.
        user.setPassword(password);

        // Mutator function to SET the new name.
        user.setName(name);

        // Mutator function to SET the new numOfReservations.
        user.setNumOfReservations(numOfReservations);

        // We've modified something in the object so we Register the instance as Dirty in the UoW.
        UnitOfWork.getInstance().registerDirty(user);
    }


    public void delete (int userID){
        //First we fetch the user by checking the identity map
        User user = userIdentityMap.find(userID);

        // If the identity map returned the object, then remove it from the IdentityMap
        if (user != null){
            UserIdentityMap.getInstance().removeFrom(user);
        }

        // We want to delete this object from out DB, so we simply register it as Deleted in the UoW
        UnitOfWork.getInstance().registerDeleted(user);

    }

    // When we are ready to submit everything that is held in UnitofWork then we use done();
    public void done (){
        UnitOfWork.getInstance().commit();
    }

    //------------------------------------------------------------------------------------------------------------------
    // For Unit of Work to be able to call the appropriate functionalities.

    // Pass the list of User to add to DB to the TDG.
    public void addUser(List<User> newList){
        tdgUser.addUser(newList);
    }

    // Pass the list of User to update in the DB to the TDG
    public void updateUser(List<User> updateList){
        tdgUser.updateUser(updateList);
    }

    // Pass the list of User to remove from DB to the TDG
    public void deleteUser(List<User> deleteList){
        tdgUser.deleteUser(deleteList);
    }

}
