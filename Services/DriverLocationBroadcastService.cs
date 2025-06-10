using Microsoft.AspNetCore.SignalR;
using Microsoft.Win32;
using Taxi_Booking.Hubs;

namespace Taxi_Booking.Services
{
    public class DriverLocationBroadcastService:BackgroundService
    {
        private readonly IHubContext<TaxiBookingHub> _hubContext;

        //ASP.NET Core automatically performs dependency injection(like IHubContext<T>) for your DriverLocationBroadcastService when you register it as a hosted service using AddHostedService<T>().
        public DriverLocationBroadcastService(IHubContext<TaxiBookingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _hubContext.Clients.Group("AvailableDrivers").SendAsync("SendLocation");

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

    }
}
