using System.ComponentModel.DataAnnotations;

namespace Taxi_Booking.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public VehicleType Type { get; set; }

        public string Model { get; set; }
    }
}
