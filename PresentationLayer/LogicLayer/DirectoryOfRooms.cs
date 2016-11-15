using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class DirectoryOfRooms
    {
        public List<Room> roomList { get; set; }

        public DirectoryOfRooms()
        {
            roomList = new List<Room>();
        }

        public Room findRoom(int roomID)
        {
            Room room = new Room();
            for (int i = 0; i < roomList.Count(); i++)
                if (roomList[i].roomID == roomID)
                     room = roomList[i];
            return room;
        }

        public void getAll()
        {

        }

        //public void makeNewTimeSlot(int roomID, DateTime date, int timeSlotID)
        //{
        //    Room room = findRoom(roomID);
        //    room.getTimeSlotList().makeNewTimeSlot(date, timeSlotID);
        //}
        //Why is this function in DirectoryOfRooms?

        public List<Room> getAllRooms()
        {
            return roomList;
        }


    }
}
