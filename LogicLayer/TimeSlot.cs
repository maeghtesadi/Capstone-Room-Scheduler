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
        private bool isReserved;
        //private int hourlyID;
        private DateTime date;

        public TimeSlot()
        {
            timeSlotID = 0;
            isReserved = false;
            //hourlyID = 0;
            date = new DateTime();
            //03012016
        }

        public TimeSlot(int timeSlotID, DateTime date, bool isReserved)
        {
            this.timeSlotID = timeSlotID;
            this.isReserved = isReserved;
           // this.hourlyID = hourlyID;
            this.date = date;
        }

        public bool getIsReserved()
        {
            return isReserved;
        }

        public void setIsReserved(bool reserved)
        {
            isReserved = reserved;
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
           // hourlyID = id;
        }

        public int getHourlyId()
        {
            //return hourlyID;
            return 0;
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
