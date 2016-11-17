using System.Collections.Generic;
using TDG;
using LogicLayer;

namespace Mappers
{
    class WaitsForMapper
    {
        // Instance of this mapper object
        private static WaitsForMapper instance = new WaitsForMapper();

        private TDGWaitsFor tdgWaitsFor = TDGWaitsFor.getInstance();

        private WaitsForMapper() { }

        public static WaitsForMapper getInstance()
        {
            return instance;
        }

        public List<int> getAllUsers(int timeslotID)
        {
            return tdgWaitsFor.getAllUsers(timeslotID);
        }

        public void refreshWaitsFor(List<TimeSlot> refreshList)
        {
            tdgWaitsFor.refreshWaitsFor(refreshList);
        }
    }
}
