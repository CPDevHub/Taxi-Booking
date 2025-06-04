using Taxi_Booking.Models.Entities;

namespace Taxi_Booking.DTO.Responses
{
    public class DriverResponseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int TotalRides { get; set; }
        public double Rating { get; set; }
        public ICollection<Ride> DriverRides { get; set; }

    }
}
