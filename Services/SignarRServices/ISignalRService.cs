using Taxi_Booking.Models;

namespace Taxi_Booking.Services.SignarRServices
{
    public interface ISignalRService
    {
        public Task NotifyNearByDrivers(Ride ride);
    }
}
