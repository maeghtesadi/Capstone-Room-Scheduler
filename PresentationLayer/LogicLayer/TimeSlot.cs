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
        public int reservationID { get; set; }
        public int hour { get; set; }
        //public bool isReserved { get; set; }
        //public DateTime date { get; set; }
        Queue<int> waitlist = new Queue<int>();

        public TimeSlot()
        {
            timeSlotID = 0;
            hour = 0;
        }

        public TimeSlot(int timeSlotID, DateTime date, bool isReserved, int hour)
        {
            this.timeSlotID = timeSlotID;
            this.hour = hour;
        }
    }
}
