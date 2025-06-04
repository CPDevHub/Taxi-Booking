using System.ComponentModel.DataAnnotations;
using Taxi_Booking.Models.Enums;

namespace Taxi_Booking.Models.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public VehicleType Type { get; set; }

        public string Model { get; set; }
    }
}
