using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Models.Entities;


namespace Taxi_Booking.Repositories.Passengers
{
    public interface IPassengerRepository
    {
        public Task<Passenger> GetPassengerByEmailAsync(string email);
        public Task<Boolean> CreatePassengerAsync(PassengerRegisterDto passenger);
        public Task<Passenger> GetPassengerByIdAsync(int passengerId);
        public Task<Boolean> UpdatePassenger(Passenger passenger);
    }
}
