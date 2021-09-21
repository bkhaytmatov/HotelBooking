using System.Collections.Generic;
using HotelBooking.Models;

namespace HotelBooking.Interfaces
{
    public interface IReservationManagement
    {
        // get all available rooms
        public List<Room> GetRoomAvailability(Reservation reservation);

        // book available room
        public Booking BookRoom(Booking booking);
    }
}
