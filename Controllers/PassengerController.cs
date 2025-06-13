using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taxi_Booking.Services.Rides;

namespace Taxi_Booking.Controllers
{
    [Route("api/v1/passenger")]
    [ApiController]
    public class PassengerController:ControllerBase
    {
        private readonly IRideService _rideService;
        public PassengerController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            string passengerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(passengerIdString, out int passengerId))
                return Unauthorized("Invalid user ID.");

            var history = await _rideService.GetHistoryPassengerAsync(passengerId);
            return Ok(history);

        }

    }
}
