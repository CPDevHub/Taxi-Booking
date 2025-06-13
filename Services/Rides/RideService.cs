using AutoMapper;
using Taxi_Booking.DTO;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Repositories.Rides;

namespace Taxi_Booking.Services.Rides
{
    public class RideService:IRideService
    {
        private readonly IRideRepository _rideRepository;
        private readonly ILogger<RideService> _logger;
        private readonly IMapper _mapper;
        public RideService(IRideRepository rideRepository, ILogger<RideService> logger, IMapper mapper)
        {
            _rideRepository = rideRepository;
            _logger = logger;
            _mapper = mapper;
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

        public async Task<List<RideHistoryDto>> GetHistoryDriverAsync(int driverId)
        {
            var rides = await _rideRepository.GetDriverHistoryAsync(driverId);
            return _mapper.Map<List<RideHistoryDto>>(rides);
        }

        public async Task<List<RideHistoryDto>> GetHistoryPassengerAsync(int passengerId)
        {
            var rides = await _rideRepository.GetPassengerHistoryAsync(passengerId);
            return _mapper.Map<List<RideHistoryDto>>(rides);
        }
    }
}
