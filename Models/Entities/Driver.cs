using System.ComponentModel.DataAnnotations;
using Taxi_Booking.Models.Enums;

namespace Taxi_Booking.Models.Entities
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }           
        public int TotalRides { get; set; }        
        public double? Rating { get; set; }        
        public string AvatarUrl { get; set; }

        public string ContactNumber { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public DriverStatus Status { get; set; }
        public int VehicleId { get; set; }
        public Vehicle DriverVehicle { get; set; }
        public ICollection<Ride> DriverRides { get; set; }
    }
}
