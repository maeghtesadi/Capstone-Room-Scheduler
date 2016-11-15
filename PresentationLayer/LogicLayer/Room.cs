using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class Room
    {
        public int roomID { get; set; }
        public string roomNum { get; set; }
        public TimeSlotList listOfTimeSlots { get; set; }

        public Room()
        {
            roomID = 0;
            roomNum = "";
            listOfTimeSlots = new TimeSlotList();
        }

        public Room(int roomID, string roomNum)
        {
            this.roomID = roomID;
            this.roomNum = roomNum;
        }

        //public void makeNewTimeSlot(DateTime date, int timeSlotID)
        //{
        //    for (int i = 0; i < listOfTimeSlots.getTimeSlotList().Count; i ++)
        //    {
        //        if (timeSlotID == listOfTimeSlots.getTimeSlotList()[i].getTimeSlotID())
        //        {
        //            if (listOfTimeSlots.getTimeSlotList()[i].getIsReserved())
        //            {
        //                //addToWaitList()
        //            }
        //            else
        //            {
        //                listOfTimeSlots.getTimeSlotList().Add(createTimeSlot(date, timeSlotID));
        //            }
        //        }     
        //    }
        //}

        //public TimeSlot createTimeSlot(DateTime date, int timeSlotID)
        //{
        //    TimeSlot timeSlot = new TimeSlot(timeSlotID, date, true);
        //    return timeSlot;
        //}

        //These two functions above should not be in Room class, should be in TimeSlotList

        public TimeSlotList getTimeSlotList()
        {
            return listOfTimeSlots;
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
