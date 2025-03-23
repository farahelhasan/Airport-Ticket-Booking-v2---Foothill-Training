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
    
        public static Booking BookFlight(int flightId, string classType, int userId)
        {
            // git the price for the class 
            double proice = FlightServices.GetPrice(flightId, classType);

            Flight flightToBook = FlightServices.GetFlight(flightId);
           
            if(flightToBook != null && proice > 0)
            {
                Booking booking = new Booking { BookingID = 1, FlightID = flightToBook.FlightID, Class = classType, UserID = userId, Price = proice };
                FileHandler.SaveBooking(booking);
                FlightServices.DecreaseSeatsAvailable(flightId, classType);
                return booking;
            }
            return null;
        }

       
    }
}
