using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Repositories.Passengers;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Exceptions;
using Taxi_Booking.Helpers;

namespace Taxi_Booking.Services.Passengers
{
    public class PassengerService:IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly ILogger<PassengerService> _logger;
        private readonly PasswordHasher _passwordHasher;
        public PassengerService(IPassengerRepository passengerRepository, ILogger<PassengerService> logger, PasswordHasher passwordHasher)
        {
            _passengerRepository = passengerRepository;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }
        public async Task<Passenger> GetPassengerByEmailAsync(string email)
        {
            return await _passengerRepository.GetPassengerByEmailAsync(email);
        }
        public async Task<Boolean> CreatePassengerAsync(PassengerRegisterDto passenger)
        {
            var passengerInDb=await  GetPassengerByEmailAsync(passenger.Email);
            if (passengerInDb!=null) throw new AlreadyExistsException("User with this email already exists.");
           
            passenger.Password = _passwordHasher.Hash(passenger.Password);
            return await _passengerRepository.CreatePassengerAsync(passenger);
        }
    }
}
