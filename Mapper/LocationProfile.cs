using AutoMapper;
using Taxi_Booking.DTO;
using Taxi_Booking.Models;

namespace Taxi_Booking.Profiles
{
    public class LocationProfile:Profile
    {
        public LocationProfile()
        {
            CreateMap<CreateRideRequestDto, LatLng>();
            CreateMap<Location, LatLng>();
        }
    }
}
