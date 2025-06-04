using Taxi_Booking.Models.Enums;

namespace Taxi_Booking.Models.Entities
{
    public class Ride
    {
        public int Id { get; set; }
        public RideStatus Status { get; set; }
        public double TotalFare { get; set; }
        public double CancellationCharges { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime RideStartAt { get; set; }
        public DateTime RideEndAt { get; set; }
        public Location PickupLocation { get; set; }
        public Location DropOffLocation { get; set; }
        public int PassengerId { get; set; }
        public int DriverId { get; set; }
        public Passenger RidePassenger { get; set; }
        public Driver RideDriver { get; set; }
    }
}
