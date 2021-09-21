using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Interfaces;
using HotelBooking.Models;
using HotelBooking.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HotelBooking.Test.Services
{
    [TestClass]
    public class ReservationManagementTest
    {
        private Mock<IDataManagement> _dataManagement;
        private IReservationManagement _reservationmanagement;
        private Hotel _hotel;

        [TestInitialize]
        public void Setup()
        {
            _dataManagement = new Mock<IDataManagement>();
            _hotel = new Hotel
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
            };

            _reservationmanagement = new ReservationManagement(_dataManagement.Object);
        }

        [TestMethod]
        public void AvailableDateInFutureReturns10AvailableRooms()
        {
            _dataManagement.Setup(x => x.GetAllRooms()).Returns(_hotel.Rooms);

            var availableRooms = _reservationmanagement.GetRoomAvailability(new Reservation
            {
                StartDate = new DateTime(2022, 05, 01),
                EndDate = new DateTime(2022, 05, 11)
            });

            Assert.IsNotNull(availableRooms);
            Assert.IsInstanceOfType(availableRooms, typeof(List<Room>));
            Assert.AreEqual(availableRooms.Count, 10);
        }

        [TestMethod]
        public void AvailableDateInFutureReturns9AvailableRooms()
        {
            _dataManagement.Setup(x => x.GetAllRooms()).Returns(_hotel.Rooms);

            var rooms = _reservationmanagement.GetRoomAvailability(new Reservation
            {
                StartDate = new DateTime(2022, 05, 15),
                EndDate = new DateTime(2022, 05, 20)
            });

            var unavailableRoom = rooms.Where(x => !x.IsAvailable).First();
            
            Assert.IsNotNull(rooms);
            Assert.IsInstanceOfType(rooms, typeof(List<Room>));
            Assert.IsNotNull(unavailableRoom);
            Assert.IsInstanceOfType(unavailableRoom, typeof(Room));
            Assert.IsFalse(unavailableRoom.IsAvailable);
            Assert.AreEqual(rooms.Count, 10);
        }

        [TestMethod]
        public void StartDateInPastReturnsNull()
        {
            var availableRooms = _reservationmanagement.GetRoomAvailability(new Reservation
            {
                StartDate = new DateTime(2020, 07, 01),
                EndDate = new DateTime(2020, 07, 11)
            });

            Assert.IsNull(availableRooms);
        }

        [TestMethod]
        public void EndDateBeforeStartDateReturnsNull()
        {
            var availableRooms = _reservationmanagement.GetRoomAvailability(new Reservation
            {
                StartDate = new DateTime(2022, 08, 01),
                EndDate = new DateTime(2022, 07, 11)
            });

            Assert.IsNull(availableRooms);
        }

        [TestMethod]
        public void ReservationDatesInWrongFormatFailsWithMessage()
        {
            var bookingResult = _reservationmanagement.BookRoom(new Booking
            {
                StartDate = new DateTime(2020, 08, 01),
                EndDate = new DateTime(2020, 07, 11)
            });

            Assert.IsNotNull(bookingResult);
            Assert.IsInstanceOfType(bookingResult, typeof(Booking));
            Assert.AreEqual(bookingResult.Message, "Date(s) is/are not in the correct format.");
            Assert.IsFalse(bookingResult.IsBooked);
        }

        [TestMethod]
        public void SuccessfullyRegisteredBookingHappyPath()
        {
            _dataManagement.Setup(x => x.GetAllRooms()).Returns(_hotel.Rooms);
            _dataManagement.Setup(x => x.CreateReservation(It.IsAny<Room>())).Returns(true);

            var bookingResult = _reservationmanagement.BookRoom(new Booking
            {
                StartDate = new DateTime(2022, 05, 01),
                EndDate = new DateTime(2022, 05, 11),
                RoomType = RoomType.SINGLE
            });

            Assert.IsNotNull(bookingResult);
            Assert.IsInstanceOfType(bookingResult, typeof(Booking));
            Assert.AreEqual(bookingResult.Message, "Successfully created booking.");
            Assert.IsTrue(bookingResult.IsBooked);
        }

        [TestMethod]
        public void DataLayerIsDownFailsWithMessage()
        {
            _dataManagement.Setup(x => x.GetAllRooms()).Returns(_hotel.Rooms);
            _dataManagement.Setup(x => x.CreateReservation(It.IsAny<Room>())).Returns(false);

            var bookingResult = _reservationmanagement.BookRoom(new Booking
            {
                StartDate = new DateTime(2022, 05, 01),
                EndDate = new DateTime(2022, 05, 11),
                RoomType = RoomType.SINGLE
            });

            Assert.IsNotNull(bookingResult);
            Assert.IsInstanceOfType(bookingResult, typeof(Booking));
            Assert.AreEqual(bookingResult.Message, "Could not register your booking, please try again later.");
            Assert.IsFalse(bookingResult.IsBooked);
        }


        [TestMethod]
        public void ProvidedDateIsNotAvailableForBookingFailsWithMessage()
        {
            var reservations = new List<Reservation>
            {
               new Reservation { StartDate = new DateTime(2022, 05, 12), EndDate = new DateTime(2022, 05, 15)},
               new Reservation { StartDate = new DateTime(2022, 05, 17), EndDate = new DateTime(2022, 05, 22)},
               new Reservation { StartDate = new DateTime(2022, 06, 01), EndDate = new DateTime(2022, 06, 01)}
            };

            // reserve all single rooms for specified dates
            for (int i = 0; i < 7; i++)
            {
                _hotel.Rooms[i].Reservations = reservations;
            }

            _dataManagement.Setup(x => x.GetAllRooms()).Returns(_hotel.Rooms);
            _dataManagement.Setup(x => x.CreateReservation(It.IsAny<Room>())).Returns(false);

            var bookingResult = _reservationmanagement.BookRoom(new Booking
            {
                StartDate = new DateTime(2022, 05, 01),
                EndDate = new DateTime(2022, 05, 22),
                RoomType = RoomType.SINGLE
            });

            Assert.IsNotNull(bookingResult);
            Assert.IsInstanceOfType(bookingResult, typeof(Booking));
            Assert.AreEqual(bookingResult.Message, "There are no rooms available for selected dates.");
            Assert.IsFalse(bookingResult.IsBooked);
        }
    }
}
