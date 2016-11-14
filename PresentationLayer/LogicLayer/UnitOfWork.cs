using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageLayer;

namespace LogicLayer
{
    public class UnitOfWork
    {

        private static UnitOfWork instance = new UnitOfWork();

        private List<User> userNewList = new List<User>();
        private List<User> userChangedList = new List<User>();
        private List<User> userDeletedList = new List<User>();

        private List<Reservation> reservationNewList = new List<Reservation>();
        private List<Reservation> reservationChangedList = new List<Reservation>();
        private List<Reservation> reservationDeletedList = new List<Reservation>();

        private List<Room> roomNewList = new List<Room>();
        private List<Room> roomChangedList = new List<Room>();
        private List<Room> roomDeletedList = new List<Room>();

        UserMapper userMapper = userMapper.getInstance();
        RoomMapper roomMapper = roomMapper.getInstance();
        ReservationMapper reservationMapper = reservationMapper.getInstance();

        private UnitOfWork() { }

        public static UnitOfWork getInstance()
        {
            return instance;
        }

        public void registerDirty(User user)
        {
            userChangedList.add(user);
        }

        public void registerNew(Reservation reservation)
        {
            reservationNewList.add(reservation);
        }

        public void registerDirty(Reservation reservation)
        {
            reservationChangedList.add(reservation);
        }

        public void registerDeleted(Reservation reservation)
        {
            reservationDeletedList.add(reservation);
        }

        public void registerNew(Room room)
        {
            roomNewList.add(room);
        }

        public void registerDirty(Room room)
        {
            roomChangedList.add(room);
        }

        public void registerDeleted(Room room)
        {
            roomDeletedList.add(room);
        }
        public void commit()
        {

            // To be verified with respective mappers
            if (userNewList.Count() != 0)
                userMapper.addUser(userNewList);
            if (userChangedList.Count() != 0)
                userMapper.updateUser(userChangedList);
            if (userDeletedList.Count() != 0)
                userMapper.deleteUser(userDeletedList);

            if (reservationNewList.Count() != 0)
                reservationMapper.addReservation(reservationNewList);
            if (reservationChangedList.Count() != 0)
                reservationMapper.updateReservation(reservationChangedList);
            if (userDeletedList.Count() != 0)
                reservationMapper.deleteReservation(reservationDeletedList);

            if (roomNewList.Count() != 0)
                roomMapper.addRoom(roomNewList);
            if (roomChangedList.Count() != 0)
                roomMapper.updateRoom(roomChangedList);
            if (roomDeletedList.Count() != 0)
                roomMapper.deleteRoom(roomDeletedList);

            //Empty the lists after the Commit.
            userDeletedList.clear();
            userChangedList.clear();
            userNewList.clear();
            reservationDeletedList.clear();
            reservationChangedList.clear();
            reservatioNewList.clear();
            roomDeletedList.clear();
            roomChangedList.clear();
            roomNewList.clear();
        }
    }
}
