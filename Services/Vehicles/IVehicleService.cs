using Taxi_Booking.DTO;
using Taxi_Booking.Models;

namespace Taxi_Booking.Services.Vehicles
{
    public interface IVehicleService
    {
        public Task<Vehicle> RegisterVechicle(VehicleDto vehicle);
        public Task<Vehicle> GetVehicleByNumber(string number);
    }
}
