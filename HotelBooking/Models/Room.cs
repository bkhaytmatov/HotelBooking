using System;
using System.Collections.Generic;

namespace HotelBooking.Models
{
    public enum RoomType
    {
        SINGLE = 1,
        DOUBLE = 2,
    }

    public class Room
    {
        public int RoomId { get; set; }
        public bool  IsAvailable { get; set; }
        public RoomType RoomType { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
