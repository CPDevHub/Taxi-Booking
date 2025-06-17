using Taxi_Booking.DTO;
using Taxi_Booking.Exceptions;
using Taxi_Booking.Models;
using Taxi_Booking.Repositories.Vehicles;
using Taxi_Booking.Services.Passengers;

namespace Taxi_Booking.Services.Vehicles
{
    public class VehicleService:IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ILogger<PassengerService> _logger;
        public VehicleService(IVehicleRepository vehicleRepository, ILogger<PassengerService> logger)
        {
            _logger = logger;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Vehicle> RegisterVechicle(VehicleDto vehicle)
        {
            var vehicleInDb = await GetVehicleByNumber(vehicle.Number);
            if(vehicleInDb!=null) throw new AlreadyExistsException("Vehicle with this number already registered.");
            return await _vehicleRepository.RegisterVechicle(vehicle);
        }
        public Task<Vehicle> GetVehicleByNumber(string number)
        {
            return _vehicleRepository.GetVehicleByNumber(number);
        }
    }
}
