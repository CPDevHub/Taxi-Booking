using Microsoft.AspNetCore.SignalR;
using Taxi_Booking.Helpers;
using Taxi_Booking.Hubs;
using Taxi_Booking.Models.Entities;

namespace Taxi_Booking.Services.SignarRServices
{
    public class SignalRService:ISignalRService
    {
        private readonly IHubContext<TaxiBookingHub> _hubContext;

        public SignalRService(IHubContext<TaxiBookingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyNearByDrivers(Ride ride)
        {
            var nearbyDrivers = TaxiBookingHub._availableConnections.Where(driver =>
            GeoUtils.GetDistanceInKm(
                driver.Value.Location,ride.PickupLocation
            ) <= 5 
        ).ToList();

            foreach (var driver in nearbyDrivers)
            {
                string connectionId = driver.Key;
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveRideRequest", new
                {
                    ride.Id,
                    PickupLocation = new
                    {
                        ride.PickupLocation?.Latitude,
                        ride.PickupLocation?.Longitude,
                        ride.PickupLocation?.Address
                    },
                    DropOffLocation = new
                    {
                        ride.DropOffLocation?.Latitude,
                        ride.DropOffLocation?.Longitude,
                        ride.DropOffLocation?.Address
                    }
                });

                if (!TaxiBookingHub._userRideAvailableDrivers.ContainsKey(ride.Id))
                    TaxiBookingHub._userRideAvailableDrivers[ride.Id] = new();

                TaxiBookingHub._userRideAvailableDrivers[ride.Id].Add((driver.Value.DriverId, driver.Value.Location));

            }
        }
    }
}
