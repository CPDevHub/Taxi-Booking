using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using Taxi_Booking.Constants;
using Taxi_Booking.Helpers;
using Taxi_Booking.Models;
using Taxi_Booking.Services.Drivers;
using Taxi_Booking.Services.Passengers;
using Taxi_Booking.Services.Rides;
using Taxi_Booking.Services.SignarRServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Taxi_Booking.Hubs
{
    public class TaxiBookingHub:Hub
    {
        private readonly IDriverService _driverService;
        private readonly IPassengerService _passengerService;
        private readonly IRideService _rideService;
        private readonly ISignalRService _signalrService;
        private readonly ILogger<TaxiBookingHub> _logger;
        public static Dictionary<string, (int DriverId, LatLng Location)> _availableConnections = new();
        public static Dictionary<int, List<int>> _userRideAvailableDrivers = new();
        public static Dictionary<int, string> PassengerConnections = new();
        public static Dictionary<int, string> DriverConnections = new();
        public static Dictionary<int, List<int> > _rideCancelledDrivers = new();
        public TaxiBookingHub(IDriverService driverService, ILogger<TaxiBookingHub> logger,IRideService rideService, IPassengerService passengerService, ISignalRService signalrService)
        {
            _passengerService = passengerService;
            _driverService = driverService;
            _logger = logger;
            _rideService = rideService;
            _signalrService = signalrService;
        }
        //invoked when cleint connected to hub 
        //SignalR groups are connection-based, not user-based
        //When a client connects and joins a group(e.g., "AvailableDrivers"), it does so via its SignalR connection ID.
        //If that connection closes, due to:logout,network drop,tab closed ,crash, Then SignalR automatically removes that connection from any groups it was in.
        //On reconnect(after login or reconnection), SignalR creates a new ConnectionId, so you’ll need to re-add the driver to the group:
        //Hub instances are transient, created per connection.
        //Also, Hub instances cannot be injected into other services because their lifetime is tied to the SignalR pipeline and client calls.
        [Authorize]
        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected");

            var role = Context.User.FindFirst(ClaimTypes.Role)?.Value;
            var userId =Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("Connected client: Id = {Id}, Role = {Role}", userId, role);


            if (role == "Driver" && int.TryParse(userId, out var driverId))
                await LoginDriver(driverId);
            else if (role == "Passenger" && int.TryParse(userId, out var passengerId))
                await LoginPassenger(passengerId);

            await base.OnConnectedAsync();
        }


        public async Task LoginDriver(int driverId)
        {
            var driver = await _driverService.GetDriverByIdAsync(Convert.ToInt32(driverId));
            if (driver != null && driver.Status == DriverStatus.Available)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "AvailableDrivers");
                DriverConnections[driverId] = Context.ConnectionId;
                _logger.LogInformation("Driver {DriverId} added to group 'AvailableDrivers'", driverId);

                await Clients.Client(Context.ConnectionId).SendAsync(SignalREvents.SendLocation);
                _logger.LogInformation("Sent initial SendLocation event to Driver {DriverId}", driverId);
            }
        }

        public async Task LoginPassenger(int passengerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{passengerId}");
            PassengerConnections[passengerId] = Context.ConnectionId;
            _logger.LogInformation("Passenger {UserId} added to group User_{UserId}", passengerId, passengerId);
        }

        public async Task UpdateStatus(string statusStr)
        {
            if (!Enum.TryParse<DriverStatus>(statusStr, out var status))
            {
                return;
            }

            var idStr = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idStr, out var driverId)) return;
            if (status == DriverStatus.Available)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "AvailableDrivers");
                DriverConnections[driverId] = Context.ConnectionId;
                await Clients.Client(Context.ConnectionId).SendAsync(SignalREvents.SendLocation);
            }
            else _availableConnections.Remove(Context.ConnectionId);
            await _driverService.UpdateDriverStatus(status, driverId);
        }

        public async Task UpdateLocation(LatLng dto)
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

        public async Task RideStart(int rideId)
        {
            var driverIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (driverIdStr == null)
            {
                _logger.LogWarning("AcceptRide failed: Driver not authenticated.");
                return;
            }
            int driverId = Convert.ToInt32(driverIdStr);
            Ride ride = await _rideService.GetRideByID(rideId);
            ride.Status = RideStatus.OnGoing;
            ride.RideStartAt = DateTime.UtcNow;
            _rideService.UpdateRide(ride);

            string passengerConnectionId = PassengerConnections[ride.PassengerId];
            if (passengerConnectionId != null)
            {
                await Clients.Client(passengerConnectionId).SendAsync(SignalREvents.RideStart);
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
            if (ride.Status == RideStatus.Accepted)
            {
                _logger.LogWarning("AcceptRide failed: Ride Already Accepted.");
                await Clients.Caller.SendAsync(SignalREvents.RideAlreadyAccepted, new
                {
                    RideId = ride.Id
                });
            }
            Driver driver = await _driverService.GetDriverWithVehicleByIdAsync(driverId);
            var passenger = await _passengerService.GetPassengerByIdAsync(ride.PassengerId);

            ride.DriverId = driverId;
            ride.Status = RideStatus.Accepted;
            await _rideService.UpdateRide(ride);
            await _driverService.UpdateDriverStatus(DriverStatus.Busy, driverId);

            _logger.LogInformation("Driver {DriverId} accepted ride {RideId}", driverId, rideId);
            await Clients.Group($"User_{ride.PassengerId}").SendAsync(SignalREvents.RideAcceptedUserNotify, new
            {
                RideId = ride.Id,
                DriverId = ride.DriverId,
                DriverName = ride.RideDriver?.Name,
                VehicleNumber = driver.DriverVehicle?.Number,
                VehicleModel = driver.DriverVehicle?.Model,
                fare = ride.TotalFare,
                previousCancellationCharges = passenger.CancellationCharges
            });


            if (_userRideAvailableDrivers.TryGetValue(rideId, out var otherDrivers))
            {
                foreach (var otherDriverId in otherDrivers)
                {
                    if (otherDriverId == driverId) continue; 

                    var connectionId = TaxiBookingHub._availableConnections
                        .FirstOrDefault(x => x.Value.DriverId == otherDriverId).Key;

                    if (connectionId != null)
                    {
                        await Clients.Client(connectionId).SendAsync(SignalREvents.RideAlreadyAccepted, new
                        {
                            RideId = ride.Id
                        });
                    }
                }

                var driverConnectionId = TaxiBookingHub._availableConnections
                        .FirstOrDefault(x => x.Value.DriverId == driverId).Key;
                await Clients.Client(driverConnectionId).SendAsync(SignalREvents.RideAcceptedDriverNotify, new {
                    RideId = ride.Id,
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
                    PassengerName = passenger.Name,
                    ContactNumber = passenger.ContactNumber,
                    fare = ride.TotalFare,
                    previousCancellationCharges = passenger.CancellationCharges
                });

                TaxiBookingHub._userRideAvailableDrivers.Remove(rideId);
            }

        }

        public async Task CancelRideBeforeAcceptance(int rideId)
        {
            if (!TaxiBookingHub._userRideAvailableDrivers.TryGetValue(rideId, out var driverIds))
                return;

            Ride ride = await _rideService.GetRideByID(rideId);
            ride.Status = RideStatus.Cancelled;
            foreach (var driverId in driverIds)
            {
                var connection = TaxiBookingHub._availableConnections
                    .FirstOrDefault(d => d.Value.DriverId == driverId);

                if (!string.IsNullOrEmpty(connection.Key))
                {
                    await Clients.Client(connection.Key)
                        .SendAsync(SignalREvents.RideCancelledByPassengerBeforeAccept, new
                        {
                            RideId = rideId
                        });
                }
            }
            TaxiBookingHub._userRideAvailableDrivers.Remove(rideId);
        }


        public async Task CompleteRide(int rideId)
        {
            var driverIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (driverIdStr == null)
            {
                _logger.LogWarning("CancelRide failed: Driver not authenticated.");
                return;
            }
            int driverId = Convert.ToInt32(driverIdStr);
            Ride ride = await _rideService.GetRideByID(rideId);
            ride.Status = RideStatus.Completed;
            ride.RideEndAt = DateTime.UtcNow;
            Driver driver = await _driverService.GetDriverByIdAsync(driverId);

            driver.TotalRides++;
            driver.Status = DriverStatus.Available;
            driver.TotalEarnings += ride.TotalFare;

            await _driverService.UpdateDriver(driver);

            string passengerConnectionId = PassengerConnections[ride.PassengerId];
            if (passengerConnectionId != null)
            {
                await Clients.Client(passengerConnectionId).SendAsync(SignalREvents.RideCompleted);
            }


        }

        public async Task CancelRideByDriver(int rideId)
        {
            var driverIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (driverIdStr == null)
            {
                _logger.LogWarning("CancelRide failed: Driver not authenticated.");
                return;
            }
            int driverId = Convert.ToInt32(driverIdStr);
            Ride ride = await _rideService.GetRideByID(rideId);

            if (!_rideCancelledDrivers.ContainsKey(rideId))
                _rideCancelledDrivers[rideId] = new List<int>();

            _rideCancelledDrivers[rideId].Add(driverId);

            ride.Status = RideStatus.Cancelled;
            ride.DriverId = null;
            await _rideService.UpdateRide(ride);
            await _driverService.UpdateDriverStatus(DriverStatus.Available, driverId);


            if (PassengerConnections.TryGetValue(ride.PassengerId, out var userConnectionId))
            {
                await Clients.Client(userConnectionId).SendAsync(SignalREvents.RideCancelledByDriver, new
                {
                    RideId = ride.Id,
                    Message = "Driver has cancelled your ride. Searching for another driver..."
                });
            }

            await _signalrService.NotifyNearByDrivers(ride);

        }

        public async Task CancelRideByPassenger(int rideId,string reason)
        {
            var passengerStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (passengerStr == null) 
            {
                _logger.LogWarning("CancelRide failed: Driver not authenticated.");
                return;
            }
            int passengerId = Convert.ToInt32(passengerStr);
            _logger.LogWarning("Passenger Authenticated with Id:{id} and doe cancellation Reason:{reason}", passengerId,reason) ;

            Ride ride = await _rideService.GetRideByID(rideId);
            var passenger = await _passengerService.GetPassengerByIdAsync(ride.PassengerId);
            ride.Status = RideStatus.Cancelled;
            ride.CancellationReason = reason;
            await _rideService.UpdateRide(ride);
            await _driverService.UpdateDriverStatus(DriverStatus.Available, ride.DriverId);


            passenger.CancellationCharges = 0.05 * ride.TotalFare;
            await _passengerService.UpdatePassenger(passenger);

            
            int driverId = ride.DriverId.Value;
            string driverConnectionId = DriverConnections[driverId];
            await Clients.Client(driverConnectionId).SendAsync(SignalREvents.RideCancelledByPassenger, new
            {
                RideId = ride.Id,
                Message = "Passenger has cancelled your ride"
            });

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
