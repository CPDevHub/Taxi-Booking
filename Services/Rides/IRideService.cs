using Taxi_Booking.DTO;
using Taxi_Booking.Models;

namespace Taxi_Booking.Services.Rides
{
    public interface IRideService
    {
        public Task<Ride> CreateRide(CreateRideRequestDto rideRequest);
        public Task<Ride> GetRideByID(int rideId);
        public Task<Boolean> UpdateRide(Ride ride);

        Task<List<RideHistoryDto>> GetHistoryDriverAsync(int driverId);
        Task<List<RideHistoryDto>> GetHistoryPassengerAsync(int passengerId);
    }
}
