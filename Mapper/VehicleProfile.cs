using AutoMapper;
using Taxi_Booking.DTO;
using Taxi_Booking.Models;

namespace Taxi_Booking.Profiles
{
    public class VehicleProfile:Profile
    {
        public VehicleProfile()
        {
            CreateMap<VehicleDto, Vehicle>();
        }
        
    }
}
