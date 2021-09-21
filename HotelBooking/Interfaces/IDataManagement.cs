using System.Collections.Generic;
using HotelBooking.Models;

namespace HotelBooking.Interfaces
{
    public interface IDataManagement
    {
        // gets the list of all rooms in the hotel
        public List<Room> GetAllRooms();

        // creates reservation in the storage
        public bool CreateReservation(Room room);
    }
}
