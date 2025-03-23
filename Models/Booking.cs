using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Models
{
    class Booking
    {
        private static int nextId = 1;  // Static counter shared across all instances

        public int BookingID { set; get; }

        public int FlightID { set; get; }
        public int UserID { set; get; }

        public String Class { set; get; }
        public double Price { set; get; }

        public Booking()
        {
            BookingID = nextId++;  // Assign and increment( as auto increment)
        }
        public override string ToString()
        {
            return $"BookingID: {BookingID} | FlightID: {FlightID} | UserID: {UserID} | Class: {Class} | Price: {Price}";
        }

    }
}
