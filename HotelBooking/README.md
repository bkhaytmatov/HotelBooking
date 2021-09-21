# Project - Hotel Coding Exercise

This is a solution to Candiadte Take Home Exercise for Rome2rio's Software Engineering position.
The project consist of:
--Interfaces for Data layer and Business layer, in order to have a high level outlook at project structure,
--Data layer "InMemoryDataManagement.cs" (responsible for only date manipulation like Room creation or Getting all rooms),
--Business layer "ReservationManagement.cs" (responsible for the logic of methods in Data Layer),
--Models (consist of Booking, Hotel, Room and Reservation models and required methods within)
--Startup class (responsible for setting up a Hotel with 7 single rooms, 3 double rooms and few reservations)
--Unit tests for Data layer, Business layer

## How to test

For the testing purpose the unit tests have been written, but also the main Program.cs consist few possible entries.
Feel free to debug in order to see how the objects are getting created and bookings are made.

### Assumptions and design decisions

Firstly I analysed types of objects to use in the application:
--Hotel (to store hotel data and list of rooms)
--Room (to store room data and list of reservations)
--Reservation (to store checkIn and checkOut times or StartDate and EndDate respectively)
--Booking (to pass into hotel everytime user makes a booking)

Assuming that:
--Partial bookings across more than one room are not possible

Problems that required thinking:
1. How to get all available rooms?
2. How to insert a new booking into available windows?

Solutions for problems above:
1. Implement getAllRooms method -resolved within first 30mins after setting up a structure
2. Main consern with insertion is finding an available window, discussions on this are described below

Finding an available window:
I considered 3 methods to resolve this:
1. Brute force - when I will loop through all the bookings of all rooms until I find the window that fits new booking.
However this approach was saved for later in case I don't come up with better solution.
2. Find a subset of bookings by month - find two bookings within month of new booking and check 
--if startDate of new booking is after endDate of earlier booking
--if endDate of new booking is before startDate of ladder booking
3. Implement a binaryTree to store a booking data and binarySearch to find a closest bookings by startDate and validate new bookings dates.
This improves time complexity during insertion. But this was decided to be an overengineering, so I went with a second approach.

Structure:
1. Interfaces are added to improve readability and containability of the programm
2. Single Responsibility principle and Separation of concern is used while writing methods and creating objects
3. Dependency injections is handled by businessLayer


#### Time took to complete
In order to get a proper way I have decided to brush up my C# skills and implement the solution in C#.
With Unit tests creation this solution took me around 4 hours to finish up.