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
            timeSlots = new List<TimeSlot>();
        }

        public List<TimeSlot> getTimeSlotList()
        {
            return timeSlots;
        }

    }
}
