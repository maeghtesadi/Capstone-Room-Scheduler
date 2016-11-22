using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class TimeSlotList
    {
        public List<TimeSlot> timeSlots { get; set; }

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

        public TimeSlot findTimeSlot(int timeSlotId)
        {
            for (int i = 0; i < timeSlots.Count; i++)
            {
                if (timeSlots[i].timeSlotID == timeSlotId)
                {
                    return timeSlots[i];
                }
            }
            return null;
        }
    }
}
