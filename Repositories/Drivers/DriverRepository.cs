using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taxi_Booking.Context;
using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Models.Entities;
using Taxi_Booking.Models.Enums;

namespace Taxi_Booking.Repositories.Drivers
{
    public class DriverRepository:IDriverRepository
    {
        private readonly TaxiBookingContext _taxiContext;
        private readonly ILogger<DriverRepository> _logger;
        private readonly IMapper _mapper;
        public DriverRepository(TaxiBookingContext taxiContext, ILogger<DriverRepository> logger,IMapper mapper)
        {
            _taxiContext = taxiContext;
            _logger = logger;
            _mapper = mapper;
            
        }
        public async Task<Driver> GetDriverByEmailAsync(string Email)
        {
            return await _taxiContext.Driver.FirstOrDefaultAsync(driver=>driver.Email==Email);
        }

        public async Task<Boolean> CreateDriverAsync(Driver driver)
        {
            await _taxiContext.Driver.AddAsync(driver);
            await _taxiContext.SaveChangesAsync();
            return true;
        }

        public async Task<Boolean> UpdateDriverStatus(DriverStatus status, int? driverId)
        {
            var driver = await _taxiContext.Driver.FindAsync(driverId);
            if (driver != null)
            {
                driver.Status = status;
                await _taxiContext.SaveChangesAsync();
            }
            return true;
        }
    }
}
