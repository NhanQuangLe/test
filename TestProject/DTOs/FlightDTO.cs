using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.DTOs
{
    internal class FlightDTO
    {
        public string Id { get; set; }
        public string Airline { get; set; }
        public string Local_depart { get; set; }
        public string originAirport { get; set; }
        public string Local_arrive { get; set; }
        public string destinationAirport { get; set; }
    }
}
