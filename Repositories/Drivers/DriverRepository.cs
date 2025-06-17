using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taxi_Booking.Context;
using Taxi_Booking.DTO;
using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Models;

namespace Taxi_Booking.Repositories.Drivers
{
    public class DriverRepository : IDriverRepository
    {
        private readonly TaxiBookingContext _taxiContext;
        private readonly ILogger<DriverRepository> _logger;
        private readonly IMapper _mapper;
        public DriverRepository(TaxiBookingContext taxiContext, ILogger<DriverRepository> logger, IMapper mapper)
        {
            _taxiContext = taxiContext;
            _logger = logger;
            _mapper = mapper;

        }
        public async Task<Driver> GetDriverByEmailAsync(string email)
        {
            _logger.LogInformation("Fetching driver by email: {Email}", email);
            return await _taxiContext.Driver.FirstOrDefaultAsync(driver => driver.Email == email);
        }

        public async Task<Boolean> CreateDriverAsync(Driver driver)
        {
            await _taxiContext.Driver.AddAsync(driver);
            await _taxiContext.SaveChangesAsync();
            _logger.LogInformation("Driver created with ID: {DriverId}, Email: {Email}", driver.Id, driver.Email);
            return true;
        }

        public async Task<Boolean> UpdateDriverStatus(DriverStatus status, int? driverId)
        {
            var driver = await _taxiContext.Driver.FindAsync(driverId);
            if (driver != null)
            {
                driver.Status = status;
                await _taxiContext.SaveChangesAsync();
                _logger.LogInformation("Updated status to {Status} for driver ID: {DriverId}", status, driver.Id);

            }
            return true;
        }

        public async Task<Driver> GetDriverByIdAsync(int driverId)
        {
            return await _taxiContext.Driver.FindAsync(driverId);
        }

        public async Task<Boolean> UpdateDriverLocation(int driverId, double latitude, double longitude)
        {
            var driver = await _taxiContext.Driver.FindAsync(driverId);
            if (driver == null)
                return false;

            if (driver.Location == null)
                driver.Location = new LatLng();

            driver.Location.Latitude = latitude;
            driver.Location.Longitude = longitude;

            await _taxiContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SubmitRatingAsync(int driverId, int rating)
        {
            var driver = await _taxiContext.Driver.FindAsync(driverId);
            if (driver == null)
                return false;
            var currentRating = driver.Rating ?? 0;
            driver.Rating = (currentRating*(driver.TotalRides-1) + rating) / driver.TotalRides;
           await _taxiContext.SaveChangesAsync();
            return true;
        }

        public async Task<Driver> GetDriverWithVehicleByIdAsync(int driverId)
        {
            return await _taxiContext.Driver.Include(d => d.DriverVehicle).FirstOrDefaultAsync(d => d.Id == driverId);
        }

        public async Task<Boolean> UpdateDriver(Driver driver)
        {
            _taxiContext.Update(driver);
            await _taxiContext.SaveChangesAsync();
            return true;
        }

        public async Task<DriverDashboardDto> GetDashboardAsync(int driverId)
        {
            var driver = await _taxiContext.Driver
           .Include(d => d.DriverVehicle)
           .FirstOrDefaultAsync(d => d.Id == driverId);

            return new DriverDashboardDto
            {
                TotalRides = driver.TotalRides,
                TotalEarnings = driver.TotalEarnings,
                CarType = driver.DriverVehicle.Type.ToString(),
                Status = driver.Status.ToString()
            };
        }

        public async Task<DriverDetailsDto> GetDetailsAsync(int driverId)
        {
            var driver = await _taxiContext.Driver
           .FirstOrDefaultAsync(d => d.Id == driverId);

            return new DriverDetailsDto
            {
                Email=driver.Email,
                ContactNumber=driver.ContactNumber,
                Status = driver.Status.ToString()
            };

        }
    }
}
