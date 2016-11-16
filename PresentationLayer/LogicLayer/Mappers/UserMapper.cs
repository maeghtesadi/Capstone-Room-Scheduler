using System;
using System.Collections.Generic;
using TDG;
using LogicLayer;
using CapstoneRoomScheduler.LogicLayer.IdentityMaps;

namespace Mappers
{
    class UserMapper
    {

        private static UserMapper instance = new UserMapper();

        private TDGUser tdgUser = TDGUser.getInstance();
        private UserIdentityMap userIdentityMap = UserIdentityMap.getInstance();

        private UserMapper() { }

        public static UserMapper getInstance()
        {
            return instance;
        }

        /**
         * Retrieve a user given its ID
         */
        public User getUser(int userID)
        {
            User user = UserIdentityMap.getInstance().find(userID);
            Object[] result = null;

            // If not found in user identity map, try to retrieve from the DB
            if (user == null)
            {
                result = tdgUser.get(userID);
                // If the TDG doesn't have it, then it doesn't exist
                if (result == null)
                {
                    return null;
                }
                else
                {
                    // We got the user from the TDG who got it from the DB and now the mapper must add it to the UserIdentityMap
                    user = new User((int)result[0], (String)result[1], (String)result[2], (String)result[3], (int)result[4]);
                    UserIdentityMap.getInstance().addTo(user);
                    return user;
                }
            }
            return null; //so all code paths return a value
        }

        /**
        * Retrieve all users
        */
        public Dictionary<int, User> getAllUser()
        {
            // Get all users from the identity map
            Dictionary<int, User> users = UserIdentityMap.getInstance().findAll();

            // Get all users in the database
            Dictionary<int, Object[]> result = tdgUser.getAll();

            // Loop through each of the result:
            foreach (KeyValuePair<int, Object[]> record in result)
            {
                // The user is not in the identity map. Create an instance, add it to identity map and to the return variable
                if (!users.ContainsKey(record.Key))
                {
                    User user = new User();
                    user.userID = ((int)record.Key); // userID
                    user.username = ((String)record.Value[1]); // userName
                    user.password = ((String)record.Value[2]); // password
                    user.name = ((String)record.Value[3]); // name
                    user.numOfReservations = ((int)record.Value[4]); // numOfReservations

                    UserIdentityMap.getInstance().addTo(user);

                    users.Add(user.userID, user);
                }
            }

            return users;
        }
        /**
         * Set user attributes
         */
        public void setUser(int userID, string name, int numOfReservations)
        {

            // First we fetch the User || We could have passed the User as a Param. But this assumes you might not have
            // access to the instance of the desired object.
            User user = getUser(userID);

            // Mutator function to SET the new name.
            user.name = (name);

            // Mutator function to SET the new numOfReservations.
            user.numOfReservations = (numOfReservations);

            // We've modified something in the object so we Register the instance as Dirty in the UoW.
            UnitOfWork.getInstance().registerDirty(user);
        }

        /**
        * Done: commit
        */
        public void done()
        {
            UnitOfWork.getInstance().commit();
        }

        /**
         * For unit of work:
         * Update list of users on DB
         */
        public void updateUser(List<User> updateList)
        {
            tdgUser.updateUser(updateList);
        }


    }
}
