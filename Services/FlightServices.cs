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
        
        public static void SearchFlights(string DepartureCountry, string DestinationCountry, string DepartureDate)
        {
            List <Flight> flights = FileHandler.ReadFlights(FileHandler.FlightsFile);
            List<Flight> result = (from flight in flights
                                   where flight.DepartureCountry.Equals(DepartureCountry, StringComparison.OrdinalIgnoreCase)
                                   && flight.DestinationCountry.Equals(DestinationCountry, StringComparison.OrdinalIgnoreCase)
                                   && flight.DepartureDate.Date.Equals(DateTime.Parse(DepartureDate))
                                   select flight).ToList();
            if (result.Count() < 1) 
            {
                Console.WriteLine("No flights match the provided criteria");
                return;
            }
            foreach(var flight in result)
            {
                Console.WriteLine(flight);
            }
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

        public static void ImportFlightsFromCSV(string ImportedFile)
        {
            List<Flight> flights = FileHandler.ReadFlights(ImportedFile);
            List<string> errors = new List<string>();
            List<Flight> validFlights = new List<Flight>();

            for (int i = 0; i < flights.Count; i++)
            {
                var flightErrors = ValidateFlight(flights[i], i + 1);
                if (flightErrors.Count > 0)
                {
                    errors.AddRange(flightErrors);
                }
                else
                {
                    validFlights.Add(flights[i]);  
                }
            }

            if (errors.Count > 0)
            {
                Console.WriteLine("Errors found in flight data:");
                foreach (var error in errors)
                    Console.WriteLine(error);
            }

            if (validFlights.Count > 0)
            {
                FileHandler.SaveFlights(validFlights);
                Console.WriteLine("Valid flights have been added successfully!");
            }
        }

        public static void ImportClassesFromCSV(string ImportedFile)
        {
            List<Flight> flights = FileHandler.ReadFlights(FileHandler.FlightsFile); 
            List<Class> classes = FileHandler.ReadClasses(ImportedFile);
            List<string> errors = new List<string>();
            List<Class> validClasses = new List<Class>();

            for (int i = 0; i < classes.Count; i++)
            {
                var classErrors = ValidateClass(classes[i], flights, i + 1);
                if (classErrors.Count > 0)
                {
                    errors.AddRange(classErrors);
                }
                else
                {
                    validClasses.Add(classes[i]);  
                }
            }

            if (errors.Count > 0)
            {
                Console.WriteLine("Errors found in class data:");
                foreach (var error in errors)
                    Console.WriteLine(error);
            }

            if (validClasses.Count > 0)
            {
                FileHandler.SaveClasses(validClasses);
                Console.WriteLine("Valid classes have been added successfully!");
            }
        }

        public static Flight GetFlight(int flightId)
        {
            List<Flight> flights = FileHandler.ReadFlights(FileHandler.FlightsFile);
            return (from flight in flights
                    where flight.FlightID == flightId
                    select flight).FirstOrDefault();
        }

      

    }
}

