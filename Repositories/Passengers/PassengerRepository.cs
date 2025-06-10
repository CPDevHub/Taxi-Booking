using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taxi_Booking.Context;
using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Models.Entities;

namespace Taxi_Booking.Repositories.Passengers
{
    public class PassengerRepository:IPassengerRepository
    {
        private readonly TaxiBookingContext _taxiContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PassengerRepository> _logger;
        public PassengerRepository(TaxiBookingContext taxiContext, ILogger<PassengerRepository> logger,IMapper mapper)
        {
            _taxiContext = taxiContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Passenger> GetPassengerByEmailAsync(string email)
        {
            _logger.LogInformation("Fetching passenger by email: {Email}", email);
            return await _taxiContext.Passenger.FirstOrDefaultAsync(passenger => passenger.Email == email);
        }
        public async Task<Boolean> CreatePassengerAsync(PassengerRegisterDto passenger)
        {
            var newPassenger = _mapper.Map<Passenger>(passenger);
            await _taxiContext.Passenger.AddAsync(newPassenger);
            await _taxiContext.SaveChangesAsync();
            _logger.LogInformation("Passenger created with ID: {PassengerId}, Email: {Email}", newPassenger.Id, newPassenger.Email);
            return true;
        }
    }
}
