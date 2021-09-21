using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelBooking.Interfaces;
using HotelBooking.Services;
using HotelBooking.Models;
using System.Collections.Generic;
using System;

namespace HotelBooking.Test.Services
{
    [TestClass]
    public class InMemoryDataManagementTest
    {
        private IDataManagement _dataManagement;
        private Hotel _hotel;


        [TestInitialize]
        public void Setup()
        {
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

            _dataManagement = new InMemoryDataManagement(_hotel);  
        }

        [TestMethod]
        public void GetAllRoomsReturnsAllRooms()
        {
            var rooms = _dataManagement.GetAllRooms();

            Assert.IsNotNull(rooms);
            Assert.IsInstanceOfType(rooms, typeof(List<Room>));
            Assert.AreEqual(rooms.Count, 10);
        }

        [TestMethod]
        public void CreateReservationAddsSingleRoomBookingToTheList()
        {
            var isBooked = _dataManagement.CreateReservation(new Room {
                RoomId = 1,
                IsAvailable = true,
                RoomType = RoomType.SINGLE,
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        StartDate = new DateTime(2022, 05, 01),
                        EndDate = new DateTime(2022, 05, 11),
                    }
                }
            });

            Assert.IsNotNull(isBooked);
            Assert.IsInstanceOfType(isBooked, typeof(bool));
            Assert.IsTrue(isBooked);
        }

        [TestMethod]
        public void CreateReservationAddsDoubleRoomBookingToTheList()
        {
            var isBooked = _dataManagement.CreateReservation(new Room
            {
                RoomId = 10,
                IsAvailable = true,
                RoomType = RoomType.DOUBLE,
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        StartDate = new DateTime(2022, 05, 01),
                        EndDate = new DateTime(2022, 05, 11),
                    }
                }
            });

            Assert.IsNotNull(isBooked);
            Assert.IsInstanceOfType(isBooked, typeof(bool));
            Assert.IsTrue(isBooked);
        }

        [TestMethod]
        public void CreateReservationAddsDoubleRoomBookingIntoSingleRoomReturnsError()
        {
            var isBooked = _dataManagement.CreateReservation(new Room
            {
                RoomId = 3,
                IsAvailable = true,
                RoomType = RoomType.DOUBLE,
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        StartDate = new DateTime(2022, 05, 01),
                        EndDate = new DateTime(2022, 05, 11),
                    }
                }
            });

            Assert.IsNotNull(isBooked);
            Assert.IsInstanceOfType(isBooked, typeof(bool));
            Assert.IsFalse(isBooked);
        }
    }
}
