using Taxi_Booking.DTO;
using Taxi_Booking.Models;


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
        public Task<DriverDashboardDto> GetDashboardAsync(int driverId);

        public Task<DriverDetailsDto> GetDetailsAsync(int driverId);
        public Task<bool> SubmitRatingAsync(int driverId, int rating);

        Task<LatLng> GetCurrentDriverLocationAsync(int driverId);
    }
}
