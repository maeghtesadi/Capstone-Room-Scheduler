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
        public List<Room> roomList { get; set; }

        public DirectoryOfRooms()
        {
            roomList = new List<Room>();

            DirectoryOfRooms roomDirectory = new DirectoryOfRooms();
            DirectoryOfReservations reservationDirectory = new DirectoryOfReservations();

            foreach (KeyValuePair<int, Room> room in RoomMapper.getInstance().getAllRooms())
            {
                roomDirectory.roomList.Add(room.Value);
            }

            for (int i = 0; i < roomDirectory.roomList.Count; i++)
            {
                for (int j = 0; j < reservationDirectory.reservationList.Count; j++)
                {
                    if (reservationDirectory.reservationList[j].roomID == roomDirectory.roomList[i].roomID)
                        roomDirectory.roomList[i].roomReservations.Add(reservationDirectory.reservationList[j]);
                }
            }
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
