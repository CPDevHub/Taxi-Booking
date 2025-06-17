using Taxi_Booking.DTO;
using Taxi_Booking.Models;

namespace Taxi_Booking.Repositories.Vehicles
{
    public interface IVehicleRepository
    {
        public Task<Vehicle> RegisterVechicle(VehicleDto vehicle);
        public Task<Vehicle> GetVehicleByNumber(string number);
    }
}
