using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDG;

namespace Mapper
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

        public List<int> getAllUsers(int reservationID)
        {
            return tdgWaitsFor.getAllUsers(reservationID);
        }

        public void refreshWaitsFor(List<Reservation> refreshList)
        {
            tdgWaitsFor.refreshWaitsFor(refreshList);
        }
    }
}
