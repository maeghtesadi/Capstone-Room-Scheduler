using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class Room
    {
        int roomID;
        string roomNum;

        public Room()
        {
            roomID = 0;
            roomNum = "";
        }

        public Room(int roomID, string roomNum)
        {
            this.roomID = roomID;
            this.roomNum = roomNum;
        }

        public void setRoomID(int id)
        {
            roomID = id;
        }

        public int getRoomID()
        {
            return roomID;
        }

        public void setRoomNum(string num)
        {
            roomNum = num;
        }

        public string getRoomNum()
        {
            return roomNum;
        }

        public bool find(TimeSlot timeSlot)
        {
            return true;
        }

        public bool checkAvailability()
        {
            return true;
        }
    }
}
