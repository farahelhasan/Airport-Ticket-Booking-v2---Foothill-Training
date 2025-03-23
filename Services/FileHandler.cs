using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Services
{
    class FileHandler
    {
        public const string FlightsFile = "C:\\Users\\pc\\source\\repos\\Airport Ticket Booking\\Data\\flights.csv";
        public const string BookingsFile = "C:\\Users\\pc\\source\\repos\\Airport Ticket Booking\\Data\\booking.csv";
        public const string ClassesFile = "C:\\Users\\pc\\source\\repos\\Airport Ticket Booking\\Data\\flight_classes.csv";

        public static List<Flight> ReadFlights(string FlightsFile)
        {
            List<Flight> flights = new List<Flight>();

            if (File.Exists(FlightsFile))
            {
                var lines = File.ReadAllLines(FlightsFile).Skip(1); // Skip header

                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    if (values.Length == 6)
                    {
                        flights.Add(new Flight
                        {
                            FlightID = int.Parse(values[0]),
                            DepartureCountry = values[1],
                            DestinationCountry = values[2],
                            DepartureAirport = values[3],
                            ArrivalAirport = values[4],
                            DepartureDate = DateTime.Parse(values[5]),
                        });
                    }
                }
            }

            return flights;
        }
        public static List<Booking> ReadBookings()
        {
            List<Booking> bookings = new List<Booking>();

            if (File.Exists(BookingsFile))
            {
                var lines = File.ReadAllLines(BookingsFile).Skip(1); // Skip header

                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    if (values.Length == 5)
                    {
                        bookings.Add(new Booking
                        {
                            BookingID = int.Parse(values[0]),
                            FlightID = int.Parse(values[1]),
                            UserID = int.Parse(values[2]),
                            Class = values[3],
                            Price = double.Parse(values[4])
                        });
                    }
                }
            }
            return bookings;
        }

        public static List<Class> ReadClasses(string ClassesFile)
        {
            List<Class> classes = new List<Class>();

            if (File.Exists(ClassesFile))
            {
                var lines = File.ReadAllLines(ClassesFile).Skip(1); // Skip header

                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    if (values.Length == 4)
                    {
                        classes.Add(new Class
                        {
                            FlightID = int.Parse(values[0]),
                            ClassType = values[1],
                            Price = double.Parse(values[2]),
                            SeatsAvailable = int.Parse(values[3]),
                        });
                    }
                }
            }
            return classes;
        }

        public static void SaveBooking(Booking booking)
        {
            bool fileExists = File.Exists(BookingsFile);
            // using : keyword ensures that StreamWriter automatically closes the file
            // opens the file in append mode
            using (StreamWriter sw = new StreamWriter(BookingsFile, true))
            {
                if (!fileExists)
                {
                    sw.WriteLine("BookingID,FlightID,UserID,Class,Price");
                }
                sw.WriteLine($"{booking.BookingID},{booking.FlightID},{booking.UserID},{booking.Class},{booking.Price}");
            }
        }

        public static void SaveFlights(List<Flight> flights)
        {
            using (var writer = new StreamWriter(FlightsFile, true))
            {
                foreach (var flight in flights)
                {
                    writer.WriteLine($"{flight.FlightID},{flight.DepartureCountry},{flight.DestinationCountry},{flight.DepartureAirport},{flight.ArrivalAirport},{flight.DepartureDate}");
                }
            }
        }

        public static void SaveClasses(List<Class> classes)
        {
            using (var writer = new StreamWriter(ClassesFile, true))
            {
                foreach (var flightClass in classes)
                {
                    writer.WriteLine($"{flightClass.FlightID},{flightClass.ClassType},{flightClass.Price},{flightClass.SeatsAvailable}");
                }
            }
        }

        public static void EditClasses(List<Class> classes)
        {
            using (var writer = new StreamWriter(ClassesFile, false))
            {
                writer.WriteLine("FlightID,ClassType,Price,SeatsAvailable");

                foreach (var flightClass in classes)
                {
                    writer.WriteLine($"{flightClass.FlightID},{flightClass.ClassType},{flightClass.Price},{flightClass.SeatsAvailable}");
                }
            }
        }

        public static void EditBooking(List<Booking> bookings)
        {
            using (var writer = new StreamWriter(BookingsFile, false))
            {
                writer.WriteLine("BookingID,FlightID,UserID,Class,Price");

                foreach (var booking in bookings)
                {
                    writer.WriteLine($"{booking.BookingID},{booking.FlightID},{booking.UserID},{booking.Class},{booking.Price}");
                }
            }
        }


    }

}
