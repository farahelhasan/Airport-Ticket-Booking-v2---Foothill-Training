using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Models
{
    class Class
    {
        public int FlightID { set; get; }
        public double Price { set; get; }
        public string ClassType { set; get; }
        public int SeatsAvailable { set; get; }

        public override string ToString()
        {
            return $"FlightID: {FlightID} | Price: {Price} | ClassType: {ClassType} | SeatsAvailable: {SeatsAvailable}";
        }

    }
}
