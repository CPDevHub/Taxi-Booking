using Taxi_Booking.Models;

namespace Taxi_Booking.DTO.Responses
{
    public class PassengerResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string ContactNumber { get; set; }
        public ICollection<Ride> PassengerRides { get; set; }
    }
}
