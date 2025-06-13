using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Models.Enums;

namespace Taxi_Booking.Services.Drivers
{
    public interface IDriverService
    {
        public Task<Driver> GetDriverByEmailAsync(string Email);
        public Task<Boolean> CreateDriverAsync(DriverRegisterDto driver);

        public Task<Boolean> UpdateDriverStatus(DriverStatus status, int? driverId);

        public Task<Driver> GetDriverByIdAsync(int driverId);

        public Task<Driver> GetDriverWithVehicleByIdAsync(int driverId);

        public Task<Boolean> UpdateDriverLocation(int driverId, double latitude, double longitude);
        public Task<Boolean> UpdateDriver(Driver driver);
    }
}
