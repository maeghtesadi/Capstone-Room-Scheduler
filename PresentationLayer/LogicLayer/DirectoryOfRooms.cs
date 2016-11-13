using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class DirectoryOfRooms
    {
        List<Room> roomList;


        public DirectoryOfRooms()
        {
        }

        public Room findRoom(int roomID)
        {
            Room room = new Room();
            for (int i = 0; i < roomList.Count(); i++)
                if (roomList[i].getRoomID() == roomID)
                     room = roomList[i];
            return room;
        }

        public void getAll()
        {

        }

        public void makeNewTimeSlot(int roomID, DateTime date, int timeSlotID)
        {
            Room room = findRoom(roomID);
            //room.makeNewTimeSlot(date, timeSlotID);
        }
    }
}
