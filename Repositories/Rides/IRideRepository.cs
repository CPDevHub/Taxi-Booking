using Taxi_Booking.DTO;
using Taxi_Booking.Models;

namespace Taxi_Booking.Repositories.Rides
{
    public interface IRideRepository
    {
        public Task<Ride> CreateRide(CreateRideRequestDto rideRequest);
        public Task<Ride> GetRideByID(int rideId);
        public Task<Boolean> UpdateRide(Ride ride);

        Task<List<Ride>> GetDriverHistoryAsync(int driverId);
        Task<List<Ride>> GetPassengerHistoryAsync(int passengerId);
    }
}
