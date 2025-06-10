using Microsoft.EntityFrameworkCore;
using Taxi_Booking.Context;
using Taxi_Booking.DTO;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Models.Enums;

namespace Taxi_Booking.Repositories.Rides
{
    public class RideRepository:IRideRepository
    {
        private readonly ILogger<RideRepository> _logger;
        private readonly TaxiBookingContext _bookingContext;
        public RideRepository(TaxiBookingContext bookingContext, ILogger<RideRepository> logger)
        {
            _logger = logger;
            _bookingContext = bookingContext;
        }
        public async Task<Ride> CreateRide(CreateRideRequestDto rideRequest)
        {
            var ride = new Ride
            {
                Status = RideStatus.Requested,
                RequestedAt = DateTime.UtcNow,
                PickupLocation = rideRequest.PickupLocation,
                DropOffLocation = rideRequest.DropOffLocation,
                PassengerId = rideRequest.PassengerId,
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
    }
}
