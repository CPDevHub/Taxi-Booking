
using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Exceptions;
using Taxi_Booking.Helpers;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Repositories.Drivers;
using Taxi_Booking.Services.Vehicles;
using Taxi_Booking.DTO;
using AutoMapper;
using Taxi_Booking.Models.Enums;

namespace Taxi_Booking.Services.Drivers
{
    public class DriverService:IDriverService
    {

        private readonly IDriverRepository _driverRepository;
        private readonly ILogger<DriverService> _logger;
        private readonly PasswordHasher _passwordHasher;
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;
        public DriverService(IDriverRepository driverRepository, ILogger<DriverService> logger, PasswordHasher passwordHasher, IVehicleService vehicleService, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<Driver> GetDriverByEmailAsync(string email)
        {
            _logger.LogInformation("Service: Retrieving driver by email: {Email}", email);
            return await _driverRepository.GetDriverByEmailAsync(email);
        }
        public async Task<Boolean> CreateDriverAsync(DriverRegisterDto driver)
        {
            _logger.LogInformation("Service: Attempting to register driver with email: {Email}", driver.Email);
            var driverInDb = await GetDriverByEmailAsync(driver.Email);
            if (driverInDb != null)
            {
                _logger.LogWarning("Service: Driver already exists with email: {Email}", driver.Email);
                throw new AlreadyExistsException("User with this email already exists.");
            }
            _logger.LogInformation("Service: Registering vehicle with number: {Number}", driver.VehicleNumber);
            Vehicle vehicleRegistered=await _vehicleService.RegisterVechicle(new VehicleDto { Number=driver.VehicleNumber,Model=driver.VehicleModel,Type=driver.DriverVehicleType});

            Driver driverToRegister = _mapper.Map<Driver>(driver);
            driverToRegister.PasswordHash = _passwordHasher.Hash(driver.Password);
            driverToRegister.DriverVehicle = vehicleRegistered;
            driverToRegister.VehicleId = vehicleRegistered.Id;
            driverToRegister.Status = DriverStatus.Unavailable;
            _logger.LogInformation("Service: Saving new driver with email: {Email}", driver.Email);
            return await _driverRepository.CreateDriverAsync(driverToRegister);
        }

        public Task<Boolean> UpdateDriverStatus(DriverStatus status, int? driverId)
        {
            _logger.LogInformation("Service: Updating driver status to {Status} for driver ID: {DriverId}", status, driverId);
            return _driverRepository.UpdateDriverStatus(status, driverId);
        }

        public Task<Driver> GetDriverByIdAsync(int driverId)
        {
            return _driverRepository.GetDriverByIdAsync(driverId);
        }
        public async Task<Boolean> UpdateDriverLocation(int driverId, double latitude, double longitude)
        {
            return await _driverRepository.UpdateDriverLocation(driverId, latitude, longitude);
        }

        public async Task<Driver> GetDriverWithVehicleByIdAsync(int driverId)
        {
            return await _driverRepository.GetDriverWithVehicleByIdAsync(driverId);
        }
    }
}
