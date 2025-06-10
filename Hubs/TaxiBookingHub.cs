using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using System.Diagnostics.Metrics;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using Taxi_Booking.Helpers;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Models.Enums;
using Taxi_Booking.Services.Drivers;
using Taxi_Booking.Services.Rides;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Taxi_Booking.Hubs
{
    public class TaxiBookingHub:Hub
    {
        private readonly IDriverService _driverService;
        private readonly IRideService _rideService;
        private readonly ILogger<TaxiBookingHub> _logger;
        public static Dictionary<string, (int DriverId, DriverLocation Location)> _availableConnections = new();
        public static Dictionary<int, List<(int DriverId, DriverLocation Location)>> _userRideAvailableDrivers = new();

        public TaxiBookingHub(IDriverService driverService, ILogger<TaxiBookingHub> logger,IRideService rideService)
        {
            _driverService = driverService;
            _logger = logger;
            _rideService = rideService;
        }
        //invoked when cleint connected to hub 
        //SignalR groups are connection-based, not user-based
        //When a client connects and joins a group(e.g., "AvailableDrivers"), it does so via its SignalR connection ID.
        //If that connection closes, due to:logout,network drop,tab closed ,crash, Then SignalR automatically removes that connection from any groups it was in.
        //On reconnect(after login or reconnection), SignalR creates a new ConnectionId, so you’ll need to re-add the driver to the group:
        //Hub instances are transient, created per connection.
        //Also, Hub instances cannot be injected into other services because their lifetime is tied to the SignalR pipeline and client calls.
        [Authorize]
        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected");
            return base.OnConnectedAsync();
        }


        public async Task LoginDriver(int driverId)
        {
            var driver = await _driverService.GetDriverByIdAsync(Convert.ToInt32(driverId));
            if (driver != null && driver.Status == DriverStatus.Available)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "AvailableDrivers");
                _logger.LogInformation("Driver {DriverId} added to group 'AvailableDrivers'", driverId);

                await Clients.Client(Context.ConnectionId).SendAsync("SendLocation");
                _logger.LogInformation("Sent initial SendLocation event to Driver {DriverId}", driverId);

            }
            
        }

        public async Task LoginPassenger(int passengerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{passengerId}");
            _logger.LogInformation("Passenger {UserId} added to group User_{UserId}", passengerId, passengerId);
        }


        public async Task UpdateLocation(DriverLocation dto)
        {
            var driverIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (driverIdStr != null)
            {
                int driverId = Convert.ToInt32(driverIdStr);
                await _driverService.UpdateDriverLocation(driverId, dto.Latitude, dto.Longitude);
                _availableConnections[Context.ConnectionId] = (driverId, dto);
                _logger.LogInformation("Updated location for Driver {DriverId}: ({Latitude}, {Longitude})", driverId, dto.Latitude, dto.Longitude);
            }
        }

        public async Task AcceptRide(int rideId)
        {
            var driverIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (driverIdStr == null)
            {
                _logger.LogWarning("AcceptRide failed: Driver not authenticated.");
                return;
            }
            int driverId = Convert.ToInt32(driverIdStr);
            Ride ride = await _rideService.GetRideByID(rideId);
            Driver driver = await _driverService.GetDriverWithVehicleByIdAsync(driverId);
            if (ride == null || ride.Status != RideStatus.Requested)
            {
                _logger.LogWarning("AcceptRide failed: Ride not found or already accepted.");
                return;
            }
            ride.DriverId = driverId;
            ride.Status = RideStatus.Accepted;
            await _rideService.UpdateRide(ride);

            _logger.LogInformation("Driver {DriverId} accepted ride {RideId}", driverId, rideId);
            await Clients.Group($"User_{ride.PassengerId}").SendAsync("RideAccepted", new{
                RideId = ride.Id,
                DriverId = ride.DriverId,
                DriverName = ride.RideDriver?.Name,
                VehicleNumber = driver.DriverVehicle?.Number,
                VehicleModel = driver.DriverVehicle?.Model
            });


            if (_userRideAvailableDrivers.TryGetValue(rideId, out var otherDrivers))
            {
                foreach (var (otherDriverId, _) in otherDrivers)
                {
                    if (otherDriverId == driverId) continue; 

                    var connectionId = TaxiBookingHub._availableConnections
                        .FirstOrDefault(x => x.Value.DriverId == otherDriverId).Key;

                    if (connectionId != null)
                    {
                        await Clients.Client(connectionId).SendAsync("RideAlreadyAccepted", new
                        {
                            RideId = ride.Id
                        });
                    }
                }

                // Optionally clear it after notifying
                TaxiBookingHub._userRideAvailableDrivers.Remove(rideId);
            }

        }

        public async Task RejectRide(int rideId)
        {
            var driverIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (driverIdStr == null)
            {
                _logger.LogWarning("RejectRide failed: Driver not authenticated.");
                return;
            }

            _logger.LogInformation("Driver {DriverId} rejected ride {RideId}", driverIdStr, rideId);
        }

        

        //invoked when client disconnected from hub
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _availableConnections.Remove(Context.ConnectionId);
            _logger.LogInformation("Client disconnected. ConnectionId: {ConnectionId}, Reason: {Reason}", Context.ConnectionId, exception?.Message ?? "Normal disconnect");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
