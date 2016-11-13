//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using StorageLayer;

//namespace LogicLayer
//{
//    class UserMapper
//    {

//        private static UserMapper instance = new UserMapper();

//        private TDGUser tdgUser = TDGUser.getInstance();
//        private UserIdentityMap userIdentityMap = UserIdentityMap.getInstance();

//        private UserMapper() { }

//        public static UserMapper getInstance()
//        {
//            return instance;
//        }

//        /**
//         * Retrieve a user given its ID
//         */
//        public User getUser(int userID)
//        {
//            User user = userIdentityMap.getInstance().find(userID);
//            Object result[] = null;

//            // If not found in user identity map, try to retrieve from the DB
//            if (user == null)
//            {
//                result = tdgUser.fetch(userID);
//                // If the TDG doesn't have it, then it doesn't exist
//                if (result == null){
//                return null;
//                }
//                else {
//                // We got the user from the TDG who got it from the DB and now the mapper must add it to the UserIdentityMap
//                User = new User((int)result[0], (String)result[1], (String)result[2], (String)result[3]), (int)result[4];
//                userIdentityMap.getInstance().addTo(user);
//                return user;
//                }
//            }
//        }

//        /**
//        * Retrieve all users
//        */
//        public Dictionary<int, User> getAllUser()
//        {
//            // Get all users from the identity map
//            Dictionary<int, User> users = userIdentityMap.getInstance().findAll();

//            // Get all users in the database
//            Dictionary<int, Object[]> result = tdgUser.fetchAll();

//            // Loop through each of the result:
//            foreach (KeyValuePair<int, Object[]> record in result)
//            {
//                // The user is not in the identity map. Create an instance, add it to identity map and to the return variable
//                if (!users.ContainsKey(record.Key))
//                {
//                    User user = new User();
//                    user.setUserID((int)record.Key); // userID
//                    user.setUserName((String)record.Value[1]); // userName
//                    user.setUserPassword((String)record.Value[2]); // password
//                    user.setName((String)record.Value[3]); // name
//                    user.setNumOfReservations((int)record.Value[4]); // numOfReservations

//                    userIdentityMap.getInstance().addTo(user);

//                    users.Add(user.getUserID(), user);
//                }
//            }

//            return users;
//        }

//    }
//}
