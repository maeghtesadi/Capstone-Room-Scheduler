using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    class TimeSlot
    {
        private int timeSlotID;
        private int hourlyID;
        private DateTime date;

        public TimeSlot()
        {
            timeSlotID = 0;
            hourlyID = 0;
            date = new DateTime();
        }

        public TimeSlot(int timeSlotID, int hourlyID, DateTime date)
        {
            this.timeSlotID = timeSlotID;
            this.hourlyID = hourlyID;
            this.date = date;
        }

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
