using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class TimeSlot
    {
        public int timeSlotID { get; set; }
        public bool isReserved { get; set; }
        //private int hourlyID;
        public DateTime date { get; set; }

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
    }
}
