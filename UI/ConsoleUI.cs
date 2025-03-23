using Airport_Ticket_Booking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.UI
{
    class ConsoleUI
    {

        public static void ShowManagerMenu()
        {
            Console.WriteLine("\nManager Menu:");
            Console.WriteLine("1. Filter Bookings");
            Console.WriteLine("2. Import Flights from CSV");
            Console.WriteLine("3. Import Classes from CSV");
            Console.WriteLine("4. Exit");
            Console.Write("\nEnter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CallFilterBookings();
                    break;
                case 2:
                    ImportFlights();
                    break;
                case 3:
                    ImportClasses();
                    break;
                case 4:
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    ShowManagerMenu();
                    break;
            }
        }

        public static void ShowPassengerMenu()
        {
            Console.WriteLine("\nPassenger Menu:");
            Console.WriteLine("1. Search for Available Flights");
            Console.WriteLine("2. Book a Flight");
            Console.WriteLine("3. Manage Bookings");
            Console.WriteLine("4. Exit");
            Console.Write("\nEnter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    SearchForFlights();
                    break;
                case 2:
                    BookFlight();
                    break;
                case 3:
                    ManageBookings();
                    break;
                case 4:
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    ShowPassengerMenu();
                    break;
            }
        }

        static void CallFilterBookings()
        {
            Console.WriteLine("\n--- Filter Bookings ---");

            Console.Write("Enter Flight ID (or press Enter to skip): ");
            int? flightID = TryParseNullableInt(Console.ReadLine());

            Console.Write("Enter Price (or press Enter to skip): ");
            double? price = TryParseNullableDouble(Console.ReadLine());

            Console.Write("Enter Departure Country (or press Enter to skip): ");
            string departureCountry = ReadNullableString();

            Console.Write("Enter Destination Country (or press Enter to skip): ");
            string destinationCountry = ReadNullableString();

            Console.Write("Enter Departure Date (yyyy-MM-dd) (or press Enter to skip): ");
            DateTime? departureDate = TryParseNullableDate(Console.ReadLine());

            Console.Write("Enter Departure Airport (or press Enter to skip): ");
            string departureAirport = ReadNullableString();

            Console.Write("Enter Arrival Airport (or press Enter to skip): ");
            string arrivalAirport = ReadNullableString();

            Console.Write("Enter User ID (or press Enter to skip): ");
            int? userID = TryParseNullableInt(Console.ReadLine());

            Console.Write("Enter Flight Class (Economy, Business, First) (or press Enter to skip): ");
            string flightClass = ReadNullableString();

            BookingServices.FilterBookings(flightID, price, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, userID, flightClass);
        }

        // helper function (for menu ..)
        static int? TryParseNullableInt(string input)
        {
            return int.TryParse(input, out int value) ? value : (int?)null;
        }

        static double? TryParseNullableDouble(string input)
        {
            return double.TryParse(input, out double value) ? value : (double?)null;
        }

        static DateTime? TryParseNullableDate(string input)
        {
            return DateTime.TryParse(input, out DateTime value) ? value : (DateTime?)null;
        }

        static string ReadNullableString()
        {
            string input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? null : input;
        }

        static void ImportFlights()
        {
            Console.Write("\nEnter the path of the Flights CSV file: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found! Please enter a valid file path.");
                return;
            }

            FlightServices.ImportFlightsFromCSV(filePath);
        }

        static void ImportClasses()
        {
            Console.Write("\nEnter the path of the Classes CSV file: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found! Please enter a valid file path.");
                return;
            }

            FlightServices.ImportClassesFromCSV(filePath);
        }

        static void SearchForFlights()
        {
            Console.Write("\nEnter Departure Country: ");
            string departureCountry = Console.ReadLine();

            Console.Write("Enter Destination Country: ");
            string destinationCountry = Console.ReadLine();

            Console.Write("Enter Departure Date (yyyy-MM-dd): ");
            string departureDate = Console.ReadLine();

            // Call the function
            FlightServices.SearchFlights(departureCountry, destinationCountry, departureDate);
        }

        public static void BookFlight()
        {
            Console.Write("Enter Flight ID: ");
            int flightId;
            while (!int.TryParse(Console.ReadLine(), out flightId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Flight ID.");
            }

            Console.Write("Enter Class Type (Economy, Business, First Class): ");
            string classType = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(classType))
            {
                Console.WriteLine("Class type cannot be empty.");
                return;
            }

            BookingServices.BookFlight(flightId, classType, Session.UserId);
        }

        public static void ManageBookings()
        {

            int userId = Session.UserId;

            while (true)
            {
                Console.WriteLine("\n--- Manage Bookings ---");
                Console.WriteLine("1. View My Bookings");
                Console.WriteLine("2. View Specific Booking");
                Console.WriteLine("3. Cancel Booking");
                Console.WriteLine("4. Modify Booking Class");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BookingServices.GetUserBooking(userId);
                        break;

                    case "2":
                        Console.Write("Enter Booking ID: ");
                        int bookingId;
                        if (int.TryParse(Console.ReadLine(), out bookingId))
                        {
                            BookingServices.GetSpesificBooking(bookingId, userId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Booking ID. Please enter a number.");
                        }
                        break;

                    case "3":
                        Console.Write("Enter Booking ID to cancel: ");
                        if (int.TryParse(Console.ReadLine(), out bookingId))
                        {
                            BookingServices.CancelBooking(bookingId, userId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Booking ID. Please enter a number.");
                        }
                        break;

                    case "4":
                        Console.Write("Enter Booking ID to modify: ");
                        if (int.TryParse(Console.ReadLine(), out bookingId))
                        {
                            Console.Write("Enter New Class (Economy, Business, First Class): ");
                            string className = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(className))
                            {
                                BookingServices.ModifyBooking(bookingId, userId, className);
                            }
                            else
                            {
                                Console.WriteLine("Class type cannot be empty.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Booking ID. Please enter a number.");
                        }
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }


    }
}
