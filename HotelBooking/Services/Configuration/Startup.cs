using System.Collections.Generic;
using System;
using HotelBooking.Models;
using HotelBooking.Interfaces;

namespace HotelBooking.Services.Configuration
{
    public static class Startup
    {
        public static IReservationManagement Configure()
        {
            // instantiating data layer
            IDataManagement inMemoryDataManagement = new InMemoryDataManagement(new Hotel
            {
                Rooms = new List<Room>
                    {
                        new Room { RoomId = 1, RoomType = RoomType.SINGLE, Reservations = new List<Reservation>
                        {
                            new Reservation { StartDate = new DateTime(2021, 05, 01), EndDate = new DateTime(2021, 05, 08)},
                            new Reservation { StartDate = new DateTime(2021, 05, 09), EndDate = new DateTime(2021, 05, 11)},
                            new Reservation { StartDate = new DateTime(2022, 05, 12), EndDate = new DateTime(2022, 05, 15)},
                            new Reservation { StartDate = new DateTime(2022, 05, 17), EndDate = new DateTime(2022, 05, 22)},
                            new Reservation { StartDate = new DateTime(2022, 06, 01), EndDate = new DateTime(2022, 06, 01)},

                        }},
                        new Room { RoomId = 2, RoomType = RoomType.SINGLE, Reservations = new List<Reservation>()},
                        new Room { RoomId = 3, RoomType = RoomType.SINGLE, Reservations = new List<Reservation>()},
                        new Room { RoomId = 4, RoomType = RoomType.SINGLE, Reservations = new List<Reservation>()},
                        new Room { RoomId = 5, RoomType = RoomType.SINGLE, Reservations = new List<Reservation>()},
                        new Room { RoomId = 6, RoomType = RoomType.SINGLE, Reservations = new List<Reservation>()},
                        new Room { RoomId = 7, RoomType = RoomType.SINGLE, Reservations = new List<Reservation>()},
                        new Room { RoomId = 8, RoomType = RoomType.DOUBLE, Reservations = new List<Reservation>()},
                        new Room { RoomId = 9, RoomType = RoomType.DOUBLE, Reservations = new List<Reservation>()},
                        new Room { RoomId = 10, RoomType = RoomType.DOUBLE, Reservations = new List<Reservation>()},
                    }
            });

            //instantiating business layer
            return new ReservationManagement(inMemoryDataManagement);
        }   
    }
}
