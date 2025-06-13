using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taxi_Booking.Context;
using Taxi_Booking.DTO;
using Taxi_Booking.Helpers;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Models.Enums;

namespace Taxi_Booking.Repositories.Rides
{
    public class RideRepository:IRideRepository
    {
        private readonly ILogger<RideRepository> _logger;
        private readonly TaxiBookingContext _bookingContext;
        private readonly IMapper _mapper;
        public RideRepository(TaxiBookingContext bookingContext, ILogger<RideRepository> logger, IMapper mapper)
        {
            _logger = logger;
            _bookingContext = bookingContext;
            _mapper = mapper;
        }
        public async Task<Ride> CreateRide(CreateRideRequestDto rideRequest)
        {
            
            double distance = GeoUtils.GetDistanceInKm(_mapper.Map<LatLng>(rideRequest.PickupLocation),_mapper.Map<LatLng>(rideRequest.DropOffLocation));
            var ratePerKm = (int)rideRequest.RideVehicle;
            var ride = new Ride
            {
                Status = RideStatus.Requested,
                RequestedAt = DateTime.UtcNow,
                PickupLocation = rideRequest.PickupLocation,
                DropOffLocation = rideRequest.DropOffLocation,
                PassengerId = rideRequest.PassengerId,
                TotalFare = distance * ratePerKm,
                RideVehicle=rideRequest.RideVehicle
            };
            _bookingContext.Ride.Add(ride);
            await _bookingContext.SaveChangesAsync();

            _logger.LogInformation("Ride booked with ID: {RideId}", ride.Id);
            return ride;
        }

        public async Task<Ride> GetRideByID(int rideId)
        {
            return await _bookingContext.Ride.FindAsync(rideId);
        }
        public async Task<Boolean> UpdateRide(Ride ride)
        {
            _bookingContext.Ride.Update(ride);
            await _bookingContext.SaveChangesAsync();
            return true;
        }


        public async Task<List<Ride>> GetDriverHistoryAsync(int driverId)
        {
            return await _bookingContext.Ride
                .Where(r => r.DriverId == driverId)
                .OrderByDescending(r => r.RideStartAt)
                .ToListAsync();
        }

        public async Task<List<Ride>> GetPassengerHistoryAsync(int passengerId)
        {
            return await _bookingContext.Ride
                .Where(r => r.PassengerId == passengerId)
                .OrderByDescending(r => r.RideStartAt)
                .ToListAsync();
        }
    }
}
