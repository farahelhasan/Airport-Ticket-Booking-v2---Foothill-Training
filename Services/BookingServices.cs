using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Services
{
    class BookingServices
    {
    
        public static void BookFlight(int flightId, string classType, int userId)
        {
            if (CheckAvailableSeat(flightId, classType))
            {
                // git the price for the class 
                double price = GetPrice(flightId, classType);
                Flight flightToBook = FlightServices.GetFlight(flightId);
                bool availableSeat = CheckAvailableSeat(flightId, classType);

                if (flightToBook != null && price > 0)
                {
                    Booking booking = new Booking { FlightID = flightToBook.FlightID, Class = classType, UserID = userId, Price = price };
                    FileHandler.SaveBooking(booking);
                    DecreaseSeatsAvailable(flightId, classType);

                    Console.WriteLine("Booking Successful!");
                    Console.WriteLine(booking);

                }
                else
                {
                    Console.WriteLine("Enter vaild FlightId...");

                }
            }
            else
            {
                Console.WriteLine($"Sorry, you cann't book this flight with ID: {flightId} with class: {classType}, because its full! ");
            }
        }

        public static void GetUserBooking(int userId)
        {
            List<Booking> bookings = FileHandler.ReadBookings();

            List<Booking> selectedBookings = (from booking in bookings
                                      where booking.UserID == userId
                                      select booking).ToList();

            if (selectedBookings.Count() < 1)
            {
                Console.WriteLine("You haven't booked any flights yet.");
            }
            else
            {
                foreach(Booking book in selectedBookings)
                {
                    Console.WriteLine(book);
                }
            }
        }

        public static void GetSpesificBooking(int bookingId, int userId)
        {
            List<Booking> bookings = FileHandler.ReadBookings();

            Booking selectedBooking = (from booking in bookings
                                              where booking.BookingID == bookingId
                                              && booking.UserID == userId
                                              select booking).FirstOrDefault();

            if (selectedBooking == null)
            {
                Console.WriteLine("Enter Vaild BookingId.");
            }
            else
            {
                    Console.WriteLine(selectedBooking);
            }
        }

        public static void CancelBooking(int bookingId, int userId)
        {
            List<Booking> bookings = FileHandler.ReadBookings();

            Booking selectedBooking = (from booking in bookings
                                       where booking.BookingID == bookingId
                                       && booking.UserID == userId
                                       select booking).FirstOrDefault();

            if (selectedBooking == null)
            {
                Console.WriteLine("Enter Vaild BookingId.");
            }
            else
            {
                bookings.Remove(selectedBooking);
                FileHandler.EditBooking(bookings);
                Console.WriteLine($"Bookiing with ID: {selectedBooking.BookingID} deleted successfully! ");
            }
        }

        public static void ModifyBooking(int bookingId, int userId, string className)
        {
            List<Booking> bookings = FileHandler.ReadBookings();

            Booking selectedBooking = (from booking in bookings
                                       where booking.BookingID == bookingId
                                       && booking.UserID == userId
                                       select booking).FirstOrDefault();

            if (selectedBooking == null)
            {
                Console.WriteLine("Enter Vaild BookingId.");
                return;
            }
            // check if the new class has available seats
            if(!CheckAvailableSeat(selectedBooking.FlightID, className))
            {
                Console.WriteLine("No available seats in the selected class.");
                return;
            }

            // we should increase available seats for old class 
            IncreaseSeatsAvailable(selectedBooking.FlightID, selectedBooking.Class);
            // decreasing modified class
            DecreaseSeatsAvailable(selectedBooking.FlightID, className);
           
            selectedBooking.Class = className;

            FileHandler.EditBooking(bookings);
            Console.WriteLine($"Bookiing with ID: {selectedBooking.BookingID} modified successfully! ");
            Console.WriteLine(selectedBooking);


        }

        private static double GetPrice(int flightId, string className)
        {
            List<Class> classes = FileHandler.ReadClasses(FileHandler.ClassesFile);

            Class selectedClass = (from classType in classes
                                   where classType.FlightID == flightId
                                   && className.Equals(classType.ClassType, StringComparison.OrdinalIgnoreCase)
                                   select classType).FirstOrDefault();
            return selectedClass?.Price ?? -1;

        }

        private static void DecreaseSeatsAvailable(int flightId, string className)
        {
            List<Class> classes = FileHandler.ReadClasses(FileHandler.ClassesFile);

            Class selectedClass = (from classType in classes
                                   where classType.FlightID == flightId
                                   && className.Equals(classType.ClassType, StringComparison.OrdinalIgnoreCase)
                                   select classType).FirstOrDefault();

            if (selectedClass == null)
            {
                Console.WriteLine("Class not found.");
                return;
            }

            selectedClass.SeatsAvailable--;
            FileHandler.EditClasses(classes);
            Console.WriteLine($"Available Seats for {selectedClass.ClassType} class decreasing successfully: {selectedClass.SeatsAvailable}");
        }

        private static bool CheckAvailableSeat(int flightId, string className)
        {
            List<Class> classes = FileHandler.ReadClasses(FileHandler.ClassesFile);

            Class selectedClass = (from classType in classes
                                   where classType.FlightID == flightId
                                   && className.Equals(classType.ClassType, StringComparison.OrdinalIgnoreCase)
                                   select classType).FirstOrDefault();

            if (selectedClass == null)
            {
                Console.WriteLine("Class not found.");
                return false;
            }

            if (selectedClass.SeatsAvailable > 0)
            {
                return true;
            }
            else
            {
                //Console.WriteLine("Sorry, you cann't modifiy class, No seats available.");
                return false;
            }
        }

        private static void IncreaseSeatsAvailable(int flightId, string className)
        {
            List<Class> classes = FileHandler.ReadClasses(FileHandler.ClassesFile);

            Class selectedClass = (from classType in classes
                                   where classType.FlightID == flightId
                                   && className.Equals(classType.ClassType, StringComparison.OrdinalIgnoreCase)
                                   select classType).FirstOrDefault();

            if (selectedClass == null)
            {
                Console.WriteLine("Class not found.");
                return;
            }

            selectedClass.SeatsAvailable++;
            FileHandler.EditClasses(classes);
            Console.WriteLine($"Available Seats for {selectedClass.ClassType} class increasing successfully: {selectedClass.SeatsAvailable}");

        }

        public static void FilterBookings(
        int? flightID = null,
        double? price = null,
        string departureCountry = null,
        string destinationCountry = null,
        DateTime? departureDate = null,
        string departureAirport = null,
        string arrivalAirport = null,
        int? userID = null,
        string flightClass = null)
        {
            List<Booking> bookings = FileHandler.ReadBookings();
            List<Flight> flights = FileHandler.ReadFlights(FileHandler.FlightsFile);

            // Join bookings with flights to get complete details
            var filteredBookings = from booking in bookings
                                   join flight in flights on booking.FlightID equals flight.FlightID
                                   where
                                       (flightID == null || booking.FlightID == flightID) &&
                                       (price == null || booking.Price == price) &&
                                       (string.IsNullOrEmpty(departureCountry) || flight.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase)) &&
                                       (string.IsNullOrEmpty(destinationCountry) || flight.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)) &&
                                       (departureDate == null || flight.DepartureDate.Date == departureDate.Value.Date) &&
                                       (string.IsNullOrEmpty(departureAirport) || flight.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase)) &&
                                       (string.IsNullOrEmpty(arrivalAirport) || flight.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
                                       (userID == null || booking.UserID == userID) &&
                                       (string.IsNullOrEmpty(flightClass) || booking.Class.Equals(flightClass, StringComparison.OrdinalIgnoreCase))
                                   select new
                                   {
                                       booking.BookingID,
                                       booking.UserID,
                                       booking.Class,
                                       booking.Price,
                                       flight.FlightID,
                                       flight.DepartureCountry,
                                       flight.DestinationCountry,
                                       flight.DepartureAirport,
                                       flight.ArrivalAirport,
                                       flight.DepartureDate
                                   };

            if (filteredBookings.Count() < 1)
            {
                Console.WriteLine("No bookings match the provided criteria.");
                return;
            }

            // print bookings
            Console.WriteLine("\nFiltered Bookings:");
            Console.WriteLine("--------------------------------------------------------------------");
            foreach (var booking in filteredBookings)
            {
                Console.WriteLine($"Booking ID: {booking.BookingID}");
                Console.WriteLine($"Passenger ID: {booking.UserID}");
                Console.WriteLine($"Class: {booking.Class}");
                Console.WriteLine($"Price: {booking.Price}");
                Console.WriteLine($"Flight ID: {booking.FlightID}");
                Console.WriteLine($"Departure Country: {booking.DepartureCountry}");
                Console.WriteLine($"Destination Country: {booking.DestinationCountry}");
                Console.WriteLine($"Departure Airport: {booking.DepartureAirport}");
                Console.WriteLine($"Arrival Airport: {booking.ArrivalAirport}");
                Console.WriteLine($"Departure Date: {booking.DepartureDate}");
                Console.WriteLine("--------------------------------------------------------------------");
            }
        }

    }
}
