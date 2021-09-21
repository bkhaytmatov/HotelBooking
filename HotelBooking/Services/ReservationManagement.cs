using System;
using System.Linq;
using System.Collections.Generic;
using HotelBooking.Interfaces;
using HotelBooking.Models;

namespace HotelBooking.Services
{
    public class ReservationManagement : IReservationManagement
    {
        private readonly IDataManagement _inMemoryDataManagement;

        public ReservationManagement(IDataManagement inMemoryDataManagement)
        {
            _inMemoryDataManagement = inMemoryDataManagement;
        }

        public List<Room> GetRoomAvailability(Reservation reservation)
        {
            // check if datetime is in the past or enddate is before startdate
            if (reservation.StartDate < DateTime.Now || reservation.StartDate > reservation.EndDate)
            {
                return null;
            }

            var allRooms = _inMemoryDataManagement.GetAllRooms();

            if(allRooms?.Count > 0) { 

                var rooms = new List<Room>();

                // 10 rooms/10 steps
                foreach (var room in allRooms)
                {

                    // get all rooms that are within the range of startdate
                    var roomSubsetByStartDate = room.Reservations.Where(x => x.StartDate.Month == reservation.StartDate.Month
                                                                          && x.StartDate.Year == reservation.StartDate.Year).ToList();

                    // with this we check if enddate is in the same month
                    // as startdate, if it's not we also add range of dates
                    // that includes the month of enddate
                    if(reservation.StartDate.Month != reservation.EndDate.Month)
                    {
                        var roomSubsetByEndDate = room.Reservations.Where(x => x.EndDate.Month == reservation.EndDate.Month
                                                                            && x.StartDate.Year == reservation.StartDate.Year).ToList();
                        roomSubsetByStartDate.AddRange(roomSubsetByEndDate);
                    }

                    // check if there are no reservation at all for a given room, then it means we can add any reservation
                    // check of there are no reservations for requested month for a given room, then we can create one
                    if (roomSubsetByStartDate?.Count == 0)
                    {
                        rooms.Add(new Room
                        {
                            RoomId = room.RoomId,
                            RoomType = room.RoomType,
                            IsAvailable = true,
                            Reservations = new List<Reservation>()
                        });

                        continue;
                    }

                    var isAvailable = false;

                    // n steps from subset
                    for (int i = 0; i < roomSubsetByStartDate.Count; i++)
                    {
                        // first condition validates if reservation is after today and before first booking from the list
                        // second condition validates if reservation is after previous enddate and before next startdate
                        if (ReservationIsAfterTodayAndBeforeFirstBooking(reservation, roomSubsetByStartDate[i], i) ||
                            ReservationIsAfterPreviousAndBeforeCurrentBooking(reservation, roomSubsetByStartDate[i], roomSubsetByStartDate, i))
                        {
                            isAvailable = true;
                            rooms.Add(new Room
                            {
                                RoomId = room.RoomId,
                                RoomType = room.RoomType,
                                IsAvailable = true,
                                Reservations = roomSubsetByStartDate
                            });

                            break;
                        }
                    }

                    if (!isAvailable)
                    {
                        rooms.Add(new Room
                        {
                            RoomId = room.RoomId,
                            RoomType = room.RoomType,
                            IsAvailable = false,
                            Reservations = roomSubsetByStartDate
                        });
                    }

                }
                return rooms;
            }

            return null;
        }




        public Booking BookRoom(Booking booking)
        {
            // make sure dates are valid and aren't in the past
            if(booking?.StartDate < DateTime.Now || booking?.EndDate < DateTime.Now)
            {
                // invalid input
                return new Booking
                {
                    IsBooked = false,
                    Message = "Date(s) is/are not in the correct format."
                };
            }

            // get all available rooms for the date
            var roomsForTheDate = GetRoomAvailability(new Reservation
            {
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            });

            // select only room types that are neeeded
            var requiredRoomTypeList = roomsForTheDate.Where(x => x.RoomType == booking.RoomType && x.IsAvailable).ToList();

            // create a return object in advance
            var bookingResponse = new Booking
            {
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
            };


            //if we have at least 1 vacant room with needed room type
            if (requiredRoomTypeList?.Count > 0)
            {
                //take first room available
                var roomToReserve = requiredRoomTypeList.First();

                // call data layer to perform an insert
                var hasRoomRegistered = _inMemoryDataManagement.CreateReservation(new Room
                {
                    RoomId = roomToReserve.RoomId,
                    Reservations = new List<Reservation>
                        {
                            new Reservation
                            {
                                StartDate = booking.StartDate,
                                EndDate = booking.EndDate
                            }
                        },
                    RoomType = roomToReserve.RoomType
                });

                if (hasRoomRegistered)
                {
                    // booking created successfully
                    bookingResponse.IsBooked = true;
                    bookingResponse.RoomId = roomToReserve.RoomId;
                    bookingResponse.RoomType = roomToReserve.RoomType;
                    bookingResponse.Message = "Successfully created booking.";
                    return bookingResponse;
                }
                else
                {
                    // error in data layer
                    bookingResponse.IsBooked = false;
                    bookingResponse.RoomId = roomToReserve.RoomId;
                    bookingResponse.RoomType = roomToReserve.RoomType;
                    bookingResponse.Message = "Could not register your booking, please try again later.";
                    return bookingResponse;
                }
            }
            else
            {
                // no rooms available for booking
                bookingResponse.IsBooked = false;
                bookingResponse.Message = "There are no rooms available for selected dates.";
                return bookingResponse;
            }
        }

        // Helper methods 
        private bool ReservationIsAfterTodayAndBeforeFirstBooking(Reservation userReservation, Reservation existingReservation, int index)
        {
            return index == 0 && userReservation.StartDate > DateTime.Now
                        && userReservation.EndDate < existingReservation.StartDate;
        }

        private bool ReservationIsAfterPreviousAndBeforeCurrentBooking(Reservation userReservation, Reservation existingCurrentReservation, List<Reservation> existingPreviousReservation, int index)
        {
            return index != 0 && userReservation.StartDate > existingPreviousReservation[index - 1].EndDate
                        && userReservation.EndDate < existingCurrentReservation.StartDate;
        }
    }
}
