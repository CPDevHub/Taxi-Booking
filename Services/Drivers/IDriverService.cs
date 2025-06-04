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
    }
}
