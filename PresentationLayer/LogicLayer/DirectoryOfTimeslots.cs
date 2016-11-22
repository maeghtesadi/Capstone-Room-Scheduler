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

        public void addToListOfTimeSlots(TimeSlot ts)
        {
            timeSlotList.Add(ts);
        }

        public void deleteFromListOfTimeSlots(int timeSlotID)
        {
            for (int i = 0; i < timeSlotList.Count; i++)
            {
                if (timeSlotList[i].timeSlotID == timeSlotID)
                {
                    timeSlotList.Remove(timeSlotList[i]);
                    break;
                }
            }
        }

        public TimeSlot makeNewTimeSlot(int timeslotID, int resID, int hour, Queue<int> wlist)
        {
            //TimeSlot timeSlot = TimeSlotMapper.getInstance().makeNew(resID, hour);
            TimeSlot timeSlot = new TimeSlot(timeslotID, resID, hour, wlist);
            //timeSlotList.Add(timeSlot);
            return timeSlot;
        }

        public void modifyTimeSlot(int timeSlotID, int resID, Queue<int> wlist)
        {
            for (int i = 0; i < timeSlotList.Count; i++ )
            {
                if (timeSlotList[i].timeSlotID == timeSlotID)
                {
                    timeSlotList[i].reservationID = resID;
                    timeSlotList[i].timeSlotID = timeSlotID;
                    timeSlotList[i].waitlist = wlist;
                    break;
                }
            }
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
