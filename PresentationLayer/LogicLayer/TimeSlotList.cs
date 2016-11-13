using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class TimeSlotList
    {
        private List<TimeSlot> timeSlots;

        public TimeSlotList()
        {
            timeSlots = new List<TimeSlot>(); //create empty list of TimeSlot objects
        }

        public List<TimeSlot> getTimeSlotList()
        {
            return timeSlots;
        }

        public void addTimeSlot(TimeSlot ts)
        {
            timeSlots.Add(ts);
        }

        public TimeSlot findTimeSlot(int timeslotid)
        {
            for (int i = 0; i < timeSlots.Count; i++)
            {
                if (timeSlots[i].getTimeSlotID() == timeslotid)
                {
                    return timeSlots[i];
                }
            }
            return null;
        }






    }
}
