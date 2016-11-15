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
        public DateTime date { get; set; }

        public TimeSlot()
        {
            timeSlotID = 0;
            isReserved = false;
            date = new DateTime();
        }

        public TimeSlot(int timeSlotID, DateTime date, bool isReserved)
        {
            this.timeSlotID = timeSlotID;
            this.isReserved = isReserved;
            this.date = date;
        }
    }
}
