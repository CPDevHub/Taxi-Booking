using AutoMapper;
using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.DTO.Responses;
using Taxi_Booking.Models;

namespace Taxi_Booking.Profiles
{
    public class ResponseProfile:Profile
    {
        public ResponseProfile()
        {
            CreateMap<Passenger,PassengerResponseDto>();
            CreateMap<Driver, DriverResponseDto>();

        }
    }
}
