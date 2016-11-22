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

        private static DirectoryOfTimeSlots instance = new DirectoryOfTimeSlots();

        public List<TimeSlot> timeSlotList { get; set; }

        public DirectoryOfTimeSlots()
        {
            timeSlotList = new List<TimeSlot>();

            foreach (KeyValuePair<int, TimeSlot> timeSlot in getAllTimeSlot())
            {
                timeSlotList.Add(timeSlot.Value);
            }
        }

        public static DirectoryOfTimeSlots getInstance()
        {
            return instance;
        }

        public TimeSlot makeNewTimeSlot(int resID, int hour)
        {
            TimeSlot timeSlot = TimeSlotMapper.getInstance().makeNew(resID, hour);
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

        public void addToWaitList(int timeslotID, int reservationID, Queue<int> waitlist)
        {
            TimeSlotMapper.getInstance().setTimeSlot(timeslotID, reservationID, waitlist);
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
