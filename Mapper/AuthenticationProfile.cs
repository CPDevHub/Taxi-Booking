using AutoMapper;
using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Models;

namespace Taxi_Booking.Profiles
{
    //A Profile in AutoMapper is a class that encapsulates mapping configuration.
    public class AuthenticationProfile:Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<PassengerRegisterDto, Passenger>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<Passenger, PassengerRegisterDto>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));

            CreateMap<DriverRegisterDto, Driver>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<Driver, DriverRegisterDto>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));

        }
    }
}
