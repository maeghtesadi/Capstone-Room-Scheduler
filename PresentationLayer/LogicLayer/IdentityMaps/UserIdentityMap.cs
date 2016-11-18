using System.Collections.Generic;
using LogicLayer;

namespace CapstoneRoomScheduler.LogicLayer.IdentityMaps
{
    public class UserIdentityMap
    {
        //default constructor
        private UserIdentityMap() { }
        //an instance
        private static UserIdentityMap instance = new UserIdentityMap();
        //list of all users in active memory
        LinkedList<User> userList_ActiveMemory = new LinkedList<User>();

        public static UserIdentityMap getInstance()
        {
            return instance;
        }

        public void addTo(User user)
        {
            userList_ActiveMemory.AddLast(user);
        }
        
        public void removeFrom(User user)
        {
            userList_ActiveMemory.Remove(user);
        }

        public User findByName(string name)
        {
            foreach (User user in userList_ActiveMemory)
            {
                if (user.name == name)
                {
                    return user;
                }
            }

            return null;
        }

        public User find(int id)
        {
            //for (int i = 0; i < userList_ActiveMemory.Count; i++)
            //{
            //    if (userList_ActiveMemory.ElementAt(i).userID == id)
            //    {
            //        return userList_ActiveMemory.ElementAt(i);
            //    }
            //}
            foreach (User user in userList_ActiveMemory)
            {
                if (user.userID == id)
                {
                    return user;
                }
            }

                return null;
        }

    }
}