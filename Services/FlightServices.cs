using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Services
{
    class FlightServices
    {
        private const string FlightsFile = "C:\\Users\\pc\\source\\repos\\Airport Ticket Booking\\Data\\flights.csv";

        public static List<Flight> SearchFlights(string DepartureCountry, string DestinationCountry, string DepartureDate)
        {
            List<Flight> flights = FileHandler.ReadFlights(FlightsFile);
            List<Flight> result = (from flight in flights
                                   where flight.DepartureCountry.Equals(DepartureCountry, StringComparison.OrdinalIgnoreCase)
                                   && flight.DestinationCountry.Equals(DestinationCountry, StringComparison.OrdinalIgnoreCase)
                                   && flight.DepartureDate.Equals(DateTime.Parse(DepartureDate))
                                   && flight.DepartureDate >= DateTime.Now
                                   select flight).ToList();
            return result;
        }

        public static List<string> ValidateFlight(Flight flight, int rowNumber)
        {
            List<string> errors = new List<string>();

            if (flight.FlightID <= 0)
                errors.Add($"Row {rowNumber}: Invalid Flight ID.");

            if (string.IsNullOrWhiteSpace(flight.DepartureCountry))
                errors.Add($"Row {rowNumber}: Departure Country is required.");

            if (string.IsNullOrWhiteSpace(flight.DestinationCountry))
                errors.Add($"Row {rowNumber}: Destination Country is required.");

            if (string.IsNullOrWhiteSpace(flight.DepartureAirport))
                errors.Add($"Row {rowNumber}: Departure Airport is required.");

            if (string.IsNullOrWhiteSpace(flight.ArrivalAirport))
                errors.Add($"Row {rowNumber}: Arrival Airport is required.");

            if (flight.DepartureDate < DateTime.Now)
                errors.Add($"Row {rowNumber}: Departure Date cannot be in the past.");

            return errors;
        }

        public static List<string> ValidateClass(Class flightClass, List<Flight> flights, int rowNumber)
        {
            List<string> errors = new List<string>();

            if (!flights.Any(f => f.FlightID == flightClass.FlightID))
                errors.Add($"Row {rowNumber}: Flight ID {flightClass.FlightID} does not exist in flights.csv.");

            if (string.IsNullOrWhiteSpace(flightClass.ClassType) || !(flightClass.ClassType == "Economy" || flightClass.ClassType == "Business" || flightClass.ClassType == "First Class"))
                errors.Add($"Row {rowNumber}: ClassType must be Economy, Business, or First Class.");

            if (flightClass.Price <= 0)
                errors.Add($"Row {rowNumber}: Price must be a positive number.");

            if (flightClass.SeatsAvailable <= 0)
                errors.Add($"Row {rowNumber}: Seats Available must be a positive number.");

            return errors;
        }


    }
}