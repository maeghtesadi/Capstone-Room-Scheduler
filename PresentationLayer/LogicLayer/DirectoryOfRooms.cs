using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappers;

namespace LogicLayer
{
    public class DirectoryOfRooms
    {

        private static DirectoryOfRooms instance = new DirectoryOfRooms();

        public List<Room> roomList { get; set; }

        public DirectoryOfRooms()
        {
            roomList = new List<Room>();

            foreach (KeyValuePair<int, Room> room in getAllRooms())
            {
                roomList.Add(room.Value);
            }           
        }

        public static DirectoryOfRooms getInstance()
        {
            return instance;
        }

        public Dictionary<int, Room> getAllRooms()
        {
            return RoomMapper.getInstance().getAllRooms();
        }

        public void done()
        {
            RoomMapper.getInstance().done();
        }

        public Room findRoom(int roomID)
        {
            Room room = new Room();
            for (int i = 0; i < roomList.Count(); i++)
                if (roomList[i].roomID == roomID)
                    room = roomList[i];
            return room;
        }

    }
}
