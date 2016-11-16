using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicLayer
{
    public class UserIdentityMap
    {
        /**
         * User Identity Map instance (Singleton)
         */
        private static UserIdentityMap instance = new UserIdentityMap();

        /**
         * Dictionary representing the users that are currently in the active memory
         */
        private Dictionary<int, User> userList_ActiveMemory = new Dictionary<int, User>();

        /**
         * Private constructor (Singleton)
         */
        private UserIdentityMap() { }

        /**
         * Returns the instance of UserIdentityMap
         */
        public static UserIdentityMap getInstance()
        {
            return instance;
        }

        /**
         * Adds a user object to the dictionary representing all users in the active memory
         */
        public void addTo(User user)
        {
            userList_ActiveMemory.Add(user.getUserID(), user);
        }

        /**
         * Removes a user object from the active memory dictionary
         */
        public void removeFrom(User user)
        {
            userList_ActiveMemory.Remove(user.getUserID());
        }

        /**
         * Finds and return a user based on its id from the active memory
         */
        public User find(int userID)
        {
            User user;
            if (userList_ActiveMemory.TryGetValue(userID, out user))
            {
                return user;
            }
            return null;
        }

        /**
         * Finds all users that are currently in the active memory
         */
        public Dictionary<int, User> findAll()
        {
            // Create a new dictionary to be returned
            Dictionary<int, User> newDictionary = new Dictionary<int, User>();

            // Copy each key value pairs (do not need to deep copy the value, user).
            // We simply want to not return the reference to the dictionary used here.
            foreach (KeyValuePair<int, User> pair in this.userList_ActiveMemory)
            {
                newDictionary.Add(pair.Key, pair.Value);
            }

            return newDictionary;
        }
    }
}