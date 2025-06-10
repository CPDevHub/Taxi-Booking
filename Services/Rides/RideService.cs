using Taxi_Booking.DTO;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Repositories.Rides;

namespace Taxi_Booking.Services.Rides
{
    public class RideService:IRideService
    {
        private readonly IRideRepository _rideRepository;
        private readonly ILogger<RideService> _logger;
        public RideService(IRideRepository rideRepository, ILogger<RideService> logger)
        {
            _rideRepository = rideRepository;
            _logger = logger;
        }
        public Task<Ride> CreateRide(CreateRideRequestDto rideRequest)
        {
            return _rideRepository.CreateRide(rideRequest);
        }
        public Task<Ride> GetRideByID(int rideId)
        {
            return _rideRepository.GetRideByID(rideId);
        }
        public Task<Boolean> UpdateRide(Ride ride)
        {
            return _rideRepository.UpdateRide(ride);
        }
    }
}
