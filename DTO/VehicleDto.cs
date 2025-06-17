using Taxi_Booking.Models;

namespace Taxi_Booking.DTO
{
    public class VehicleDto
    {
        public string Number { get; set; }
        public VehicleType Type { get; set; }
        public string Model { get; set; }
    }
}
