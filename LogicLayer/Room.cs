using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class Room
    {
        private int roomID;
        private string roomNum;
        private List<TimeSlot> timeSlots;

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

        public void makeNewTimeSlot(DateTime date, int timeSlotID)
        {
            for (int i = 0; i < timeSlots.Count; i ++)
            {
                if (timeSlotID == timeSlots[i].getTimeSlotID())
                {
                    if (timeSlots[i].getIsReserved())
                    {
                        //addToWaitList()
                    }
                    else
                    {
                        timeSlots.Add(createTimeSlot(date, timeSlotID));
                    }
                }     
            }
        }

        public TimeSlot createTimeSlot(DateTime date, int timeSlotID)
        {
            TimeSlot timeSlot = new TimeSlot(timeSlotID, date, true);
            return timeSlot;
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
