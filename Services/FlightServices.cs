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

        public static List<Flight> SearchFlights(string DepartureCountry, string DestinationCountry, string DepartureDate)
        {
            List <Flight> flights = FileHandler.ReadFlights(FileHandler.FlightsFile);
            List<Flight> result = (from flight in flights
                                   where flight.DepartureCountry.Equals(DepartureCountry, StringComparison.OrdinalIgnoreCase)
                                   && flight.DestinationCountry.Equals(DestinationCountry, StringComparison.OrdinalIgnoreCase)
                                   && flight.DepartureDate.Equals(DateTime.Parse(DepartureDate))
                                   && flight.DepartureDate >= DateTime.Now
                                   select flight).ToList();
            return result;
        }
    }
}
