using Taxi_Booking.Models.Entities;
using Taxi_Booking.Models.Enums;

namespace Taxi_Booking.DTO
{
    public class CreateRideRequestDto
    {
        public Location PickupLocation { get; set; }
        public Location DropOffLocation { get; set; }
        public int PassengerId { get; set; }
        //public VehicleType PreferenceVehicle { get; set; }
    }
}
