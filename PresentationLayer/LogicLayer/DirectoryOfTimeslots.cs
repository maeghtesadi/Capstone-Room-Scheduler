using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappers;

namespace LogicLayer
{
    public class DirectoryOfTimeSlots
    {

        public List<TimeSlot> timeSlotList { get; set; }

        public DirectoryOfTimeSlots()
        {
            timeSlotList = new List<TimeSlot>();
            foreach (KeyValuePair<int, TimeSlot> timeSlot in TimeSlotMapper.getInstance().getAllTimeSlot())
            {
                timeSlotList.Add(timeSlot.Value);
            }
            for (int i = 0; i < timeSlotList.Count; i++)
            {
                List<int> waitList = WaitsForMapper.getInstance().getAllUsers(timeSlotList[i].timeSlotID);
                if (waitList != null)
                    for (int j = 0; j < waitList.Count; j++)
                        timeSlotList[i].waitlist.Enqueue(waitList[j]);
            }
        }

        public TimeSlot makeNewTimeSlot(int resid, int hour)
        {
            TimeSlot timeSlot = TimeSlotMapper.getInstance().makeNew(resid, hour);
            timeSlotList.Add(timeSlot);
            return timeSlot;
        }

        public void deleteTimeSlot(int id)
        {
            TimeSlotMapper.getInstance().delete(id);

            foreach (TimeSlot timeSlot in timeSlotList)
                if (timeSlot.timeSlotID == id)
                    timeSlotList.Remove(timeSlot);
        }

        public void refresh()
        {
            for (int i = 0; i < timeSlotList.Count; i++)
            {
                List<int> waitList = WaitsForMapper.getInstance().getAllUsers(timeSlotList[i].timeSlotID);
                if (waitList != null)
                    for (int j = 0; j < waitList.Count; j++)
                        timeSlotList[i].waitlist.Enqueue(waitList[j]);
            }
        }

        public void addToWaitList(int timeslotid, int reservationid, Queue<int> waitlist)
        {
            TimeSlotMapper.getInstance().setTimeSlot(timeslotid, reservationid, waitlist);
        }

    }
}
