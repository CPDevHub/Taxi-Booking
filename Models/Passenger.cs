
using System.ComponentModel.DataAnnotations;

namespace Taxi_Booking.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }     
        
        public string? AvatarUrl { get; set; }         
        public double CancellationCharges { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Ride> PassengerRides { get; set; }
    }
}
