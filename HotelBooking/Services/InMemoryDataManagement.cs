using System;
using System.Collections.Generic;
using HotelBooking.Interfaces;
using HotelBooking.Models;
using System.Linq;

namespace HotelBooking.Services
{
    public class InMemoryDataManagement : IDataManagement
    {
        private readonly Hotel _reservationData;

        public InMemoryDataManagement(Hotel reservationData)
        {
            _reservationData = reservationData;
        }

        public List<Room> GetAllRooms()
        {
            if(_reservationData.Rooms != null && _reservationData.Rooms.Count > 0)
            {
                return _reservationData.Rooms;
            }

            return null;
        }

        public bool CreateReservation(Room room)
        {
            // get the room to be booked by RoomId and RoomType
            var roomToModify = _reservationData.Rooms.Where(x => x.RoomId == room.RoomId && x.RoomType == room.RoomType).FirstOrDefault();

            // make sure there's a value
            if(roomToModify == null)
            {
               return false;
            }

            // get index of the room to be booked
            var indexOfRoom = _reservationData.Rooms.IndexOf(roomToModify);

            // add new reservation to the list
            _reservationData.Rooms[indexOfRoom].Reservations.Add(new Reservation
            {
                StartDate = room.Reservations.First().StartDate,
                EndDate = room.Reservations.First().EndDate
            });

            // override unsorted registrations with sorted list in the data set
            _reservationData.Rooms[indexOfRoom].Reservations
                = _reservationData.Rooms[indexOfRoom].Reservations.OrderBy(x => x.StartDate).ToList();

            return true;
        }
    }
}
