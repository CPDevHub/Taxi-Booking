using AutoMapper;
using Taxi_Booking.DTO;
using Taxi_Booking.Models;

namespace Taxi_Booking.Profiles
{
    public class RideProfile:Profile
    {
        public RideProfile()
        {
            CreateMap<Ride,RideHistoryDto>().ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.PickupLocation.Address))
                .ForMember(dest => dest.DropoffLocation, opt => opt.MapFrom(src => src.DropOffLocation.Address));

        }
    }
}
