using System;
using System.Collections.Generic;
using TDG;
using LogicLayer;
using CapstoneRoomScheduler.LogicLayer.IdentityMaps;

namespace Mappers
{
    class RoomMapper
    {
        // Instance of this mapper object
        private static RoomMapper instance = new RoomMapper();

        private TDGRoom tdgRoom = TDGRoom.getInstance();
        private RoomIdentityMap roomIdentityMap = RoomIdentityMap.getInstance();

        // The last ID that is used
        private int lastID;

        // Lock to modify last ID
        private readonly Object lockLastID;

        private RoomMapper()
        {
            this.lastID = tdgRoom.getLastID();
        }

        public static RoomMapper getInstance()
        {
            return instance;
        }


        /**
         *  Obtain the next ID available
         **/
        private int getNextID()
        {
            // Increments the last ID atomically, return the increment value
            int nextID;

            lock (this.lockLastID)
            {
                this.lastID++;
                nextID = this.lastID;
            }
            return nextID;
        }

        /**
         * Handles the creation of a new room
         */
        public Room makeNew (String roomNum)
        {

            int nextID = getNextID();

            // Make a new room object
            Room room = new Room();
            room.roomID = nextID;
            room.roomNum = roomNum;

            // Add it to the identity map
            roomIdentityMap.addTo(room);

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
            Room room = roomIdentityMap.find(roomID);
            Object[] result = null;
            if(room == null)
            {
                // Not found in identity map: try to retrive from DB
                result = tdgRoom.get(roomID);
                if (result != null)
                {
                    room = new Room();
                    room.roomID = ((int) result[0]); // roomID
                    room.roomNum = ((String) result[1]); // roomNum
                    roomIdentityMap.addTo(room);
                }
            }

            // Null is returned if it is not found in the identity map nor in the DB
            return room;
        } 

        /**
         * Retrieve all rooms
         */
        public Dictionary<int, Room> getAllRooms()
        {
            // Get all rooms from the identity map
            Dictionary<int, Room> rooms = roomIdentityMap.findAll();

            // Get all rooms in the database
            Dictionary<int, Object[]> result = tdgRoom.getAll();

            // Loop through each of the result:
            foreach(KeyValuePair<int, Object[]> record in result)
            {
                // The room is not in the identity map. Create an instance, add it to identity map and to the return variable
                if(!rooms.ContainsKey(record.Key))
                {
                    Room room = new Room();
                    room.roomID = ((int) record.Key); // roomID
                    room.roomNum = ((String) record.Value[1]); // roomNum
                    
                    roomIdentityMap.addTo(room);
                    
                    rooms.Add(room.roomID, room);
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
            room.roomNum = (RoomNum);

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
                roomIdentityMap.removeFrom(room);
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
