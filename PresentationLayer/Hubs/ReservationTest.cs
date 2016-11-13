using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Hubs
{
    public class ReservationTest
    {   public ReservationTest(int initialTimeslot, int finalTimeslot, int roomId, string studentName, string CourseName) {
            this.initialTimeslot = initialTimeslot;
            this.finalTimeslot = finalTimeslot;
            this.roomId = roomId;
            this.studentName = studentName;
            this.CourseName = CourseName;
    }
        public int initialTimeslot { get; set; }
        public int finalTimeslot { get; set; }
        public int roomId { get; set; }
        public string CourseName { get; set; }
        public string studentName { get; set; }
    }
}