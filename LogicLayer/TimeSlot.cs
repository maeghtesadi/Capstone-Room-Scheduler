using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class TimeSlot
    {
        int timeSlotID;
        int hourlyID;
        DateTime date;

        public void setTimeSlotID(int id)
        {
            timeSlotID = id;
        }

        public int getTimeSlotID()
        {
            return timeSlotID;
        }

        public void setHourlyId(int id)
        {
            hourlyID = id;
        }

        public int getHourlyId()
        {
            return hourlyID;
        }

        public void setDate(DateTime newDate)
        {
            date = newDate;
        }

        public DateTime getDate()
        {
            return date;
        }
    }
}
