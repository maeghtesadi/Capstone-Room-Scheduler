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

            foreach (KeyValuePair<int, Room> room in RoomMapper.getInstance().getAllRooms())
            {
                roomList.Add(room.Value);
            }

            for (int i = 0; i < roomList.Count; i++)
            {
                foreach (KeyValuePair<int, Reservation> reservation in ReservationMapper.getInstance().getAllReservation())
                {
                    if (reservation.Value.roomID == roomList[i].roomID)
                        roomList[i].roomReservations.Add(reservation.Value);
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
