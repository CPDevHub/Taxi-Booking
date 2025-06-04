
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
        private readonly ILogger<DriverRepository> _logger;
        private readonly PasswordHasher _passwordHasher;
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;
        public DriverService(IDriverRepository driverRepository, ILogger<DriverRepository> logger, PasswordHasher passwordHasher, IVehicleService vehicleService, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<Driver> GetDriverByEmailAsync(string Email)
        {
            return await _driverRepository.GetDriverByEmailAsync(Email);
        }
        public async Task<Boolean> CreateDriverAsync(DriverRegisterDto driver)
        {
            var driverInDb = await GetDriverByEmailAsync(driver.Email);
            if (driverInDb != null) throw new AlreadyExistsException("User with this email already exists.");

            Vehicle vehicleRegistered=await _vehicleService.RegisterVechicle(new VehicleDto { Number=driver.VehicleNumber,Model=driver.VehicleModel,Type=driver.DriverVehicleType});

            Driver driverToRegister = _mapper.Map<Driver>(driver);
            driverToRegister.PasswordHash = _passwordHasher.Hash(driver.Password);
            driverToRegister.DriverVehicle = vehicleRegistered;
            driverToRegister.VehicleId = vehicleRegistered.Id;
            driverToRegister.Status = DriverStatus.Unavailable;

            return await _driverRepository.CreateDriverAsync(driverToRegister);
        }

        public Task<Boolean> UpdateDriverStatus(DriverStatus status, int? driverId)
        {
            return _driverRepository.UpdateDriverStatus(status, driverId);
        }
    }
}
