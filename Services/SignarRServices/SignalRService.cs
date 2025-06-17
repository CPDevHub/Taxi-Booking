using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Taxi_Booking.Helpers;
using Taxi_Booking.Hubs;
using Taxi_Booking.Models;
using Taxi_Booking.Services.Drivers;

namespace Taxi_Booking.Services.SignarRServices
{
    public class SignalRService:ISignalRService
    {
        private readonly IHubContext<TaxiBookingHub> _hubContext;
        private readonly IDriverService _driverService;
        private readonly IMapper _mapper;
        private readonly ILogger<SignalRService> _logger;

        public SignalRService(IHubContext<TaxiBookingHub> hubContext, IDriverService driverService ,IMapper mapper, ILogger<SignalRService> logger)
        {
            _hubContext = hubContext;
            _driverService = driverService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task NotifyNearByDrivers(Ride ride)
        {

            var cancelledDrivers = TaxiBookingHub._rideCancelledDrivers.ContainsKey(ride.Id)
        ? TaxiBookingHub._rideCancelledDrivers[ride.Id]
        : new List<int>();

            var nearbyDrivers = TaxiBookingHub._availableConnections.Where(driver =>
            GeoUtils.GetDistanceInKm(
                driver.Value.Location,_mapper.Map<LatLng>(ride.PickupLocation)
            ) <= 5 &&
            !cancelledDrivers.Contains(driver.Value.DriverId)).ToList();

            foreach (var driver in nearbyDrivers)
            {
                _logger.LogInformation($"Checking driver: {driver.Value.DriverId}");
                var driverEntity = await _driverService.GetDriverWithVehicleByIdAsync(driver.Value.DriverId);
                if (driverEntity.Status != DriverStatus.Available) continue;
                if (driverEntity.DriverVehicle.Type != ride.RideVehicle) continue;

                _logger.LogInformation($"Driver {driver.Value.DriverId} is eligible. Sending ride request.");
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
                    },
                    fare=ride.TotalFare
                });

                if (!TaxiBookingHub._userRideAvailableDrivers.ContainsKey(ride.Id))
                    TaxiBookingHub._userRideAvailableDrivers[ride.Id] = new();

                TaxiBookingHub._userRideAvailableDrivers[ride.Id].Add(driver.Value.DriverId);

                }
            
        }
    }
}
