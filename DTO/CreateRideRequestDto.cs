using Taxi_Booking.Models;

namespace Taxi_Booking.DTO
{
    public class CreateRideRequestDto
    {
        public Location PickupLocation { get; set; }
        public Location DropOffLocation { get; set; }
        public int PassengerId { get; set; }
        public VehicleType RideVehicle { get; set; }
    }
}
