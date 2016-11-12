using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageLayer;

namespace LogicLayer
{
    class RoomMapper
    {
        // Instance of this mapper object
        private static RoomMapper instance = new RoomMapper();

        private TDGRoom tdgRoom = TDGRoom.getInstance();
        private RoomIdentityMap roomIdentityMap = RoomIdentityMap.getInstance();

        private RoomMapper() { }

        public static RoomMapper getInstance()
        {
            return instance;
        }

        /**
         * Handles the creation of a new room
         */
        public Room makeNew (String roomNum)
        {
            // Make a new room object
            Room room = new Room();
            room.setRoomID(room.GetHashCode());
            room.setRoomNum(roomNum);

            // Add it to the identity map
            roomIdentityMap.add(room);

            // Register as a new room
            UnitOfWork.getInstance().registerNew(room);

            return room;
        } 

        /**
         * Retrieve a room given its ID
         */
        public Room getRoom (int roomID)
        {
            // Try to obtain the room from the identity map
            Room room = roomIdentityMap.getInstance().find(roomID);
            Object[] result = null;
            if(room == null)
            {
                // Not found in identity map: try to retrive from DB
                result = tdgRoom.fetch(roomID);
                if (result != null)
                {
                    room = new Room();
                    room.setRoomID((int) result[0]); // roomID
                    room.setRoomNum((String) result[1]); // roomNum
                    roomIdentityMap.getInstance().addTo(room);
                }
            }

            // Null is returned if it is not found in the identity map nor in the DB
            return room;
        } 

        /**
         * Retrieve all rooms
         */
        public Dictionary<int, Room> getAllRoom()
        {
            // Get all rooms from the identity map
            Dictionary<int, Room> rooms = roomIdentityMap.getInstance().findAll();

            // Get all rooms in the database
            Dictionary<int, Object[]> result = tdgRoom.fetchAll();

            // Loop through each of the result:
            foreach(KeyValuePair<int, Object[]> record in result)
            {
                // The room is not in the identity map. Create an instance, add it to identity map and to the return variable
                if(!rooms.ContainsKey(record.Key))
                {
                    Room room = new Room();
                    room.setRoomID((int) record.Key); // roomID
                    room.setRoomNum((String) record.Value[1]); // roomNum
                    
                    roomIdentityMap.getInstance().addTo(room);
                    
                    rooms.Add(room.getRoomID(), room);
                }
            } 

            return rooms;
        }  
        
        /**
         * Set room attributes
         */
        public void setRoom(int roomID, String RoomNum)
        {
            // Get the room that needs to be updated
            Room room = getRoom(roomID);

            // Update the room
            room.setRoomNum(RoomNum);

            // Register it to the unit of work
            UnitOfWork.getInstance().registerDirty(room);
        } 

        /**
         * Delete room
         */
        public void delete(int roomID)
        {
            // Get the room to be deleted
            Room room = roomIdentityMap.find(roomID);

            // If found, remove it from identity map
            if(room != null)
            {
                roomIdentityMap.getInstance().removeFrom(room);
            }

            // Register as deleted
            UnitOfWork.getInstance().registerDeleted(room);
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
         * Add a list of rooms to DB
         */
        public void addRoom(List<Room> newList)
        {
            tdgRoom.addRoom(newList);
        } 

        /**
         * For unit of work:
         * Update list of rooms on DB
         */
        public void updateRoom(List<Room> updateList)
        {
            tdgRoom.updateRoom(updateList);
        }
        
        /**
         * For unit of work:
         * Remove list of rooms from DB
         */
        public void deleteRoom(List<Room> deleteList)
        {
            tdgRoom.deleteRoom(deleteList);
        }
    }
}
