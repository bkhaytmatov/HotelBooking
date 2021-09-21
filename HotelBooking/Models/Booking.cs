using System;
namespace HotelBooking.Models
{
    public class Booking
    {
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RoomType RoomType { get; set; }
        public bool IsBooked { get; set; }
        public string Message { get; set; }
    }
}
