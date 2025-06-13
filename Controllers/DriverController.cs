using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taxi_Booking.Services.Rides;

namespace Taxi_Booking.Controllers
{

    [Route("api/v1/driver")]
    [ApiController]
    public class DriverController:ControllerBase
    {
        private readonly IRideService _rideService;
        public DriverController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            string driverIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(driverIdStr, out int driverId))
                return Unauthorized("Invalid user ID.");

            var history = await _rideService.GetHistoryDriverAsync(driverId);
            return Ok(history);

        }
    }
}
