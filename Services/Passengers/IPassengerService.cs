using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Models;

namespace Taxi_Booking.Services.Passengers
{
    public interface IPassengerService
    {
        public Task<Passenger> GetPassengerByEmailAsync(string email);
        public Task<Boolean> CreatePassengerAsync(PassengerRegisterDto passenger);

        public Task<Passenger> GetPassengerByIdAsync(int passengerId);
        public Task<Boolean> UpdatePassenger(Passenger passenger);

        
    }
}
