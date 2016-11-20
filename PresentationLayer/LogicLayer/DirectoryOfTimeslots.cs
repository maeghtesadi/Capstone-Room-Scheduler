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

            foreach (KeyValuePair<int, TimeSlot> timeSlot in getAllTimeSlot())
            {
                timeSlotList.Add(timeSlot.Value);
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

        public List<int> getAllUsers(int timeSlotID)
        {
            return TimeSlotMapper.getInstance().getAllUsers(timeSlotID);
        }

        public void addToWaitList(int timeslotid, int reservationid, Queue<int> waitlist)
        {
            TimeSlotMapper.getInstance().setTimeSlot(timeslotid, reservationid, waitlist);
        }

        public Dictionary<int, TimeSlot> getAllTimeSlot()
        {
            return TimeSlotMapper.getInstance().getAllTimeSlot();
        }

        public void done()
        {
            TimeSlotMapper.getInstance().done();
        }
    }
}
