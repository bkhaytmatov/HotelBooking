using System;
using HotelBooking.Models;
using HotelBooking.Services.Configuration;

namespace HotelBooking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is a simple Console App - it is where everthing starts. " +
                "\nYou can verify the same results by running unit test project from the same solution. " +
                "\nWe begin by performing DI in manually created Startup file. " +
                "\nAfterwards we can execute calls to public methods of IReservationManagement interface implemented by ReservationManagement.");

            //perform all DI
            var reservationManagement = Startup.Configure();

            var availableBeginning = reservationManagement.GetRoomAvailability(new Reservation
            {
                StartDate = new DateTime(2022, 05, 01),
                EndDate = new DateTime(2022, 05, 11)
            });

            var reserved = reservationManagement.GetRoomAvailability(new Reservation
            {
                StartDate = new DateTime(2022, 05, 15),
                EndDate = new DateTime(2022, 05, 20)
            });

            var availableEnd = reservationManagement.GetRoomAvailability(new Reservation
            {
                StartDate = new DateTime(2022, 07, 01),
                EndDate = new DateTime(2022, 07, 11)
            });

            var pastDate = reservationManagement.GetRoomAvailability(new Reservation
            {
                StartDate = new DateTime(2020, 07, 01),
                EndDate = new DateTime(2020, 07, 11)
            });

            var endDateBeforeStartDate = reservationManagement.GetRoomAvailability(new Reservation
            {
                StartDate = new DateTime(2022, 08, 01),
                EndDate = new DateTime(2022, 07, 11)
            });


            var createReservation = reservationManagement.BookRoom(new Booking
            {
                StartDate = new DateTime(2022, 05, 01),
                EndDate = new DateTime(2022, 05, 11),
                RoomType = RoomType.SINGLE
            });

            Console.ReadLine();
        }
    }
}
