using Taxi_Booking.Models.Entities;
using Taxi_Booking.Models.Enums;


namespace Taxi_Booking.Repositories.Drivers
{
    public interface IDriverRepository
    {
        public Task<Driver> GetDriverByEmailAsync(string Email);
        public Task<Boolean> CreateDriverAsync(Driver driver);

        public Task<Boolean> UpdateDriverStatus(DriverStatus status, int? driverId);
    }
}
