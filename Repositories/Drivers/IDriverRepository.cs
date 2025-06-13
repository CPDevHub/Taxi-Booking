using Taxi_Booking.Models.Entities;
using Taxi_Booking.Models.Enums;


namespace Taxi_Booking.Repositories.Drivers
{
    public interface IDriverRepository
    {
        public Task<Driver> GetDriverByEmailAsync(string Email);
        public Task<Boolean> CreateDriverAsync(Driver driver);

        public Task<Boolean> UpdateDriverStatus(DriverStatus status, int? driverId);

        public Task<Driver> GetDriverByIdAsync(int driverId);
        public Task<Boolean> UpdateDriverLocation(int driverId, double latitude, double longitude);
        public Task<Driver> GetDriverWithVehicleByIdAsync(int driverId);

        public Task<Boolean> UpdateDriver(Driver driver);
    }
}
