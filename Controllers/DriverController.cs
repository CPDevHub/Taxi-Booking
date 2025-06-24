using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Claims;
using Taxi_Booking.Constants;
using Taxi_Booking.Services.Drivers;
using Taxi_Booking.Services.Rides;

namespace Taxi_Booking.Controllers
{

    [ApiController]
    public class DriverController:ControllerBase
    {
        private readonly IRideService _rideService;
        private readonly IDriverService _driverService;
        public DriverController(IRideService rideService, IDriverService driverService)
        {
            _rideService = rideService;
            _driverService = driverService;
        }

        [HttpGet(ApiRoutes.Driver.History)]
        public async Task<IActionResult> GetHistory()
        {
            string driverIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(driverIdStr, out int driverId))
                return Unauthorized("Invalid user ID.");

            var history = await _rideService.GetHistoryDriverAsync(driverId);
            return Ok(history);
        }

        [HttpGet(ApiRoutes.Driver.Dashboard)]
        public async Task<IActionResult> GetDashboardData()
        {
            var driverIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(driverIdStr, out var driverId))
                return Unauthorized();

            var dashboard = await _driverService.GetDashboardAsync(driverId);
            return Ok(dashboard);
        }

        [HttpGet(ApiRoutes.Driver.Settings)]
        public async Task<IActionResult> GetCurrentDetails()
        {
            var driverIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(driverIdStr, out var driverId))
                return Unauthorized();

            var details = await _driverService.GetDetailsAsync(driverId);
            return Ok(details);
        }

        [HttpPost(ApiRoutes.Driver.SubmitRating)]
        public async Task<IActionResult> SubmitRating(int driverId, [FromBody] int rating)
        {
            if (rating < 1 || rating > 5)
                return BadRequest("Rating must be between 1 and 5");

            bool success = await _driverService.SubmitRatingAsync(driverId, rating);

            if (!success)
                return NotFound("Driver not found");

            return Ok(new { message = "Rating submitted successfully" });
        }

        [HttpGet(ApiRoutes.Driver.location)]
        public async Task<IActionResult> GetCurrentDriverLocation(int driverId)
        {
            var location = await _driverService.GetCurrentDriverLocationAsync(driverId);

            if (location == null)
                return NotFound("Driver location not found.");

            return Ok(location);
        }
    }
}
