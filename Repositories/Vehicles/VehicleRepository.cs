using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taxi_Booking.Context;
using Taxi_Booking.DTO;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Repositories.Passengers;

namespace Taxi_Booking.Repositories.Vehicles
{
    public class VehicleRepository:IVehicleRepository
    {
        private readonly TaxiBookingContext _taxiContext;
        private readonly ILogger<VehicleRepository> _logger;
        private readonly IMapper _mapper;
        public VehicleRepository(TaxiBookingContext taxiContext, ILogger<VehicleRepository> logger, IMapper mapper)
        {
            _taxiContext = taxiContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Vehicle> RegisterVechicle(VehicleDto vehicle)
        {
            var newVehicle = _mapper.Map<Vehicle>(vehicle);
            await _taxiContext.Vehicle.AddAsync(newVehicle);
            await _taxiContext.SaveChangesAsync();
            _logger.LogInformation("Vehicle registered successfully with number: {Number}, ID: {VehicleId}", newVehicle.Number, newVehicle.Id);
            return newVehicle;
        }
        public async Task<Vehicle> GetVehicleByNumber(string number)
        {
            _logger.LogInformation("Fetching vehicle by number: {Number}", number);
            return await _taxiContext.Vehicle.FirstOrDefaultAsync(vehicle => vehicle.Number == number);
        }
    }
}
