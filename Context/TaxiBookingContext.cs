using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Taxi_Booking.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Taxi_Booking.Context
{
    public class TaxiBookingContext:DbContext
    {

        public DbSet<Driver> Driver { get; set; }
        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<Ride> Ride { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }


        public TaxiBookingContext(DbContextOptions<TaxiBookingContext> options) : base(options){ }


        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Vehicle>().Property(vehicle => vehicle.Type).HasConversion<string>();
            modelBuilder.Entity<Ride>().Property(ride => ride.Status).HasConversion<string>();
            modelBuilder.Entity<Driver>().Property(driver => driver.Status).HasConversion<string>();



            //must tell EF explicitly that Location is an owned type, meaning “Location is not an entity with its own table, it belongs to Journey, and its properties should be stored in the same table.”
            modelBuilder.Entity<Ride>().OwnsOne(ride => ride.PickupLocation);
            modelBuilder.Entity<Ride>().OwnsOne(ride => ride.DropOffLocation);
            modelBuilder.Entity<Driver>().OwnsOne(driver => driver.Location);

            modelBuilder.Entity<Passenger>().Property(p => p.CancellationCharges).HasDefaultValue(0);
            modelBuilder.Entity<Ride>().Property(r => r.CancellationCharges).HasDefaultValue(0);
            modelBuilder.Entity<Driver>().Property(d => d.TotalRides).HasDefaultValue(0);


        }

    }
}
