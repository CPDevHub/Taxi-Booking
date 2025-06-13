using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Taxi_Booking.DTO;
using Taxi_Booking.Exceptions;
using Taxi_Booking.Hubs;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Services.Rides;
using Taxi_Booking.Services.SignarRServices;

namespace Taxi_Booking.Controllers
{
    [Route("api/v1/ride")]
    [ApiController]
    public class RideController:ControllerBase
    {
        
        private readonly ILogger<RideController> _logger;
        private readonly IRideService _rideService;
        private readonly ISignalRService _signalrService;

        public RideController(IRideService rideService, ILogger<RideController> logger, ISignalRService signalrService)
        {
            _rideService = rideService;
            _logger = logger;
            _signalrService = signalrService;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookRide([FromBody] CreateRideRequestDto rideRequest)
        {
            
            var passengerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(passengerId!=null) rideRequest.PassengerId = Convert.ToInt32(passengerId);

            Ride ride=await _rideService.CreateRide(rideRequest);
            await _signalrService.NotifyNearByDrivers(ride);
            return Ok(new
            {
                rideId = ride.Id,
                status = ride.Status,
                message = "Ride request created. Waiting for driver to accept..."
            });
        }
    }
}
