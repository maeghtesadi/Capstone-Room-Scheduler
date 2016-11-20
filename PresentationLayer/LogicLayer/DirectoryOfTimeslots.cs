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

        public List<TimeSlot> timeSlotList { get; set; }

        public DirectoryOfTimeSlots()
        {
            timeSlotList = new List<TimeSlot>();
            foreach (KeyValuePair<int, TimeSlot> timeSlot in TimeSlotMapper.getInstance().getAllTimeSlot())
            {
                timeSlotList.Add(timeSlot.Value);
            }

            for (int i = 0; i < timeSlotList.Count; i++)
            {
                List<int> waitList = WaitsForMapper.getInstance().getAllUsers(timeSlotList[i].timeSlotID);
                for (int j = 0; j < waitList.Count; j++)
                    timeSlotList[i].waitlist.Enqueue(waitList[j]);
            }
        }

    }
}
